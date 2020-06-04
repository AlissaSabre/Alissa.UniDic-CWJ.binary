How to build Alissa.UniDic-CWJ.binary package(s)
================================================

This page describes how to build the `Alissa.UniDic-CWJ.binary` nuget package and its suplementary pacakges using the files included in this repository.
If you are looking for information how to use the packages in your app, please see [the main Readme](Readme.md).

## Prerequisites

You need the following two development toolsets (or equivalent):
1. .NET Core SDK including `dotnet` CLI tool.
2. .NET Framework Developer Pack.

I'm not sure exactly which versions you will need.  My PC has .NET Core SDK 3.1 and .NET Framework Developer Pack 4.8, and I'm too lazy to clarify the minimum requirements.  If you have Visual Studio 2019 with workloads _.NET desktop development_ and _.NET Core cross-platform development_, that should be sufficient.  (I'm _not_ using Visual Studio 2019, though.)

I'm working on Windows 10.  You may or may not be able to build this package on another platform.  The problem is that the build process requires to produce an assembly targetting .NET Framework 4.0.  See [below](#About-CustomBuildTasks) to know more on this point.  (I love to know the result if you tried, regardless it was successful or not.)

## Building steps

First of all, you need enough free disk space (> 10GB).  Due to the large size of UniDic-CWJ analysis dictionary for morphological analizer MeCab as well as inefficient disk usage of my own build process, the total size of the files in the working tree exceeds 10GB after the build is finished.  Explorer reported me that it occupies 10.3GB on disk.

After ensuring the free disk space, follow the steps below to build the nuget packages:
1. Clone this repository.
2. Visit the official UniDic website [https://unidic.ninjal.ac.jp] and download the version 2.3.0 of "UniDic CWJ analysis dictionary for the morphological analizer MeCab" (解析用現代書き言葉 UniDic) by agreeing to the license terms, and save the zip archive file (unidic-cwj-2.3.0.zip) in a folder of name `UniDic-CWJ` in the working tree.  Don't unzip the file by yourself at the moment.
3. Type `dotnet build` and wait for a while.  You will get the four `*.nupkg` files in the folder of name `nupkgs` that the build system creates.

## Some details of the build process

The build proceeds as follows:
1. Compiles a C# source file `CustomBuildTasks.cs` into `CustomBuildTasks.dll`.  This assembly includes two related MSBuild custom task classes.  `SplitFile` is soon used during the build of the nuget packages.  `ConcatnateFiles` will be used when a project that included `Alissa.UniDic-CWJ.binary` package is built.  (See [below](#About-CustomBuildTasks) for more details on this assembly.)
2. The zip file `unidic-cwj-2.3.0.zip` is fully unzipped in the `UniDic-CWJ` directory, though we only need the following nine files:
    - `AUTHORS`
    - `BSD`
    - `COPYING`
    - `GPL`
    - `LGPL`
    - `char.bin`
    - `matrix.bin`
    - `sys.dic`
    - `unk.dic`
3. The largest file among them, `matrix.bin` is split into three smaller files: `matrix.001`, `matrix.002`, and `matrix.003`.
4. The four nuget packages are created, internally issueing `dotnet pack` command four times.

Note that the project file, `UniDic-CWJ.MeCab-binary.csproj` is crafted by hands from the scratch, and it is in an unusual organization.  It supports two targets, `Build` and `Clean` (so that `dotnet clean` works).  I'm not sure whether other standard MSBuild targets work with it.

## About CustomBuildTasks

`CustomBuildTasks.dll` is the key trick that made this set of packages possible (though it is not very tricky).

The binary analysis dictionary of UniDic-CWJ consists of four files, and the largest file among them is `martix.bin`, which is more than 700MB in size.  It has about 400MB even after compressing.  On the other hand, [nuget.org](https://www.nuget.org) insists a limit of 250MB per a .nupkg file.  So, I need to split the file into multiple parts when packaging, and to concatenate the parts to form a single file before it is used in the client project (i.e., a project that installed Alissa.UniDic-CWJ.binary package).

The splitting and concatenation are performed in the build processes by MSBuild; the splitting is during the build of this nuget package, and the concatenation is during the build of the client project.  To do so, I wrote `CustomBuildTasks.cs` containing two `Microsoft.Build.Framework.ITask` implementations.  `Alissa.UniDic.CustomBuildTasks.SplitFile` task performs the split, and `Alissa.UniDic.CustomBuildTasks.ConcatnateFiles` task performs the concatenation.

The problem is that `Microsoft.Build.Framework` assembly, that is essential to create a custom MSBuild task, appears only compatible with .NET Framwork (as opposed to .NET Core).  That's why the build process requires .NET Framework Developer Pack.

However, thinking twice about it, I felt it is strange.  .NET Core SDK comes with MSBuild, and it runs on .NET Core.  Actually I can successfully run `dotnet build` command to build a client (.NET Core) project that installed `Alissa.UniDic-CWJ.binary` on Linux or MacOS, so the `CustomBuildTasks.dll` targetting .NET Framwork 4 is executed on .NET Core SDK, referring to `Microsoft.Build.Framework` assembly.

The observation makes me think that there should be a way to produce `CustomBuildTasks.dll` without using .NET Framwork Developer Pack.  Any idea?  I love to know it!

Alissa Sabre
