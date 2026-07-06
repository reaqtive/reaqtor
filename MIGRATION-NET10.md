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

| Check                                          | Result                                                                                       |
|------------------------------------------------|----------------------------------------------------------------------------------------------|
| `dotnet build All.slnx` (Debug **and** Release) | ✅ 0 warnings, 0 errors                                                                       |
| Full test suite on Microsoft.Testing.Platform  | ✅ **8,844 passing**, 0 failing                                                               |
| `dotnet pack` (Release, `CreatePackage=true`)  | ✅ **68** `net10.0` NuGet packages                                                            |
| Surviving projects                             | **121** (120 `.csproj` + 1 `.vbproj`), all single-target `net10.0` (Rxcel `net10.0-windows`) |
| Archived projects                              | **46** (moved to `archive/`, out of all solutions)                                           |

The work is split into **15 focused commits** (`git log 013e65a..HEAD`), one per phase, so each step is
reviewable on its own and the build stays green across the sequence.

## Before → after

|                            | Before                                                           | After                                                                    |
|----------------------------|------------------------------------------------------------------|--------------------------------------------------------------------------|
| Target frameworks          | `netstandard2.0;netstandard2.1;net472;net6.0` (+ a few variants) | **`net10.0`** (single); `net10.0-windows` for the one WPF/WinForms pearl |
| Package versions           | inline `Version=` in 100+ `.csproj`                              | **Central Package Management** (`Directory.Packages.props`)              |
| Test framework             | MSTest 3.1.1 on the classic **VSTest** runner                    | MSTest **4.2.3** on **Microsoft.Testing.Platform (MTP) 2.2.3**           |
| Language                   | C# 11                                                            | **C# 14**                                                                |
| `.NET`-Framework-only code | built (net472 target)                                            | **archived** (Remoting, CAS, `BinaryFormatter`, …)                       |

## Scope of changes (by phase)

0. **Toolchain & CI** — add `global.json` (.NET 10 SDK); update Azure Pipelines off the retired 7.x/8.x
   SDK + 6.x/3.1 runtime installs; drop the Remoting "Glitching" CI job (its sample is archived).
1. **Archive** the .NET-Framework-bound projects (see next section) into a path-mirrored `archive/`
   tree, removed from every solution. Source + git history preserved; not built or shipped.
2. **Central Package Management** — `Directory.Packages.props`; deleted now-in-box packages
   (`System.Collections.Immutable`, `System.Buffers`); bumped library/build packages.
3–4. **Collapse all surviving projects to `net10.0`**, area by area (Nuqleon → Reaqtive → Reaqtor),
   each gated on its `*.Core.slnx` building + testing green. `Nuqleon.Documentation` was **ported**
   (it only used portable APIs and was simply never multi-targeted), not archived.
