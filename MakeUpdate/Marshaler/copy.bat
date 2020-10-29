@echo off

echo ArgumentMarshaler Downloader
echo ----------------------------
echo Current Directory:
echo %cd%
echo ----------------------------

:booleanmarshaler
echo BooleanMarshalerLib
set /P c=Download [Y/N]? 
if /I "%c%" EQU "Y" goto :booleanmarshalerdownload
if /I "%c%" EQU "N" goto :integermarshaler
goto :booleanmarshaler

:booleanmarshalerdownload
powershell -Command "(New-Object Net.WebClient).DownloadFile('https://github.com/sunriax/argument/releases/latest/download/RaGae.ArgumentLib.BooleanMarshalerLib.dll', 'RaGae.ArgumentLib.BooleanMarshalerLib.dll')"

:integermarshaler
echo IntegerMarshalerLib
set /P c=Download [Y/N]? 
if /I "%c%" EQU "Y" goto :integermarshalerdownload
if /I "%c%" EQU "N" goto :stringmarshaler
goto :integermarshaler

:integermarshalerdownload
powershell -Command "(New-Object Net.WebClient).DownloadFile('https://github.com/sunriax/argument/releases/latest/download/RaGae.ArgumentLib.IntegerMarshalerLib.dll', 'RaGae.ArgumentLib.IntegerMarshalerLib.dll')"

:stringmarshaler
echo StringMarshalerLib
set /P c=Download [Y/N]? 
if /I "%c%" EQU "Y" goto :stringmarshalerdownload
if /I "%c%" EQU "N" goto :doublemarshaler
goto :stringmarshaler

:stringmarshalerdownload
powershell -Command "(New-Object Net.WebClient).DownloadFile('https://github.com/sunriax/argument/releases/latest/download/RaGae.ArgumentLib.StringMarshalerLib.dll', 'RaGae.ArgumentLib.StringMarshalerLib.dll')"

:doublemarshaler
echo DoubleMarshalerLib
set /P c=Download [Y/N]? 
if /I "%c%" EQU "Y" goto :doublemarshalerdownload
if /I "%c%" EQU "N" goto :end
goto :doublemarshaler

:doublemarshalerdownload
powershell -Command "(New-Object Net.WebClient).DownloadFile('https://github.com/sunriax/argument/releases/latest/download/RaGae.ArgumentLib.DoubleMarshalerLib.dll', 'RaGae.ArgumentLib.DoubleMarshalerLib.dll')"

:end
echo End of downloading