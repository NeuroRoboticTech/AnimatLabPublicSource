echo "Generating RoboticsAnimatSim Mono Solution"

..\..\..\bin\premake4m --os=linux --file=Solution.lua monodevelop
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\..\include</Include>" "<Include>../../../../include</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<LibPath>..\..\..\..\bin</LibPath>" "<LibPath>../../../../bin</LibPath>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\StdUtils</Include>" "<Include>../../../StdUtils</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\AnimatSim</Include>" "<Include>../../../AnimatSim</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "</OutputPath>" "</OutputPath><ExtraCompilerArguments>-std=c++0x -Wl,-rpath,'$ORIGIN'</ExtraCompilerArguments>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\include</Include>" "<Include>../include</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\Libraries\StdUtils</Include>" "<Include>../Libraries/StdUtils</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\Libraries\AnimatSim</Include>" "<Include>../Libraries/AnimatSim</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\Libraries\RoboticsAnimatSim</Include>" "<Include>../Libraries/RoboticsAnimatSim</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\..\..\3rdParty\stlsoft-1.9.117\include</Include>" "<Include>../../../../../3rdParty/stlsoft-1.9.117/include</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\..\..\3rdParty\DynamixelSDK\linux\include</Include>" "<Include>../../../../../3rdParty/DynamixelSDK/linux/include</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\..\..\3rdParty\openFrameworksArduino\src</Include>" "<Include>../../../../../3rdParty/openFrameworksArduino/src</Include>"


@pause