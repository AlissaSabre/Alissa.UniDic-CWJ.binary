How to build Alissa.UniDic-CWJ.binary package(s)
================================================

This page describes how to build the `Alissa.UniDic-CWJ.binary` nuget package and its suplementary pacakges using the files included in this repository.
If you are looking for information how to use the packages in your app, please see [the main Readme](Readme.md).

## Prerequisites

You need the following two development toosets:
1. .NET Core SDK including `dotnet` CLI tool.
2. .NET Framework Developer Pack that is capable of building a dll that targets .NET Framework 4.0.

I'm not sure what version you need.  I have several different versions of both, and I'm too lazy to clarify the minimum requirements...  If you have Visual Studio 2019 with workloads _.NET desktop development_ and _.NET Core cross-platform development_, that should be fine.   (I'm _not_ using Visual Studio 2019, though.)

I'm working on Windows 10.  You may or may not be able to build this package on another platform.  (I love to know the result if you tried, regardless it was successful or not.)

## Building steps

Follow the following steps to build the nuget packages:
1. Clone this repository.
2. Visit the official UniDic website [https://unidic.ninjal.ac.jp] and download the version 2.3.0 of "UniDic CWJ analysis dictionary for the morphological analizer MeCab" (解析用現代書き言葉 UniDic) by agreeing to the license terms, and save the zip archive file (unidic-cwj-2.3.0.zip) in a folder of name `UniDic-CWJ` in the working tree.  Don't unzip the file by yourself at the moment.
3. Type `dotnet build` and wait for a while.  You will get the four `*.nupkg` files in the folder of name `nupkgs` that the build system creates.

## Some details of the build process

The build proceeds as follows:
1. Compiles a C# source file `CustomBuildTasks.cs` into `CustomBuildTasks.dll`.  This assembly includes two related MSBuild custom task classes.  `SplitFile` is soon used during the build of the nuget packages.  `ConcatnateFiles` will be used when a project that included `Alissa.UniDic-CWJ.binary` package is built.
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

Note that the project file, `UniDic-CWJ.MeCab-binary.csproj` is crafted by hands from the scratch, and it is in an unusual organization.  It supports two targets, `Build` and `Clean`.  I'm not sure whether other standard MSBuild targets work with it.

Have fun!
Alissa Sabre
