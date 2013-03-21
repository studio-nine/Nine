@echo off


setlocal
set msbuild=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
set flags=/nologo /p:Configuration=Release /p:Optimize=true /p:DebugSymbols=false

pushd ..

call %msbuild% %flags% Source\Nine.Serialization.sln
call %msbuild% %flags% Source\Nine.Windows.sln
call %msbuild% %flags% Source\Nine.Studio.2012.sln

popd

endlocal

pause