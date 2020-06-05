Alissa.UniDic-CWJ.binary
=======================
An unofficial nuget binary package of UniDic-CWJ

## What is it?

Alissa.UniDic-CWJ.binary is a nuget package to provide binary dictionary of 
"UniDic-CWJ, the analysis dictionary for the morphological analizer MeCab" (解析用現代書き言葉 UniDic).

It is Alissa Sabre's unofficial packaging of UniDic analysis dictionary to help setting up the dictionary for use in your app.

Due to its large size, the files are split into four nuget packages.
See [below](#how-to-use-it) for how to use it.

## What is UniDic?

UniDic is a cover-all name for
1. _design principles_ of an electronic dictionary for Japanese language based on a set of uniform lexical units (called SUW, Short Unit Word, 短単位) and hierarchical definition of word indexes both defined by National Institute for Japanese Language and Linguistics (国立国語研究所);
2. _UniDic database_, a relational database that implements the principles; and
3. _the analysis dictionaries for the morphological analizer MeCab_, an analysis dictionary whose entries consist of SUWs exported from the UniDic database for use with [MeCab](#What-is-MeCab).

You can learn more on UniDic on its official website: https://unidic.ninjal.ac.jp

For the moment, there are several types of UniDic analysis dictionaries published, including UniDic-CWJ (現代書き言葉 UniDic) and UniDic-CSJ (現代話し言葉 UniDic).  Alissa.UniDic-CWJ.binary is the binary package for the analysis dictionary UniDic-CWJ (現代書き言葉 UniDic).

## What is MeCab?

MeCab is a part-of-speech and morphological analyzer primarily for Japanese language, originally developed by Taku Kudo (工藤拓) in joint project between Graduate School of Informatics, Kyoto University (京都大学大学院情報学研究科) and NTT Communication Science Laboratories (日本電信電話株式会社コミュニケーション科学基礎研究所) in mid '00s.  It is now widely used as a basis of academic researches of Japanese language and/or various Japanese language processing systems.

The original version of MeCab was written in C++.

## How to use it?

Use your nuget/MSBuild-compatible development environment to download and install `Alissa.UniDic-CWJ.binary` nuget package into your app project.  During the build process of your app (using MSBuild or a compatible build tool), a subfolder of name `UniDic-CWJ` is created in your `$(OutDir)` (the folder that you will get your `.exe` or `.dll` in), and the binary files of analysis dictionary are copied into it automatically.

Due to the large size of the dictionary files and the package size limitation that [nuget.org](https://www.nuget.org/) insists, the dictionary files are split into four nuget packages: the main package `Alissa.UniDic-CWJ.binary` and three supplementary packages.  You always need the four packages to use UniDic-CWJ analysis dictionary. The main package depends on the three supplementary packages, so you only need to specify the main package in your development environment; supplementary packages should be downloaded and installed automatically.

Note that this nuget package (or a set of four packages) only contains the binary dictionary files.
You need to have MeCab (or compatible software) separately.
(I'm primarily using [NMeCab](https://github.com/komutan/NMeCab), a .NET port of MeCab analysis library, for my purposes recently.)

## What is this github repository for?

It contains additional files other than UniDic that I used to produce the set of nuget packages.

## How can I build the nuget package(s) using the files in this repository, then?

Please see [another page](Howtobuild.md).

## Revision history

- 2020-6-5: 2.3.0 Beta4: Preserved timestamp of `matrix.bin`.
- 2020-6-4: 2.3.0 Beta3: Improved compatibility with Linux and MacOS.
- 2020-6-3: 2.3.0 Beat2: Various fixes including Clean build target issue.  Also changed the default folder name to `UniDic-CWJ`.
- 2020-6-1: 2.3.0 Beta posted on [nuget.org](https://www.nuget.org/) by splitting the files into four packages.
- 2020-5-30: 2.3.0 Alpha posted on [SourceForge](https://sourceforge.net/projects/alissa-unidic-cwj-binary/files/) (because it was too large for distribution from [nuget.org](https://www.nuget.org/)).

## Copyright information

The UniDic CWJ analysis dictionary for the morphological analizer MeCab (解析用現代書き言葉 UniDic) is copyrighted by The UniDic Consortium.  It is distributed under the choice of three open source licenses, GPL 2, LGPL 2, or BSD.  See the file COPYING for details.

The nuget package files and their support files (included in this repository) are crafted/written by Alissa Sabre and are licensed under the same condition as the UniDic CWJ.  Please give credits to The UniDic Consortium when referring to these nuget packages unless you are talking about the features, issues, etc. specific to my packaging.

Have fun!

Alissa Sabre
