// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0057:Use range operator", Justification = "Not available while we continue to support .NET Framework and netstandard2.0")]
[assembly: SuppressMessage("Performance", "CA1847:Use char literal for a single character lookup", Justification = "Doesn't work on .NET FX")]
