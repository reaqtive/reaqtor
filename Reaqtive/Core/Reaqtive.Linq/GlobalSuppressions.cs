// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "~N:Reaqtive.Tasks", Justification = "API design.")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "~N:Reaqtive.Operators", Justification = "API design.")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Commonly used by query operators to feed exceptions into OnError.")]
