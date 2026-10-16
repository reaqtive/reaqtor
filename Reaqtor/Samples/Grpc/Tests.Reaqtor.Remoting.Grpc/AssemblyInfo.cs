// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

// These tests launch out-of-process gRPC hosts on loopback ports and exercise the process-wide MessageRouter static
// (via the in-process oracle), so they must run sequentially. MSTest is sequential by default; this makes the
// requirement explicit so a future [assembly: Parallelize] cannot introduce port or static-state races.
[assembly: DoNotParallelize]
