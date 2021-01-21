@echo off

ilasm.exe /DLL /PDB /OPTIMIZE /NOLOGO /QUIET Nuqleon.Runtime.ExceptionServices.Extensions.il
peverify.exe /NOLOGO /QUIET Nuqleon.Runtime.ExceptionServices.Extensions.dll

ilasm.exe /DLL /PDB /OPTIMIZE /NOLOGO /QUIET Tests.Nuqleon.Runtime.ExceptionServices.Extensions.il
peverify.exe /NOLOGO /QUIET Tests.Nuqleon.Runtime.ExceptionServices.Extensions.dll

csc.exe /nologo /r:Nuqleon.Runtime.ExceptionServices.Extensions.dll /r:Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll /t:library Tests.Nuqleon.Runtime.ExceptionServices.Extensions.CSharp.cs
