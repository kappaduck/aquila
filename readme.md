# Aquila

Aquila is a NuGet package build on top of [SDL3], designed to provide a clean and modern .NET 9.0 API. Aquila ensures high performance and flexibility for game development and multimedia applications, while abstracting away the low-level details of SDL3 API. It's offers window management, input handling, audio, rendering and more.

## Table of contents

- [Aquila](#aquila)
  - [Table of contents](#table-of-contents)
  - [Compatibility](#compatibility)
    - [Cross-platform](#cross-platform)
  - [Installation](#installation)
  - [Usage](#usage)
  - [Development](#development)
    - [Prerequisites](#prerequisites)
    - [SDL](#sdl)
      - [Arguments](#arguments)
    - [Sandbox](#sandbox)
      - [Arguments](#arguments-1)
  - [Acknowledgments](#acknowledgments)

## Compatibility

Here is a list of supported SDL versions:

| Aquila version | SDL version |
| :------------: | :---------: |
|   `>= 0.1.0`   |   `3.2.0`   |

### Cross-platform

At the moment, Aquila is only supported on Windows. However, it is designed to be cross-platform and will support theses platforms in the future.

- Linux
- Web (wasm)

> It is possible that Aquila will support other platforms such as macOS, Android, iOS, and consoles but it is not planned at the moment.

## Installation

*Work in progress...*

## Usage

*Work in progress...*

## Development

To build Aquila from source, you need to have the following tools installed:

### Prerequisites

- [.NET 9.0 SDK]
> The .NET 9.0 SDK includes everything you need to build and run .NET applications on your machine.

- [Cmake]
> Minimum version required: `3.16.0`
>
> CMake is an open-source, cross-platform family of tools designed to build, test and package software which is used to build the native SDL3 library.
>
> You can install CMake using the following command `winget install Kitware.CMake`.

### SDL

Aquila depends on the SDL3 library. It is not included in the repository. The project offers a script to download and build SDL from source.

```bash
.setup.bat sdl --silent --configuration debug
```
or
```sh
./setup.sh sdl --silent --configuration debug
```
or using aliases
```bash
./setup.bat sdl -sf -c debug
```

#### Arguments

Available arguments for the SDL setup script:

|     Argument      | Alias |           Description           |     Default     |
| :---------------: | :---: | :-----------------------------: | :-------------: |
|    `--branch`     | `-b`  | build a specific SDL branch/tag | `release-3.2.0` |
| `--configuration` | `-c`  |       Build configuration       |    `release`    |
|     `--force`     | `-f`  | Force the reinstallation of SDL |     `false`     |
|    `--silent`     | `-s`  |           Silent mode           |     `false`     |

### Sandbox

The sandbox project is a simple C++ console application that experiments with the SDL3 library. It make sure that Aquila has the same behavior as the original SDL3 library.

> Make sure to run the SDL setup script before building the sandbox.

```console
.setup.bat sandbox --silent --configuration debug
```
or
```console
./setup.sh sandbox --silent --configuration debug
```
or using aliases
```console
./setup.bat sandbox -sf -c debug
```

#### Arguments

Available arguments for the sandbox setup script:

|     Argument      | Alias  |     Description     |  Default  |
| :---------------: | :----: | :-----------------: | :-------: |
| `--configuration` |  `-c`  | Build configuration | `release` |
|     `--force`     |  `-f`  |  Force the rebuild  |  `false`  |
|    `--no-run`     | `none` | Do not run the app  |  `false`  |
|    `--silent`     |  `-s`  |     Silent mode     |  `false`  |

## Acknowledgments

Aquila leverages SDL3 and draws inspiration from the following resources:

- [SDL3]
- [Lazy Foo' Productions]
- [Sayers.SDL2.Core]
- [SDL3-CS]

[Cmake]: https://cmake.org/download/
[Lazy Foo' Productions]: https://lazyfoo.net/index.php
[.NET 9.0 SDK]: https://dotnet.microsoft.com/download/dotnet/9.0
[Sayers.SDL2.Core]: https://github.com/JeremySayers/Sayers.SDL2.Core
[SDL3]: https://www.libsdl.org/
[SDL3-CS]: https://github.com/flibitijibibo/SDL3-CS
