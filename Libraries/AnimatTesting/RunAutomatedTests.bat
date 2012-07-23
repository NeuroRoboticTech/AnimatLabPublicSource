rem @echo off
set hour=%time:~0,2%
if "%hour:~0,1%" == " " set hour=0%hour:~1,1%
rem echo hour=%hour%
set min=%time:~3,2%
if "%min:~0,1%" == " " set min=0%min:~1,1%
rem echo min=%min%
set secs=%time:~6,2%
if "%secs:~0,1%" == " " set secs=0%secs:~1,1%
rem echo secs=%secs%

set year=%date:~-4,4%
rem echo year=%year%
set month=%date:~4,2%
if "%month:~0,1%" == " " set month=0%month:~1,1%
rem echo month=%month%
set day=%date:~7,2%
if "%day:~0,1%" == " " set day=0%day:~1,1%
rem echo day=%day%

set testfile=TestResults_%year%%month%%day%_%hour%%min%%secs%.trx

"C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\mstest.exe" /testcontainer:C:\Projects\AnimatLabSDK\AnimatLabPublicSource\bin\AnimatTesting.dll /resultsfile:C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatTesting\TestResults\%testfile%
