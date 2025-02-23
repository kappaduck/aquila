@echo off
setlocal

:: Cleanup build
for /r ".\artifacts" %%f in (*.nupkg) do (
    del /q /f "%%f"
)

:: Build and Pack Quack
set setupPath=.\src\Aquila.Setup\Aquila.Setup.csproj

dotnet build %setupPath% --configuration release
dotnet pack %setupPath% --configuration release

:: Find the .nupkg file
set quack=release\Aquila.Setup.*.nupkg

set "nupkg="

for /r %CD% %%f in (%quack%) do (
    set nupkg=%%f
    set nupkgFolder=%%~dpf
    goto :install
)

echo "Could not find the .nupkg file"
exit /b 1

:: Extract the version number from the .nupkg file
:install

for %%A in (%nupkg%) do set filename=%%~nxA

for /f "tokens=3 delims=." %%A in ("%filename%") do set version=%%A

set version=%filename:Aquila.Setup.=%
set version=%version:.nupkg=%

dotnet tool install --local --add-source %nupkgFolder% Aquila.Setup --version %version%

exit /b %ERRORLEVEL%
