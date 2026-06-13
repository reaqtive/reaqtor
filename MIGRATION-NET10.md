# .NET 10 Migration

> **Branch:** `feature/dotnet-10` · **Status:** framework migration complete, build + tests green ·
> **SDK:** .NET 10 (`global.json` pins `10.0.300`)

This document is the reviewer's guide to the .NET 10 upgrade. It explains **what changed**, **what
was archived and why**, the **decisions and trade-offs**, and the **work deliberately deferred** to a
follow-up so the diff is reviewable in pieces. Read the "Compromises & deferred work" section before
concluding anything is missing — several omissions are intentional.

## Goal

Upgrade the solution to **.NET 10**, **drop .NET Framework and .NET Standard**, **archive** projects
and types that depend on now-removed .NET Framework APIs, and **modernise package management and the
test stack (MSTest)**.

## Results at a glance

| Check | Result |
|---|---|
| `dotnet build All.sln` (Debug **and** Release) | ✅ 0 warnings, 0 errors |
| Full test suite on Microsoft.Testing.Platform | ✅ **8,583 passing**, 0 failing |
| `dotnet pack` (Release, `CreatePackage=true`) | ✅ **68** `net10.0` NuGet packages |
| Surviving projects | **119** (118 `.csproj` + 1 `.vbproj`), all single-target `net10.0` (Rxcel `net10.0-windows`) |
| Archived projects | **48** (moved to `archive/`, out of all solutions) |

The work is split into **15 focused commits** (`git log 013e65a..HEAD`), one per phase, so each step is
reviewable on its own and the build stays green across the sequence.

## Before → after

| | Before | After |
|---|---|---|
| Target frameworks | `netstandard2.0;netstandard2.1;net472;net6.0` (+ a few variants) | **`net10.0`** (single); `net10.0-windows` for the one WPF/WinForms pearl |
| Package versions | inline `Version=` in 100+ `.csproj` | **Central Package Management** (`Directory.Packages.props`) |
| Test framework | MSTest 3.1.1 on the classic **VSTest** runner | MSTest **3.11.1** on **Microsoft.Testing.Platform (MTP)** |
| Language | C# 11 | **C# 14** |
| `.NET`-Framework-only code | built (net472 target) | **archived** (Remoting, CAS, `BinaryFormatter`, …) |

## Scope of changes (by phase)

0. **Toolchain & CI** — add `global.json` (.NET 10 SDK); update Azure Pipelines off the retired 7.x/8.x
   SDK + 6.x/3.1 runtime installs; drop the Remoting "Glitching" CI job (its sample is archived).
1. **Archive** the .NET-Framework-bound projects (see next section) into a path-mirrored `archive/`
   tree, removed from every solution. Source + git history preserved; not built or shipped.
2. **Central Package Management** — `Directory.Packages.props`; deleted now-in-box packages
   (`System.Collections.Immutable`, `System.Buffers`); bumped library/build packages.
3–4. **Collapse all surviving projects to `net10.0`**, area by area (Nuqleon → Reaqtive → Reaqtor),
   each gated on its `*.Core.sln` building + testing green. `Nuqleon.Documentation` was **ported**
   (it only used portable APIs and was simply never multi-targeted), not archived.
