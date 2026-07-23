// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

// These tests construct in-process InMemoryReactivePlatform instances that share the process-wide MessageRouter
// static, so they must run sequentially (a later MessageRouter.Initialize must not clobber a still-live firehose).
// MSTest is sequential by default; this makes the requirement explicit and future-proof.
[assembly: DoNotParallelize]
