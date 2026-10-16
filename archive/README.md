# Archived projects

These projects were retired during the .NET 10 modernization (branch `feature/dotnet-10`).
They depend on **.NET Framework-only technologies that do not exist on modern .NET** and
cannot build on the .NET 10 SDK. They are kept here as **reference source only**: they are
removed from all active solutions, are not built, are not tested, and are not shipped. Their
project/package references to the live libraries are **not maintained** and will be stale.

The directory layout mirrors the original repository paths (under `archive/`) so the
intra-archive references and the retained `.sln` files remain internally consistent.

## What was archived and why

| Path | Reason |
|------|--------|
| `Reaqtor/Samples/Remoting/` | The entire distributed sample is built on **.NET Remoting** (`MarshalByRefObject`, remoting channels) and AppDomain sandboxing — removed from .NET Core / .NET 5+. |
| `Nuqleon/Core/BCL/Nuqleon.Runtime.Remoting.Tasks/` (+ tests) | Task-based async over **.NET Remoting** (`System.Runtime.Remoting`, `MarshalByRefObject`). No modern equivalent. |
| `Nuqleon/Museum/` | Explicitly legacy/superseded libraries (e.g. `Nuqleon.Linq.Expressions.Serialization`, replaced by Bonsai serialization). Already a "museum"; not part of the modern surface. |
| `Reaqtor/Pearls/OperatorFusion/` | Experimental pearl, net472-only (save-based `System.Reflection.Emit`). |
| `Nuqleon/Pearls/BCL/ProjectJohnnie/Perf.Nuqleon.Memory.Diagnostics/` + `…/ProjectJohnnie/` | net472-only perf/playground harnesses (Windows ETW/diagnostics). The `Nuqleon.Memory.Diagnostics` library and its tests were migrated and survive. |
| `Nuqleon/Pearls/LINQ/BinaryExpressionSerialization/` | Prototype binary expression serializer whose default object serializer is built on **`BinaryFormatter`**, which is removed at runtime on .NET 9+. Leaf pearl; its raison d'être cannot function on modern .NET. |
| `Common/TestUtilities/AssertEx.Legacy.cs` | The callback-based `AssertEx.ThrowsException{Async}` helpers, superseded in the live tree by MSTest 4's `Assert.ThrowsExactly{Async}` (which returns the caught exception). Kept because archived test projects still call them. |

## Code factored back out of the archive

Some transport-agnostic building blocks that lived in the archived Remoting sample have since
been factored back into the shipped libraries (see #158):

- The invocation tupletization/detupletization machinery from
  `Reaqtor.Remoting.Client.Library/ExpressionServices/{Tupletizing,Detupletizing}ExpressionServices.cs`
  now lives in `Reaqtor.ExpressionTupletization` (in `Reaqtor.Shared.Core`).
- The allow-list based `ExpressionServices` family now lives in `Reaqtor.Client.Core` as
  `CheckedReactiveExpressionServices`, `TupletizingReactiveExpressionServices`, and
  `DetupletizingReactiveExpressionServices` (with the JSON allow list moved from Newtonsoft.Json
  to `System.Text.Json.Nodes`; consumers can bless additional members by overriding
  `CheckedReactiveExpressionServices.IsAllowedMember`).

The archived copies remain for reference but should not be used as a source for new code.

## Resurrecting a project

To revive any of these on modern .NET, the relevant Framework dependency must be replaced first
(e.g. .NET Remoting → gRPC/ASP.NET Core), then the project re-targeted to `net10.0` and re-added
to a solution.
