#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Runs `dotnet format style` over All.slnx with the exclusions this repo
    requires. Single source of truth for both CI and local use so the "apply"
    and "verify" invocations can never drift apart.

.DESCRIPTION
    Two categories of exclusion, both tracked by
    https://github.com/reaqtive/reaqtor/issues/138:

    1. Nuqleon.Linq.Expressions.Optimizers - IDE0001 runs pathologically slowly
       on this project, causing the format pass to hang/time out. Excluded for
       performance.

    2. ~16 source/test files that are <Compile Link>'d into more than one project
       with different preprocessor symbols (the USE_SLIM Bonsai links). When
       `dotnet format` tries to merge the differing per-project edits for such a
       linked file it mangles the source: re-verified on SDK 10.0.3xx
       (2026-07), the pass now completes without throwing in TryApplyChanges but
       LinkedFileMergeConflictCommentAdditionService writes
       "<<<<<<< TODO: Unmerged change from project ..." conflict markers into
       the files. Excluding these files keeps the pass safe. Keep this list in
       sync with the <Compile Include .. Link> entries in the *.Bonsai* project
       files.

    Primary-constructor conversion (IDE0290) is disabled in .editorconfig, so it
    is not applied by this pass; only whitespace/style fixes are.

.PARAMETER Verify
    Check mode: pass --verify-no-changes (report, do not modify). Used by CI.

.PARAMETER NoRestore
    Pass --no-restore (assumes the caller already restored). Used by CI.

.EXAMPLE
    ./eng/Run-DotnetFormat.ps1            # apply formatting locally
    ./eng/Run-DotnetFormat.ps1 -Verify    # check only (what CI runs)
#>
[CmdletBinding()]
param(
    [switch] $Verify,
    [switch] $NoRestore
)

$ErrorActionPreference = 'Stop'

# Run from the repository root (the parent of this script's eng/ folder).
$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
Push-Location $repoRoot
try {
    $solution = 'All.slnx'

    # Linked files compiled into multiple projects with differing symbols (USE_SLIM).
    # See the .Bonsai* project files; dotnet format crashes merging their edits (#138).
    $linkedFiles = @(
        'Nuqleon/Core/LINQ/Nuqleon.Linq.CompilerServices/Expressions/ExpressionEqualityComparator.cs'
        'Nuqleon/Core/LINQ/Nuqleon.Linq.CompilerServices/Expressions/ExpressionEqualityComparer.cs'
        'Nuqleon/Core/LINQ/Nuqleon.Linq.CompilerServices/Expressions/Rewriters/Misc/ExpressionTupletizer.cs'
        'Nuqleon/Core/LINQ/Nuqleon.Linq.CompilerServices/Expressions/Rewriters/TypeSystem/TypeSubstitutionExpressionVisitor.cs'
        'Nuqleon/Core/LINQ/Nuqleon.Linq.CompilerServices/Expressions/Visitors/ExpressionVisitor.Generic.cs'
        'Nuqleon/Core/LINQ/Nuqleon.Linq.CompilerServices/Expressions/Visitors/ExpressionVisitorBase.Generic.cs'
        'Nuqleon/Core/LINQ/Nuqleon.Linq.CompilerServices/Expressions/Visitors/ExpressionVisitorWithReflection.cs'
        'Nuqleon/Core/LINQ/Nuqleon.Linq.CompilerServices/Expressions/Visitors/ScopedExpressionVisitor.cs'
        'Nuqleon/Core/LINQ/Nuqleon.Linq.CompilerServices/Expressions/Visitors/ScopedExpressionVisitor.Generic.cs'
        'Nuqleon/Core/LINQ/Nuqleon.Linq.CompilerServices/Expressions/Visitors/ScopedExpressionVisitorBase.cs'
        'Nuqleon/Core/LINQ/Nuqleon.Linq.CompilerServices/Expressions/Visitors/ScopedExpressionVisitorBase.Generic.cs'
        'Nuqleon/Core/LINQ/Nuqleon.Linq.CompilerServices/TypeSystem/TypeSubstitutor.cs'
        'Nuqleon/Core/LINQ/Tests.Nuqleon.Linq.CompilerServices/Expressions/UnsafeExpressionTests.cs'
        'Nuqleon/Core/LINQ/Tests.Nuqleon.Linq.CompilerServices/Expressions/ExpressionEqualityComparerTests.cs'
        'Nuqleon/Core/LINQ/Tests.Nuqleon.Linq.CompilerServices/Expressions/Visitors/ScopedExpressionVisitorGenericTests.cs'
        'Nuqleon/Core/LINQ/Tests.Nuqleon.Linq.CompilerServices/Expressions/Rewriters/Misc/ExpressionTupletizerTests.cs'
    )

    # Excluded for performance (IDE0001 is pathologically slow), plus the linked files above.
    $excludes = @('Nuqleon/Core/LINQ/Nuqleon.Linq.Expressions.Optimizers') + $linkedFiles

    $formatArgs = @('format', 'style', $solution, '--exclude') + $excludes + @('--verbosity', 'diagnostic')
    if ($Verify)    { $formatArgs += '--verify-no-changes' }
    if ($NoRestore) { $formatArgs += '--no-restore' }

    Write-Host "dotnet $($formatArgs -join ' ')"
    & dotnet @formatArgs
    exit $LASTEXITCODE
}
finally {
    Pop-Location
}
