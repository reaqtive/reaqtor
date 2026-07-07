// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

using global::Grpc.Core;

using Reaqtor.Remoting.Grpc.Contracts;

namespace Reaqtor.Remoting.Grpc.Protocol
{
    //
    // Fault marshaling for the unary store RPCs (plan §6.1/§6.2). A server-side handler converts a thrown CLR
    // exception into an RpcException whose Status.Detail carries the message and whose trailers carry the §6.1
    // ErrorInfo fields (type name, transient flag, stack). The client adapter converts that RpcException back into
    // a CLR exception, RE-RAISING the original type for the engine's exception-contract types (§6.2: the KeyValueStore
    // indexer's absent-key fault must reach the engine as the same type it would in-proc). Unknown types fall back to
    // GrpcRemoteException. This is the typed-store counterpart of NotificationConverter's ErrorInfo bridge for the
    // streaming OnError path.
    //
    /// <summary>Converts exceptions to/from <see cref="RpcException"/> across the typed store RPCs (§6.1/§6.2).</summary>
    public static class GrpcFault
    {
        // ASCII-safe trailer keys (CLR type names and the flag are ASCII; the message rides in Status.Detail and the
        // stack in a binary "-bin" trailer, so non-ASCII content never breaks Metadata's ASCII-value rule).
        private const string TypeNameKey = "error-type-name";
        private const string TransientKey = "error-is-transient";
        private const string StackKey = "error-stack-bin";

        /// <summary>Server side: pack an exception into an <see cref="RpcException"/> with §6.1 ErrorInfo trailers.</summary>
        public static RpcException ToRpcException(Exception exception)
        {
            ArgumentNullException.ThrowIfNull(exception);

            var info = NotificationConverter.ToErrorInfo(exception);

            // §6.2 status mapping: a missing-key lookup is NotFound; argument faults (incl. ArgumentNullException, a
            // subclass) are InvalidArgument; transient faults are retriable (Unavailable); everything else is Internal.
            // The authoritative reconstruction key is the type-name trailer, not the status code (the client re-raises
            // by type regardless), but the code is what logs/metrics/retry-policies see, so it must be semantically right.
            var statusCode = exception switch
            {
                KeyNotFoundException => StatusCode.NotFound,
                ArgumentException => StatusCode.InvalidArgument,
                _ when info.IsTransient => StatusCode.Unavailable,
                _ => StatusCode.Internal,
            };

            var trailers = new global::Grpc.Core.Metadata
            {
                { TypeNameKey, info.TypeName ?? string.Empty },
                { TransientKey, info.IsTransient ? "1" : "0" },
            };

            if (!string.IsNullOrEmpty(info.StackTrace))
            {
                trailers.Add(StackKey, Encoding.UTF8.GetBytes(info.StackTrace));
            }

            return new RpcException(new Status(statusCode, info.Message ?? string.Empty), trailers);
        }

        /// <summary>Client side: reconstruct the CLR exception, re-raising the original type where known (§6.2).</summary>
        public static Exception ToException(RpcException exception)
        {
            ArgumentNullException.ThrowIfNull(exception);

            var trailers = exception.Trailers;
            var typeName = trailers.GetValue(TypeNameKey);
            var message = exception.Status.Detail;
            var transient = trailers.GetValue(TransientKey) == "1";

            string stack = null;
            var stackBytes = trailers.GetValueBytes(StackKey);
            if (stackBytes != null)
            {
                stack = Encoding.UTF8.GetString(stackBytes);
            }

            // Re-raise the engine's exception-contract types with their identity intact (§6.2). Others keep the
            // type name as data on GrpcRemoteException (§6.1) — custom subtype identity was already accepted as lost.
            // NB: for the Argument* types the full Message round-trips verbatim (we pass paramName: null so the
            // "(Parameter '…')" suffix already in Message is not re-appended); the ParamName *property* is not
            // preserved — carrying it would require splitting it back out of Message, an accepted minor fidelity loss.
            return typeName switch
            {
                "System.ArgumentNullException" => new ArgumentNullException(paramName: null, message),
                "System.ArgumentException" => new ArgumentException(message),
                "System.Collections.Generic.KeyNotFoundException" => new KeyNotFoundException(message),
                "System.InvalidOperationException" => new InvalidOperationException(message),
                _ => new GrpcRemoteException(typeName, message, stack, transient),
            };
        }
    }
}
