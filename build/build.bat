@echo off

if not defined DevEnvDir (
    call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Auxiliary\Build\vcvarsall.bat" x64 || goto :error
)

pushd ..\extlibs\
call build.bat || goto :error
popd

msbuild TGUI.net.sln /p:Configuration=Release /p:Platform=x64 /t:tgui /m || goto :error
copy ..\extlibs\SFML.Net\lib\x64\* ..\lib\x64\ /Y > nul || goto :error

goto :EOF
:error
exit /b %errorlevel%