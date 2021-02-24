// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "~N:System.Text", Justification = "Logical extension of BCL.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "~N:System.IO", Justification = "Logical extension of BCL.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "~P:System.Collections.Generic.PooledListHolder`1.List", Justification = "By design for holder types.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Scope = "member", Target = "~M:System.Memory.FunctionMemoizationExtensions.#cctor", Justification = "Many types referenced as elements in a static array of types.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "~M:System.Memory.FunctionMemoizationExtensions.WithoutException`1.GetOrAddCore(System.Func{`0})~`0", Justification = "Fine to cause NullReferenceException when invoking a memoized function.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Scope = "member", Target = "~M:System.Memory.FunctionMemoizationExtensions.WithException`1.GetOrAddCore(System.Func{`0})~`0", Justification = "Fine to cause NullReferenceException when invoking a memoized function.")]
