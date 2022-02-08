echo on
cd osu-farmer\bin\x64\Release\net6.0-windows10.0.19041\win10-x64
mkdir app
ROBOCOPY "%CD%" "%CD%\app\ " /XF *.lnk /XD app /E /IS /MOVE
@REM powershell "$s=(New-Object -COM WScript.Shell).CreateShortcut('%cd%\osu!Farmer.lnk');$s.TargetPath='%%windir%\system32\cmd.exe /c start \app\osu-farmer.exe';$s.Save()"

echo off
set SCRIPT="%TEMP%\%RANDOM%-%RANDOM%-%RANDOM%-%RANDOM%.vbs"

echo Set oWS = WScript.CreateObject("WScript.Shell") >> %SCRIPT%
echo sLinkFile = "%cd%\osu!Farmer.lnk" >> %SCRIPT%
echo Set oLink = oWS.CreateShortcut(sLinkFile) >> %SCRIPT%
echo oLink.TargetPath = "%%windir%%\system32\cmd.exe" >> %SCRIPT%
echo oLink.Arguments = "/c start .\app\osu-farmer.exe" >> %SCRIPT%
@REM echo oLink.IconLocation = ".\app\osu-farmer.exe, 0" >> %SCRIPT%
echo oLink.Save >> %SCRIPT%

cscript /nologo %SCRIPT%
del %SCRIPT%
echo on
pause