5. **Samples & Pearls** to `net10.0` (IoT, Shebang; CSE, DelegatingBinder, PartitionedSubject,
   OperatorLocalStorage's `Reaqtive.Storage`, the LINQ pearls). Rxcel keeps its Windows/headless split.
6. **Microsoft.Testing.Platform** — MSTest 3.1.1 → 3.11.1, `EnableMSTestRunner`, MTP coverage/TRX/
   hangdump extensions, `global.json` `test.runner`, CI test-arg translation,
   `coverlet.runsettings` → `codecoverage.runsettings`.
7. **C# 14** (`LangVersion`), removed the dead `net472` `Microsoft.CSharp` reference.
8. Removed the **dead `netstandard2.0` Reflection.Emit polyfill** (13 whole-file shims).

Plus a cross-cutting decision-4 change: **removed obsolete `ISerializable`/`BinaryFormatter`
exception-serialization plumbing** repo-wide (it triggers `SYSLIB0051`/`SYSLIB0011`, which are
errors here).

## Archived projects (`archive/`) — what and why

48 projects were moved to `archive/` because they depend on .NET Framework technology that **does not
exist or does not function on .NET 10**. They are reference-only: removed from all solutions, not
built, not shipped; their project references are not maintained. See `archive/README.md`.

| Archived | Reason |
|---|---|
| `Reaqtor/Samples/Remoting/**` (~31 projects) | Built on **.NET Remoting** (`MarshalByRefObject`, remoting channels) + AppDomain sandboxing — removed in .NET Core/5+. |
| `Nuqleon.Runtime.Remoting.Tasks` (+ tests) | Task-based async over **.NET Remoting**. No modern equivalent. |
| `Nuqleon/Museum/**` | Explicitly legacy/superseded libraries (e.g. pre-Bonsai serialization). |
| net472-only Pearls (OperatorFusion; ProjectJohnnie perf/playground; OperatorLocalStorage playground/tests) | Save-based `Reflection.Emit` / Windows-only diagnostics. The portable parts (e.g. `Reaqtive.Storage`, `Nuqleon.Memory.Diagnostics`) were migrated and survive. |
| `Nuqleon.Pearls.Linq.Expressions.Bonsai.Serialization.Binary` (BinaryExpressionSerialization) | Its object serializer **is** `BinaryFormatter`, which is removed at runtime on .NET 9+ — the prototype cannot function on .NET 10. |

## Key decisions

- **Single `net10.0`, not multi-targeting.** Matches the "drop Framework/Standard" goal and lets all
  framework conditionals be eliminated. (net10.0 is an LTS release.)
- **MSTest 3.11.1, not 4.x.** MTP (the platform) is the requested modernisation, and MSTest 3.11.1 is
  fully MTP-native. MSTest **4.x** changes the `TestMethodAttribute.Execute` / `GetTestMethodAttribute`
  extensibility APIs that `Common/TestUtilities/AssertEx.cs` overrides (and that file is linked into
  every test project), so 4.x would cascade breakages. A v3→v4 bump is a clean, separate follow-up.
- **MTP package versions must be aligned.** All `Microsoft.Testing.*` packages share a platform major
  version; they are pinned to the **MTP 1.9.1** that MSTest 3.11.1 depends on (a mismatch surfaces as a
  runtime `IDataConsumer` `TypeLoadException`).
- **`NET6_0` → `NET6_0_OR_GREATER` rewrite.** The code gated its "modern .NET" branches on the bare
  `NET6_0` symbol (true only when targeting *exactly* net6.0). On net10 that is false, so a naive TFM
  bump would have compiled the **.NET Framework `#else` branches**. The symbol was rewritten on
  `#if`/`#elif` lines (behaviour-preserving for every prior TFM) so net10 selects the modern branches.
- **Removed**, rather than kept-with-suppression, the deprecated public surface that *cannot work* on
  .NET 10 (CAS / `PermissionSet`, `BinaryFormatter`/`ISerializable` exception plumbing). Obsolete-but-
  *functional* reflection mirrors (`Type.IsSerializable`, etc.) are kept with a scoped `NoWarn`.

## Notable fixes surfaced by the migration

- **MTP process hang (root-caused & fixed).** `PhysicalScheduler`'s worker and heartbeat threads were
  *foreground* threads, so a scheduler that wasn't explicitly disposed kept the process alive. VSTest
  force-terminated its host and masked this; under MTP each test project is its own executable that
  must exit cleanly, so leaked schedulers hung the run. Fixed by making those threads
  `IsBackground=true` (a scheduler should never keep a process alive on its own).
- **Bonsai/DataModel serialization tests** updated to recognise the net10 core assembly
  (`System.Private.CoreLib 10.0.0.0`) in their `"STD"` normalization helpers.
- **C# 14 `params` collections in an expression tree** — `string.Format(...)` in a LINQ expression tree
  now binds to the `params ReadOnlySpan<object>` overload (invalid in expression trees); forced the
  classic `object[]` overload (same resulting tree).
- `FormatterServices.GetUninitializedObject` → `RuntimeHelpers.GetUninitializedObject`.
- Two `Tests.*`-named **non-test** projects (a data "Catalog" and a VB fixture library, both
  `IsTestProject=false`) were being built as MTP executables by the name-based heuristic; forced them
  back to libraries.

## Compromises & deferred work

These were **deliberately left for a follow-up** so this PR stays a *framework* migration rather than
also absorbing a large, separate code-style/analyzer churn. All are documented inline in
`Directory.Build.props` and do not affect correctness — the build and tests are green without them.

1. **`EnforceCodeStyleInBuild` is currently `false`.** The .NET 10 SDK's formatter/style analyzers
   (IDE0055 whitespace, IDE0350, …) are far stricter than the SDK this repo was last formatted against,
   producing a large **style-only** wave. `dotnet format` cannot auto-apply the fixes on this solution
   today — its MSBuildWorkspace throws on `TryApplyChanges` across the many linked/generated files
   (related to the pre-existing issue #138). **Follow-up:** a dedicated reformatting pass, then re-enable
   the property. The `dotnet format` **CI gate is `continueOnError`** until then.
2. **`AnalysisLevel` stays at `7.0`** and the temporary `NoWarn` list (issue #143:
   `IDE0079;IDE0090;CA1305;CA1822;CA1854`) is retained. Raising the level / removing the list surfaces a
   separate CA wave.
3. **MSTest analyzer guidance is suppressed** (`MSTEST0006/0036/0037/0039/0053/0055`, e.g. "use
   `Assert.ThrowsExactly` instead of `Assert.ThrowsException`"). These have different semantics and
   cannot be auto-converted safely; adopting them is test-code modernisation. Optional MSTest **v3→v4**
   is a related follow-up.
4. **`CS8981` is suppressed.** The operators use a deliberate, long-standing convention of terse private
   nested type names (`_` for the primary operator/observer, `i` for a secondary inner observer); C# 14
   flags all-lowercase type names as possibly-future-reserved.
5. **Inline dead framework branches remain.** Whole-file-dead polyfills were deleted, but the inactive
   `#else`/`#if NET472` branches inside otherwise-active files are left in place (they compile to
   nothing on net10). Stripping them safely is mechanical cleanup best reviewed on its own.
6. **CI coverage on MTP is translated but unverified end-to-end.** The `dotnet test` arguments were
   converted to MTP equivalents (`--coverage` + `codecoverage.runsettings`, `--hangdump-timeout`, the
   reportgenerator glob), but the Azure Pipelines run itself could not be exercised locally — worth a
   check on the first CI build.

## How it was verified

- Per-area gates during the collapse: `dotnet build`/`dotnet test` on each `*.Core.sln` before moving on.
- Full solution: `dotnet build All.sln -c Debug` **and** `-c Release` → clean.
- Full test suite on MTP: `dotnet test All.sln --filter "TestCategory!~NonDeterministic_Strong"`
  → **8,583 pass**, no hangs (with `--hangdump-timeout` as a safety net while diagnosing the hang above).
- Packaging: `dotnet build All.sln -c Release /p:CreatePackage=true` → 68 packages, each containing
  `lib/net10.0`.

## Notes for reviewers

- The archive move uses `git mv` (history preserved); use `git log --follow` on an archived file to trace it.
- Suppressions added during the migration are intentional and commented at their definition
  (`Directory.Build.props`, the relevant `.csproj`, or inline `#pragma`) — each explains *why*.
- `coverlet.runsettings` was removed (coverlet is gone under MTP); coverage config now lives in
  `codecoverage.runsettings`.
