// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Reaqtor.Remoting.Grpc.Client;
using Reaqtor.Remoting.Grpc.Contracts;

namespace Reaqtor.Remoting.Platform.Grpc;

/// <summary>
/// Launches a Kestrel gRPC host exe out-of-process and waits for readiness via
/// <c>ReactiveServiceControl.Ping</c> (plan §7 — the retarget of the archived <c>ProcessRunnable</c>/
/// <c>TcpRunnable</c>). Disposing terminates the host process.
/// </summary>
public sealed class GrpcProcessRunnable : IDisposable
{
    private readonly Process _process;

    private GrpcProcessRunnable(Process process, int port)
    {
        _process = process;
        Port = port;
        Address = string.Create(CultureInfo.InvariantCulture, $"http://localhost:{port}");
    }

    /// <summary>The loopback gRPC address of the launched host (e.g. <c>http://localhost:8081</c>).</summary>
    public string Address { get; }

    /// <summary>The TCP port the host listens on.</summary>
    public int Port { get; }

    /// <summary>Reserves an ephemeral free TCP port on the loopback interface.</summary>
    public static int GetFreeTcpPort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        return ((IPEndPoint)listener.LocalEndpoint).Port;
    }

    /// <summary>
    /// Launches <c>dotnet &lt;hostAssemblyPath&gt; &lt;port&gt; [extraArgs…]</c>. If <paramref name="port"/> is
    /// null an ephemeral free port is chosen. <paramref name="extraArgs"/> are passed after the port (e.g. a
    /// standalone Messaging broker address for the query-evaluator host — Milestone 5).
    /// </summary>
    public static GrpcProcessRunnable Launch(string hostAssemblyPath, int? port = null, params string[] extraArgs)
    {
        if (string.IsNullOrEmpty(hostAssemblyPath))
            throw new ArgumentNullException(nameof(hostAssemblyPath));

        var resolvedPort = port ?? GetFreeTcpPort();

        var startInfo = new ProcessStartInfo("dotnet") { UseShellExecute = false };
        ScrubProfilerEnvironment(startInfo);
        startInfo.ArgumentList.Add(hostAssemblyPath);
        startInfo.ArgumentList.Add(resolvedPort.ToString(CultureInfo.InvariantCulture));

        if (extraArgs != null)
        {
            foreach (var arg in extraArgs)
            {
                startInfo.ArgumentList.Add(arg);
            }
        }

        var process = Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start the gRPC host process.");
        return new GrpcProcessRunnable(process, resolvedPort);
    }

    /// <summary>
    /// Environment variables through which a CLR profiler (and, specifically, the code-coverage
    /// instrumentation engine) attaches itself to a .NET process. Matched by prefix.
    /// </summary>
    private static readonly string[] ProfilerEnvironmentPrefixes =
    [
        "CORECLR_ENABLE_PROFILING",
        "CORECLR_PROFILER",
        "COR_ENABLE_PROFILING",
        "COR_PROFILER",
        "MicrosoftInstrumentationEngine_",
        "CODE_COVERAGE_",
        "MICROSOFT_CODE_COVERAGE",
    ];

    /// <summary>
    /// Prevents the launched host from inheriting the parent's CLR-profiler environment.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Under <c>dotnet test --coverage</c> the code-coverage collector attaches an instrumentation
    /// profiler to the test host via <c>CORECLR_*</c> environment variables. Because
    /// <see cref="ProcessStartInfo.UseShellExecute"/> is <c>false</c>, a child process inherits that
    /// environment and the profiler attaches to every short-lived gRPC host we spawn.
    /// </para>
    /// <para>
    /// On the hosted Linux CI agent that corrupts the *parent's* instrumented modules: once a child
    /// has run, the test host's first JIT of <c>Reaqtor.QueryEngine.ReactiveEntity.Cache</c> fails with
    /// <c>InvalidProgramException</c>. It reproduces only on that agent, only with <c>--coverage</c>, and
    /// only in this test project — the sole one that spawns child processes. The same instrumented
    /// method JITs correctly in test hosts that do not.
    /// </para>
    /// <para>
    /// Scrubbing the variables is right independently of the bug: these hosts are deployment artifacts
    /// under test, their coverage is not consumed by the run, and attaching an instrumentation engine to
    /// each one only slows the tests down.
    /// </para>
    /// </remarks>
    private static void ScrubProfilerEnvironment(ProcessStartInfo startInfo)
    {
        var environment = startInfo.Environment;

        var stale = new List<string>();

        foreach (var key in environment.Keys)
        {
            foreach (var prefix in ProfilerEnvironmentPrefixes)
            {
                if (key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    stale.Add(key);
                    break;
                }
            }
        }

        foreach (var key in stale)
        {
            environment.Remove(key);
        }
    }

    /// <summary>Polls <c>Ping</c> until the host answers or the timeout elapses.</summary>
    public async Task WaitForReadyAsync(TimeSpan timeout, CancellationToken token = default)
    {
        using var channel = GrpcConnectionFactory.CreateChannel(Address);
        var control = GrpcConnectionFactory.CreateControlClient(channel);

        var deadline = DateTime.UtcNow + timeout;
        Exception last = null;

        while (DateTime.UtcNow < deadline)
        {
            token.ThrowIfCancellationRequested();

            if (_process.HasExited)
            {
                throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture,
                    $"The gRPC host process exited early with code {_process.ExitCode} before becoming ready."));
            }

            try
            {
                await control.PingAsync(Empty.Instance).ConfigureAwait(false);
                return;
            }
#pragma warning disable CA1031 // Transient startup errors (connection refused, host not listening yet) are expected and retried.
            catch (Exception ex)
            {
                last = ex;
                await Task.Delay(100, token).ConfigureAwait(false);
            }
#pragma warning restore CA1031
        }

        throw new TimeoutException(string.Create(CultureInfo.InvariantCulture,
            $"The gRPC host at {Address} did not become ready within {timeout}."), last);
    }

    /// <summary>Terminates the host process (best effort).</summary>
    public void Dispose()
    {
        try
        {
            if (!_process.HasExited)
            {
                _process.Kill(entireProcessTree: true);
                if (!_process.WaitForExit(5000))
                {
                    // The kill did not take within the grace period; surface it rather than silently leaking a
                    // host process (review #13). Trace is the only diagnostic channel available to this library.
                    Trace.TraceWarning(string.Create(CultureInfo.InvariantCulture,
                        $"The gRPC host process (PID {_process.Id}) did not exit within 5s of Kill; it may be leaked."));
                }
            }
        }
#pragma warning disable CA1031 // Best-effort teardown of an external process; nothing actionable on failure.
        catch (Exception)
        {
        }
#pragma warning restore CA1031
        finally
        {
            _process.Dispose();
        }
    }
}
