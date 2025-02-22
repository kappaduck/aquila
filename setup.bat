@echo off

set setupPath=.\src\Setup\Setup.csproj

dotnet run --project %setupPath% -- --target %*

if %ERRORLEVEL% NEQ 0 (
    exit /b 1
)

exit /b 0
