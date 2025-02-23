#!/bin/bash

# Cleanup build
find ./artifacts -type f -name "*.nupkg" -exec rm -f {} \;

# Build and Pack Quack
setupPath="./src/Aquila.Setup/Aquila.Setup.csproj"

dotnet build "$setupPath" --configuration release
dotnet pack "$setupPath" --configuration release

# Find the .nupkg file
nupkg=""

for f in ./release/Aquila.Setup.*.nupkg; do
    nupkg="$f"
    nupkgFolder=$(dirname "$nupkg")
    break
done

if [[ -z "$nupkg" ]]; then
    echo "Could not find the .nupkg file"
    exit 1
fi

# Extract the version number from the .nupkg file
filename=$(basename "$nupkg")

# Remove the prefix 'Aquila.Setup.' and suffix '.nupkg'
version="${filename//Aquila.Setup./}"
version="${version//.nupkg/}"

dotnet tool install --local --add-source "$nupkgFolder" Aquila.Setup --version "$version"

exit $?
