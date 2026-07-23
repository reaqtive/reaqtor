// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

// Each test hosts an in-process Kestrel/h2c MagicOnion server on an ephemeral loopback port, so they must run
// sequentially to avoid port contention. MSTest is sequential by default; this makes the requirement explicit.
[assembly: DoNotParallelize]
