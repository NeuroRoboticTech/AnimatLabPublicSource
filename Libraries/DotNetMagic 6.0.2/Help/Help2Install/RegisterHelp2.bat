REM In order to run this Help 2 registration script you must have
REM  all COL_* files and the .hxs file in the same directory as 
REM  this file. Additionally, InnoHxReg.exe must be in the same
REM  directory or in the system path.
REM
REM For more information on deploying Help 2 files, refer to the 
REM  HelpStudio on-line help file under the 'Deploying the Help 
REM  System' section.

CD %1%

REM Register the Namespace
InnovaHxReg /R /N /Namespace:DotNetMagic /Description:"DotNetMagic" /Collection:COL_DotNetMagic.hxc

REM Register the help file (title in Help 2.0 terminology)
InnovaHxReg /R /T /namespace:DotNetMagic /id:DotNetMagic /langid:1033 /helpfile:DotNetMagicHelp.hxs

REM Un-comment to plug in to the Visual Studio.NET 2002 help system
REM InnovaHxReg /R /P /productnamespace:MS.VSCC /producthxt:_DEFAULT /namespace:DotNetMagic /hxt:_DEFAULT

REM Un-comment to plug in to the Visual Studio.NET 2003 help system
REM InnovaHxReg /R /P /productnamespace:MS.VSCC.2003 /producthxt:_DEFAULT /namespace:DotNetMagic /hxt:_DEFAULT

REM Un-comment to plug in to the Visual Studio.NET 2005 help system
InnovaHxReg /R /P /productnamespace:MS.VSIPCC.v80 /producthxt:_DEFAULT /namespace:DotNetMagic /hxt:_DEFAULT