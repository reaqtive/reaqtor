// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

// Kept sequential for consistency with the other RPC sample test assemblies (and so the wire-trace transcript artifact
// is written deterministically). MSTest is sequential by default; this makes the requirement explicit.
[assembly: DoNotParallelize]
