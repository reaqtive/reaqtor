// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Scope = "type", Target = "~T:Reaqtive.Scheduler.WorkItemBase`1", Justification = "avoiding breaking change")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Scope = "type", Target = "~T:Reaqtive.Scheduler.HeapBasedPriorityQueue`1", Justification = "it's a Queue, and not ambiguous given its namespace")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.MSInternal", "CA908:AvoidTypesThatRequireJitCompilationInPrecompiledAssemblies", Scope = "member", Target = "~F:Reaqtive.Scheduler.WorkerThread._canary", Justification = "it's used as a JIT compiled component")]
