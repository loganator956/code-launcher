# code-launcher

A simple .NET (C#) console application to launch VS Code portable and set some arguments and environment variables.  

## Requirements

The program, currently, is very specifically programmed to my needs/setup.

It expects the following folder structure:

```Files
.
    PortableGit
        ...
        home
            bin
        ...
        usr
            bin
        bin
    VSCode...
        bin
        ...
        code-launcher.exe
        ...
        extensionslist.txt
        ...
```

| Directory/File | Description |
| --- | --- |
| `./PortableGit/home` | This is what will be treated as your `HOME` directory (Primarily for git/ssh/etc.) |
| `./VSCode...`;`./PortableGit`;`./PortableGit/usr/bin`;`./PortableGit/bin` | These are assigned to the `PATH` environment variable |
| `./VSCode/bin` | This contains the `code` binary file (Should be included from VS Code install files, though) |
| `./VSCode/code-launcher.exe` | This is where you put the executable from this repo |
| `./VSCode.../extensionslist.txt` | This file needs the extensions ID (eg: `ms-dotnettools.csharp`) on each line to be installed when ran |

Furthermore, it may (currently is, AFAIK) be required to NOT install any extensions until you "deploy" the portable (I have a "master" copy where I setup folder structures and basic settings and make copies of it where the extensions and runtime configs are setup).
