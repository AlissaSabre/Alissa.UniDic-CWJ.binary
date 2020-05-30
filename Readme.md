Alissa.UniDic-CWJ.binary
=======================
An unofficial nuget binary package of UniDic-CWJ

## What is it?

Alissa.UniDic-CWJ.binary is a nuget package to provide binary dictionary of 
"UniDic-CWJ, the analysis dictionary for the morphological analizer MeCab" (解析用現代書き言葉 UniDic).

It is Alissa Sabre's unofficial packaging of UniDic analysis dictionary to help setting up the dictionary for use in your app.

Due to its large size (> 500MB after compression), [nuget.org] can't host it.  You need to download the nupkg file manually and setup a local package source to make use of it with your development environment.  (Will write more details later.)

## What is UniDic?

UniDic is a cover-all name for
1. <i>design principles</i> of an electronic dictionary for Japanese language based on a set of uniform lexical units (called SUW, Short Unit Word, 短単位) and hierarchical definition of word indexes both defined by National Institute for Japanese Language and Linguistics (国立国語研究所);
2. <i>UniDic database</i>, a relational database that implements the principles; and
3. <i>the analysis dictionaries for the morphological analizer MeCab</i>, an analysis dictionary whose entries consist of SUWs exported from the UniDic database for use with the morphological analizer MeCab.

You can learn more on UniDic on its official website: [https://unidic.ninjal.ac.jp]

For the moment, there are several types of UniDic analysis dictionaries published, including UniDic-CWJ (現代書き言葉 UniDic) and UniDic-CSJ (現代話し言葉 UniDic).  Alissa.uniDic-CWJ.binary is the binary package for the analysis dictionary UniDic-CWJ.

## What is MeCab?

MeCab is a part-of-speech and morphological analyzer primarily for Japanese language, originally developed by Taku Kudo in joint project between Graduate School of Informatics, Kyoto University (京都大学大学院情報学研究科) and NTT Communication Science Laboratories (日本電信電話株式会社コミュニケーション科学基礎研究所) in mid '00s.  It is now widely used as a basis of achademic research of Japanese language and/or various Japanese language processing systems.

The original version of MeCab was written in C++.

## How to use Alissa.UniDic-CWJ.binary?

Alissa.UniDic-CWJ.binary is a nuget package, so you can add the package to your development project using your development environment's standard process on nuget package.  However, due to its large size (> 500MB after compression), [nuget.org] can't host it.  You need to setup a local package feed to use Alissa.UniDic-CWJ.binary.

A local package feed is just a shared file folder among your development team.  You can read more on nuget local package feed in [https://docs.microsoft.com/nuget/hosting-packages/local-feeds].

Note that this nuget package only contains the binary dictionary files.
You need to have MeCab beforehand.
(I'm using NMeCab, a .NET port of MeCab, with this package.)

If you are asking how to use UniDic with MeCab, please consult the documentation of the MeCab tool you plan to use.

## What is this github repository for?

It contains additional files other than UniDic that I used to produce the nuget package.

## So, how to use the files?

If you want to reproduce the nuget package by yourself, follow the steps below.
1. Visit the official UniDic website [https://unidic.ninjal.ac.jp] and download the version 2.3.0 of "UniDic CWJ analysis dictionary for the morphological analizer MeCab" (解析用現代書き言葉 UniDic) by agreeing to the license, and save the zip archive file (unidic-cwj-2.3.0.zip) in a handy location.
2. Extract the following nine files from the zip archive and copy them into the folder `UniDicCWJ`.
    - AUTHORS
    - BSD
    - COPYING
    - GPL
    - LGPL
    - char.bin
    - matrix.bin
    - sys.dic
    - unk.dic
3. Run nuget CLI by typing `nuget pack`.

Have fun!

Alissa Sabre