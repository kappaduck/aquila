#!/bin/bash

setupPath="./src/Setup/Setup.csproj"

dotnet run --project "$setupPath" -- --target "$@"

exit $?
