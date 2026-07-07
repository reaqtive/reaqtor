// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;

using Reaqtor.Remoting.Grpc.Contracts;
using Reaqtor.Remoting.Protocol;
using Reaqtor.Remoting.Protocol.FaultHandling;

namespace Reaqtor.Remoting.Grpc.Protocol
{
    /// <summary>
    /// Converts between the engine's <see cref="INotification{T}"/> (over <see cref="byte"/> payloads) and the wire
    /// <see cref="Contracts.Notification"/>, and between an <see cref="Exception"/> and <see cref="ErrorInfo"/>
    /// (plan §4.3, §6.1). Predicate notifications are never put on the wire (they are test-only, §6).
    /// </summary>
    public static class NotificationConverter
    {
        /// <summary>Converts an engine notification to its wire form.</summary>
        public static Contracts.Notification ToWire(INotification<byte[]> notification)
        {
            ArgumentNullException.ThrowIfNull(notification);

            var wire = new Contracts.Notification { Kind = notification.Kind };

            switch (notification.Kind)
            {
                case NotificationKind.OnNext:
                    wire.Value = notification.Value;
                    break;
                case NotificationKind.OnError:
                    wire.Error = ToErrorInfo(notification.Exception);
                    break;
                case NotificationKind.OnCompleted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(notification), notification.Kind, "Unexpected notification kind.");
            }

            return wire;
        }

        /// <summary>Converts a wire notification back to an engine notification.</summary>
        public static INotification<byte[]> FromWire(Contracts.Notification wire)
        {
            ArgumentNullException.ThrowIfNull(wire);

            return wire.Kind switch
            {
                NotificationKind.OnNext => ObserverNotification.CreateOnNext<byte[]>(wire.Value),
                NotificationKind.OnError => ObserverNotification.CreateOnError<byte[]>(FromErrorInfo(wire.Error)),
                NotificationKind.OnCompleted => ObserverNotification.CreateOnCompleted<byte[]>(),
                _ => throw new ArgumentOutOfRangeException(nameof(wire), wire.Kind, "Unexpected notification kind."),
            };
        }

        /// <summary>Packs an exception into its wire form, preserving the transient flag for BaseExceptions.</summary>
        public static ErrorInfo ToErrorInfo(Exception exception)
        {
            if (exception == null)
            {
                return new ErrorInfo { TypeName = typeof(Exception).FullName, Message = string.Empty };
            }

            return new ErrorInfo
            {
                TypeName = exception.GetType().FullName,
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                IsTransient = exception is BaseException baseException && baseException.IsTransient(),
            };
        }

        /// <summary>Rehydrates an exception from its wire form as a <see cref="GrpcRemoteException"/>.</summary>
        public static Exception FromErrorInfo(ErrorInfo error)
        {
            if (error == null)
            {
                return new GrpcRemoteException("A remote error occurred.");
            }

            return new GrpcRemoteException(error.TypeName, error.Message, error.StackTrace, error.IsTransient);
        }
    }
}
