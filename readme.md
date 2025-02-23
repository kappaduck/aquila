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
    - [Quack!](#quack)
      - [Usage](#usage-1)
    - [SDL](#sdl)
      - [Usage](#usage-2)
      - [Example](#example)
    - [Sandbox](#sandbox)
      - [Usage](#usage-3)
      - [Example](#example-1)
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

> It is possible that Aquila will support other platforms but it is not planned at the moment.

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

### Quack!

Quack is a simple dotnet tool that helps you to setup Aquila and its dependencies. You can install it using the following command which will install the tool locally.

```bash
.setup.bat
```
or
```bash
./setup.sh
```

#### Usage

```bash
dotnet quack [OPTIONS] <COMMAND>
```

Available Options:

|   Option    | Alias |        Description         |
| :---------: | :---: | :------------------------: |
|  `--help`   | `-h`  |  Prints help information   |
| `--version` | `-v`  | Prints version information |

Available Commands:

|  Command  |           Description           |
| :-------: | :-----------------------------: |
|   `sdl`   | Install SDL and his dependecies |
| `sandbox` |    Build and run the sandbox    |

### SDL

Aquila depends on the SDL3 library. It is not included in the repository. Quack will execute the following command to download and build SDL and dependencies from source.

#### Usage

```bash
dotnet quack sdl [OPTIONS]
```

|     Argument      | Alias |                 Description                 |  Default  |
| :---------------: | :---: | :-----------------------------------------: | :-------: |
|     `--help`      | `-h`  |           Prints help information           |           |
|    `--version`    | `-v`  |           SDL3 version to install           |  `3.2.0`  |
| `--configuration` | `-c`  |         Build configuration to use          | `release` |
|     `--force`     | `-f`  | Indicating whether to force an installation |  `false`  |
|    `--silent`     | `-s`  |   Indicating whether to silent the output   |  `false`  |
|  `--with-image`   | `-i`  | Install SDL_image with an optional version  |  `3.2.0`  |

#### Example

```bash
dotnet quack sdl -s -c debug
dotnet quack sdl --silent --configuration debug --force
dotnet quack sdl -s -c debug -v 3.2.4 --with-image 3.2.0
```

### Sandbox

The sandbox project is a simple C++ console application that experiments with the SDL3 library. It make sure that Aquila has the same behavior as the original SDL3 library.

> Make sure to install SDL and his dependencies before using the sandbox.

#### Usage

```sh
dotnet quack sandbox [OPTIONS]
```

|     Argument      | Alias |               Description               |  Default  |
| :---------------: | :---: | :-------------------------------------: | :-------: |
|     `--help`      | `-h`  |         Prints help information         |           |
| `--configuration` | `-c`  |       Build configuration to use        | `release` |
|    `--silent`     | `-s`  | Indicating whether to silent the output |  `true`   |

#### Example

```sh
dotnet quack sandbox -c debug
dotnet quack sandbox --silent false --configuration debug
```

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
