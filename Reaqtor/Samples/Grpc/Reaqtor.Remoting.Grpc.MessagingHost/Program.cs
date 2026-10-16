// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Globalization;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ProtoBuf.Grpc.Server;

using Reaqtor.Remoting.Grpc.Server;
using Reaqtor.Remoting.Messaging;
using Reaqtor.Remoting.Protocol;

//
// Standalone Messaging broker host (plan §4.3 / Milestone 5): promotes the in-host broker to its own process. It
// runs ONLY the broker — a MessagingConnection (the ConcurrentDictionary<topic, …> fan-out kernel) behind the gRPC
// Messaging service — with no engine. The query-evaluator host (its firehose legs) and external clients all connect
// here, so a Publish on any connection fans out to every Subscribe stream for the topic across processes. The port
// is args[0] (default 8086, the §7 Messaging port); h2c, as for the other hosts.
//
var port = args.Length > 0 && int.TryParse(args[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var p)
    ? p
    : 8086;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
    options.ListenLocalhost(port, listen => listen.Protocols = HttpProtocols.Http2));

builder.Services.AddCodeFirstGrpc();

// The single shared broker connection for this host (one process => one broker, the §3.6 invariant is automatic).
builder.Services.AddSingleton<IReactiveMessagingConnection>(new MessagingConnection());
builder.Services.AddSingleton<MessagingGrpcAdapter>();
builder.Services.AddSingleton<ReactiveServiceControlAdapter>();

var app = builder.Build();

app.MapGrpcService<MessagingGrpcAdapter>();
app.MapGrpcService<ReactiveServiceControlAdapter>();

app.Run();
