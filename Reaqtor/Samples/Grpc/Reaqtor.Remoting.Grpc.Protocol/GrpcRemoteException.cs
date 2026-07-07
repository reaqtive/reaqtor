// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

namespace Reaqtor.Remoting.Grpc.Protocol;

/// <summary>
/// The client-side rehydration of an exception that crossed a gRPC stream/command as an
/// <see cref="Contracts.ErrorInfo"/> (plan §6.1). Carries the original type name, remote stack trace, and the
/// transient flag so callers can reason about retriability without the original CLR type. (Full
/// <c>BaseException</c>-shaped reconstruction via fault interceptors is a later refinement.)
/// </summary>
public sealed class GrpcRemoteException : Exception
{
    /// <summary>The fully-qualified name of the original server-side exception type.</summary>
    public string RemoteTypeName { get; }

    /// <summary>The server-side stack trace, if any.</summary>
    public string RemoteStackTrace { get; }

    /// <summary>Whether the original fault was transient (retriable).</summary>
    public bool IsTransient { get; }

    public GrpcRemoteException()
    {
    }

    public GrpcRemoteException(string message)
        : base(message)
    {
    }

    public GrpcRemoteException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public GrpcRemoteException(string remoteTypeName, string message, string remoteStackTrace, bool isTransient)
        : base(message)
    {
        RemoteTypeName = remoteTypeName;
        RemoteStackTrace = remoteStackTrace;
        IsTransient = isTransient;
    }
}
