@echo off

set _use_code_coverage=0
for %%a in (%*) do if "%%a"=="-coverage" set _use_code_coverage=1

set _release_build=0
for %%a in (%*) do if "%%a"=="-release" set _release_build=1

set _build_linq_sres_tmp=%temp%\SD\IPE\LINQ\SRES
rd /s /q "%_build_linq_sres_tmp%"
mkdir "%_build_linq_sres_tmp%"

set _runtest_list=Tests.Nuqleon.Runtime.ExceptionServices.Extensions.dll Tests.Nuqleon.Runtime.ExceptionServices.Extensions.CSharp.dll

set _test_coverage_targets=Nuqleon.Runtime.ExceptionServices.Extensions.dll

call build.bat
copy *.dll "%_build_linq_sres_tmp%\"
copy *.pdb "%_build_linq_sres_tmp%\"

set _src_linq=%cd%

pushd "%_build_linq_sres_tmp%"

set _original_path=%path%
path %path%;c:\Program Files (x86)\Microsoft Visual Studio 12.0\Team Tools\Performance Tools;

if %_use_code_coverage%==1 (
  for %%a in (%_test_coverage_targets%) do call vsinstr -coverage %%a
  start vsperfmon -coverage -output:%cd%\runtests.coverage
)

call vstest.console.exe %_runtest_list%

if %_use_code_coverage%==1 (
  vsperfcmd -shutdown
  copy %cd%\runtests.coverage "%_src_linq%\"
)

path %_original_path%

popd
