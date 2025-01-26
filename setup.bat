@echo off

set setupPath=.\src\Setup\Setup.csproj

dotnet run --project %setupPath% -- --target %*

exit /b %ERRORLEVEL%