5. **Samples & Pearls** to `net10.0` (IoT, Shebang; CSE, DelegatingBinder, PartitionedSubject,
   OperatorLocalStorage's `Reaqtive.Storage`, the LINQ pearls). Rxcel keeps its Windows/headless split.
6. **Microsoft.Testing.Platform** — MSTest 3.1.1 → 4.2.3, `EnableMSTestRunner`, MTP coverage/TRX/
   hangdump extensions, `global.json` `test.runner`, CI test-arg translation,
   `coverlet.runsettings` → `codecoverage.runsettings`.
7. **C# 14** (`LangVersion`), removed the dead `net472` `Microsoft.CSharp` reference.
8. Removed the **dead `netstandard2.0` Reflection.Emit polyfill** (13 whole-file shims).
9. **Solution format → `.slnx`.** All 25 live solutions (and the 4 archived ones) were converted from
   the classic GUID-based `.sln` to the modern XML `.slnx` format via `dotnet sln migrate`, and the
   `.sln` files removed. `All.slnx` preserves all 119 projects and 84 solution folders and builds
   clean; `dotnet build`/`dotnet test` auto-detect `.slnx`. **Requires** VS 2022 17.13+ / Rider
   2024.3+ / `dotnet` SDK 9.0.200+ — older Visual Studio can no longer open these solutions.

Plus a cross-cutting decision-4 change: **removed obsolete `ISerializable`/`BinaryFormatter`
exception-serialization plumbing** repo-wide (it triggers `SYSLIB0051`/`SYSLIB0011`, which are
errors here).

## Archived projects (`archive/`) — what and why

46 projects live in `archive/` because they depend on .NET Framework technology that **does not
exist or does not function on .NET 10**. They are reference-only: removed from all solutions, not
built, not shipped; their project references are not maintained. See `archive/README.md`.

| Archived                                                                                                   | Reason                                                                                                                                                         |
|------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `Reaqtor/Samples/Remoting/**` (~31 projects)                                                               | Built on **.NET Remoting** (`MarshalByRefObject`, remoting channels) + AppDomain sandboxing — removed in .NET Core/5+.                                         |
| `Nuqleon.Runtime.Remoting.Tasks` (+ tests)                                                                 | Task-based async over **.NET Remoting**. No modern equivalent.                                                                                                 |
| `Nuqleon/Museum/**`                                                                                        | Explicitly legacy/superseded libraries (e.g. pre-Bonsai serialization).                                                                                        |
| net472-only Pearls (OperatorFusion; ProjectJohnnie perf/playground) | Save-based `Reflection.Emit` / Windows-only diagnostics. The portable parts (e.g. `Reaqtive.Storage`, `Nuqleon.Memory.Diagnostics`) were migrated and survive. The OperatorLocalStorage playground and tests, initially archived for being net472-pinned, had no Framework-only dependencies and were migrated back to `net10.0`. |
| `Nuqleon.Pearls.Linq.Expressions.Bonsai.Serialization.Binary` (BinaryExpressionSerialization)              | Its object serializer **is** `BinaryFormatter`, which is removed at runtime on .NET 9+ — the prototype cannot function on .NET 10.                             |

## Key decisions

- **Single `net10.0`, not multi-targeting.** Matches the "drop Framework/Standard" goal and lets all
  framework conditionals be eliminated. (net10.0 is an LTS release.)
- **MSTest 4.2.3 (current major).** The v3→v4 breaking changes were absorbed as part of the migration:
  `Assert.ThrowsException(Async)` → `Assert.ThrowsExactly(Async)` (~2,700 call sites),
  `[ExpectedException]` → `Assert.ThrowsExactly` (44 sites), the removed
  `(message, params object[])` assert overloads folded into interpolated messages, and
  `TestMethodAttribute.Execute` → `ExecuteAsync` in `Common/TestUtilities/AssertEx.cs`.
  The repo's own callback-based `AssertEx.ThrowsException(Async)` helpers (~640 call sites) were
  subsequently retired in favor of `Assert.ThrowsExactly(Async)`, whose returned exception makes the
  callback pattern redundant; the legacy helpers moved to `archive/Common/TestUtilities/AssertEx.Legacy.cs`
  because archived test projects still call them.
- **MTP package versions must be aligned.** All `Microsoft.Testing.*` packages share a platform major
  version; they are pinned to the **MTP 2.2.3** that MSTest 4.2.3 depends on (a mismatch surfaces as a
  runtime `IDataConsumer` `TypeLoadException`); CodeCoverage 18.x tracks MTP 2.x.
- **`NET6_0` → `NET6_0_OR_GREATER` rewrite.** The code gated its "modern .NET" branches on the bare
  `NET6_0` symbol (true only when targeting *exactly* net6.0). On net10 that is false, so a naive TFM
  bump would have compiled the **.NET Framework `#else` branches**. The symbol was rewritten on
  `#if`/`#elif` lines (behaviour-preserving for every prior TFM) so net10 selects the modern branches.
- **Removed**, rather than kept-with-suppression, the deprecated public surface that *cannot work* on
  .NET 10 (CAS / `PermissionSet`, `BinaryFormatter`/`ISerializable` exception plumbing). Obsolete-but-
  *functional* reflection mirrors (`Type.IsSerializable`, etc.) are kept with a scoped `NoWarn`.

## Notable fixes surfaced by the migration

- **MTP process lifetime.** `PhysicalScheduler`'s worker and heartbeat threads are *foreground*
  threads — deliberately so: a live scheduler means the process still has work in flight (e.g.
  persisting state on shutdown), and it must keep the process alive. Under MTP each test project is
  its own executable that must exit cleanly (VSTest used to force-terminate its host, masking leaks),
  so a test that leaks an undisposed scheduler hangs the run. The test suites dispose their
  schedulers (`using` / `TestCleanup` / `ClassCleanup`); any future MTP hang points at a leaked
  scheduler that needs disposing — the threads stay foreground by design.
- **Bonsai/DataModel serialization tests** updated to recognise the net10 core assembly
  (`System.Private.CoreLib 10.0.0.0`) in their `"STD"` normalization helpers.
- **C# 14 `params` collections in an expression tree** — `string.Format(...)` in a LINQ expression tree
  now binds to the `params ReadOnlySpan<object>` overload (invalid in expression trees); forced the
  classic `object[]` overload (same resulting tree).
- `FormatterServices.GetUninitializedObject` → `RuntimeHelpers.GetUninitializedObject`.
- Two `Tests.*`-named **non-test** projects (a data "Catalog" and a VB fixture library, both
  `IsTestProject=false`) were being built as MTP executables by the name-based heuristic; forced them
  back to libraries.
- **`CA2000`/`CA2213` disposal analyzers (stricter on the .NET 10 SDK).** The pre-existing
  `AnalysisMode=All` + `TreatWarningsAsErrors` (in `Directory.Build.props` since 2021) meets the newer
  CA analyzers in the .NET 10 SDK, which flag a large, **codebase-wide** wave of IDisposable
  diagnostics the .NET 7/8 SDK did not (≈64 `CA2000`/`CA2213` sites). CI only ever reported the first
  handful because each build job fails fast on the first project. These are overwhelmingly false
  positives over the reactive/operator patterns here (back-pointers to an owning operator/subject,
  fields disposed via `DisposeCore`/`OnDispose` hooks the analyzer doesn't model, ownership transferred
  to a returned wrapper), so they are **deferred wholesale via `NoWarn` in `Directory.Build.props`**
  (joining the existing `CA1305;CA1822;CA1854` deferral, issue #143) rather than churning core code —
  consistent with this PR staying a framework migration. The one genuine leak the wave surfaced —
  `SynchronizedFactory.Impl<T,R>` in `MemoizationCacheFactoryExtensions.cs` never disposing its owned
  `_cache`/`_gate` — was **fixed in code** (a `DisposeCore()` override matching its sibling
  `ImplBase<T,R>`), not merely suppressed.

## Compromises & deferred work

These were **deliberately left for a follow-up** so this PR stays a *framework* migration rather than
also absorbing a large, separate code-style/analyzer churn. All are documented inline in
`Directory.Build.props` and do not affect correctness — the build and tests are green without them.
(Items marked *resolved* were subsequently folded back into this PR.)

1. **`EnforceCodeStyleInBuild` is `true` and the `dotnet format` CI gate is enforcing** (resolved,
   issue #160). The ~3,500-warning style wave the .NET 10 SDK analyzers produced was cleared in
   full: a solution-scoped `dotnet format style` pass for the bulk, plus project-scoped passes for
   the files the solution-scoped run cannot safely touch (see below). Deliberate exceptions carry a
   per-site `#pragma` citing their rationale (review-kept code shapes, and spots where a style fix
   would trip an error-severity CA rule such as CA1825/CA1024). Notes that remain true:
   - **Primary constructors are opted out.** `.editorconfig` sets
     `csharp_style_prefer_primary_constructors = false` and `dotnet_diagnostic.IDE0290.severity = none`.
     (An earlier pass had converted ~1,360 types to primary constructors; that conversion was reverted
     and the rule disabled so it is not (re-)applied.)
   - **The #138 exclusions remain, but only for the solution-scoped format path**
     (`eng/Run-DotnetFormat.ps1`, used by CI's verify gate). The hazard is specific to one file
     being `<Compile Link>`'d into multiple projects *of the same format workspace with different
     symbols* (the `USE_SLIM` Bonsai links): the per-project edits then go through
     `LinkedFileMergeConflictCommentAdditionService`, which writes conflict markers (issue #138).
     The excluded files themselves are now formatted — each was formatted via *project-scoped*
     `dotnet format` runs (one per `USE_SLIM` view), where the file has a single compilation and
     there is nothing to merge — and build-time enforcement keeps them clean. `Nuqleon.Linq.Expressions.Optimizers`
     stays excluded for a different reason (IDE0001 is pathologically slow there); project-scoped
     runs with an explicit `--diagnostics` list avoid loading IDE0001 entirely.
     (Empirically re-verified: `Common/TestUtilities/AssertEx.cs` — linked into every test project
     but with *identical* symbols — merges cleanly under solution-scoped format and needs no
     exclusion; an earlier revision of this document was wrong to list it.)
2. **`AnalysisLevel` is `latest` and the migration-era `NoWarn` lists are retired** (resolved,
   issue #143). The `IDE0079;IDE0090;CA1305;CA1822;CA1854` list was fixed at its sites (47 IDE0090
   via `dotnet format`, the rest by hand; public-API CA1822 hits carry a rationale `#pragma`). The
   `CA2000;CA2213` disposal wave described above **no longer fires at all** at `latest` — the .NET 10
   analyzers resolved the false-positive patterns, so the suppression came out with no code churn.
   Raising the level also enabled the throw-helper rules: ~2,900 canonical null guards became
   `ArgumentNullException.ThrowIfNull` (plus 103 sites across 23 T4 templates, regeneration-verified),
   with `ThrowIfNegative`/`ThrowIfLessThan`/etc. and `ObjectDisposedException.ThrowIf` applied to
   the AOORE/ODE guard shapes; sites whose `ObjectName` carries diagnostic identity (entity URIs,
   reader/writer facets) keep their explicit throws under a rationale `#pragma`. `CA1859` (use
   concrete types) is the one new rule deferred, as an IDE suggestion with rationale in
   `.editorconfig`.
3. **MSTest analyzer guidance is adopted** (resolved; formerly `MSTEST0017/0032/0036/0055` in
   `NoWarn`). MSTEST0017: 104 swapped expected/actual argument pairs fixed. MSTEST0032: the
   always-true `Assert.IsTrue(true)` markers ("did not throw" / "reached here") removed or replaced
   with real assertions. MSTEST0036: the QueryEngine test classes' lifecycle members renamed
   (`ClassTeardown`/`TestSetup`/`TestTeardown`) — MSTest invokes by attribute, so the base-member
   hiding (`new`) was never needed. MSTEST0055: one rationale suppression over expression-tree test
   data whose where-clause predicates the analyzer misreads as discarded calls. The MSTest **v3→v4**
   upgrade itself was done during the migration (see key decisions above).
4. **`CS8981` is suppressed.** The operators use a deliberate, long-standing convention of terse private
   nested type names (`_` for the primary operator/observer, `i` for a secondary inner observer); C# 14
   flags all-lowercase type names as possibly-future-reserved.
5. **Inline dead framework branches: swept (#159).** With every live project single-targeting
   net10.0, all `#if` conditions over TFM symbols became statically decidable; a three-valued
   evaluator removed the dead branches and unwrapped the live ones (~217 constructs across 88
   files, pure deletions). Constructs involving real feature flags (`USE_SLIM`, `DEBUG`, …) are
   untouched. (`USE_SPAN` was later found to be a TFM flag in disguise — its `net6.0`-conditioned
   define never fired after the retarget, silently disabling the span paths in
   `Nuqleon.Json.Serialization` — so the span code was made unconditional and the flag removed.)
6. **CI coverage on MTP is translated but unverified end-to-end.** The `dotnet test` arguments were
   converted to MTP equivalents (`--coverage` + `codecoverage.runsettings`, `--hangdump-timeout`, the
   reportgenerator glob), but the Azure Pipelines run itself could not be exercised locally — worth a
   check on the first CI build. (Also unverified for the same reason: the later move from the retired
   `PublishBuildArtifacts@1`/`PublishCodeCoverageResults@1` tasks to per-job pipeline artifacts
   (`RawCoverage_*`, `Logs_*`) with a pattern download in the coverage stage, and the
   `coverlet/reports` → `coverage/reports` path rename.)

## How it was verified

- Per-area gates during the collapse: `dotnet build`/`dotnet test` on each `*.Core.slnx` before moving on.
- Full solution: `dotnet build All.slnx -c Debug` **and** `-c Release` → clean.
- Full test suite on MTP: `dotnet test All.slnx --filter "TestCategory!~NonDeterministic_Strong"`
  → **8,844 pass**, no hangs (with `--hangdump-timeout` as a safety net while diagnosing the hang above).
- Packaging: `dotnet build All.slnx -c Release /p:CreatePackage=true` → 68 packages, each containing
  `lib/net10.0`.

## Notes for reviewers

- The archive move uses `git mv` (history preserved); use `git log --follow` on an archived file to trace it.
- Suppressions added during the migration are intentional and commented at their definition
  (`Directory.Build.props`, the relevant `.csproj`, or inline `#pragma`) — each explains *why*.
- `coverlet.runsettings` was removed (coverlet is gone under MTP); coverage config now lives in
  `codecoverage.runsettings`.
