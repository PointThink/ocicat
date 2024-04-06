# OpenAL on Windows
Unfortunately OpenTK (The library ocicat uses) does not provide any native binaries for OpenAL.
Most Linux distros and MacOS provide OpenAL binaries out of the box, but for Windows you will need to get them manually.

> [!IMPORTANT]  
> The official OpenAL redistributable WILL NOT WORK! (probably)
> Your best option is to use OpenAL-Soft.

## Getting OpenAL-Soft
OpenAL-Soft is available for download [here](https://openal-soft.org/).
Download the Win64 and Win32 binaries.

## Adding OpenAL-Soft to your project
Copy the files:
- router/(Platform)/OpenAL32.dll
- bin/(Platform)/soft-oal.dll

into (Your Project Path)/bin/(Your build mode)/net8.0/runtimes/(Platform)/

Platforms:
- win-x64/Win64 - 64-bit Windows
- win-x86/Win32 - 32-bit Windows
