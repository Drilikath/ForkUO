cd EmergencyBackup
SET DOTNET=C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319
SET PATH=%DOTNET%
csc.exe /r:..\SevenZipSharp.dll /debug /nowarn:0618 /nologo /out:..\EmergencyBackup.exe /optimize /unsafe /recurse:*.cs
cd ..
cd Server
csc.exe /win32icon:forkuo.ico /r:..\OpenUO.Core.dll /r:..\OpenUO.Ultima.dll /r:..\OpenUO.Ultima.Windows.Forms.dll /r:..\SevenZipSharp.dll /debug /nowarn:0618 /nologo /out:..\ForkUO.exe /optimize /unsafe /recurse:*.cs
PAUSE
cd ..
title ForkUO - By Team ForkUO @ http://forkuo.craftuo.com/
echo off
cls
ForkUO.exe