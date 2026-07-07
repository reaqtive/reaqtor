// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using global::Grpc.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reaqtor.Remoting.Grpc.Protocol;
using Reaqtor.Remoting.Protocol.FaultHandling;

namespace Reaqtor.Remoting.Grpc.Tests;

//
// Milestone 7 (fault hardening): pins the §6.1/§6.2 status-code + exception-reconstruction mappings used by every
// unary store RPC (GrpcFault). Round-trips an exception server-side (ToRpcException) and back client-side
// (ToException), asserting the status code and the reconstructed CLR type/transient flag. (The store adapters
// exercise these end-to-end over the wire in M3/M4; this is the focused mapping pin.)
//
[TestClass]
public class GrpcFaultMappingTests
{
    [TestMethod]
    public void ArgumentException_Maps_To_InvalidArgument_And_Is_ReRaised_By_Type()
    {
        var rpc = GrpcFault.ToRpcException(new ArgumentException("bad arg"));
        Assert.AreEqual(StatusCode.InvalidArgument, rpc.StatusCode, "an argument fault is InvalidArgument, not NotFound (reserved for missing-key lookups)");

        var reconstructed = GrpcFault.ToException(rpc);
        Assert.IsInstanceOfType(reconstructed, typeof(ArgumentException), "ArgumentException must be re-raised by type (§6.2)");
        Assert.AreEqual("bad arg", reconstructed.Message);
    }

    [TestMethod]
    public void KeyNotFoundException_Maps_To_NotFound_And_Is_ReRaised_By_Type()
    {
        var rpc = GrpcFault.ToRpcException(new KeyNotFoundException("absent"));
        Assert.AreEqual(StatusCode.NotFound, rpc.StatusCode);

        var reconstructed = GrpcFault.ToException(rpc);
        Assert.IsInstanceOfType(reconstructed, typeof(KeyNotFoundException), "KeyNotFoundException must be re-raised by type (§6.2)");
    }

    [TestMethod]
    public void Transient_Fault_Maps_To_Unavailable_And_Preserves_Transient_Flag()
    {
        var rpc = GrpcFault.ToRpcException(new BaseException("retry me", transient: true));
        Assert.AreEqual(StatusCode.Unavailable, rpc.StatusCode, "a transient fault is retriable -> Unavailable (§6.2)");

        var reconstructed = GrpcFault.ToException(rpc);
        var remote = reconstructed as GrpcRemoteException;
        Assert.IsNotNull(remote, "a non-contract type rehydrates as GrpcRemoteException (§6.1)");
        Assert.IsTrue(remote.IsTransient, "the transient flag must survive the round-trip");
    }

    [TestMethod]
    public void Permanent_Fault_Maps_To_Internal_And_Is_Not_Transient()
    {
        var rpc = GrpcFault.ToRpcException(new InvalidTimeZoneException("nope"));
        Assert.AreEqual(StatusCode.Internal, rpc.StatusCode, "a non-transient, non-argument fault -> Internal (§6.2)");

        var reconstructed = GrpcFault.ToException(rpc);
        var remote = reconstructed as GrpcRemoteException;
        Assert.IsNotNull(remote);
        Assert.IsFalse(remote.IsTransient);
        Assert.AreEqual(typeof(InvalidTimeZoneException).FullName, remote.RemoteTypeName, "the original type name is preserved as data (§6.1)");
    }
}
