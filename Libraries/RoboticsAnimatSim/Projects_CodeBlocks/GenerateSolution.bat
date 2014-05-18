echo "Generating RoboticsAnimatSim CodeBlocks Solution"

..\..\..\bin\premake4m --os=linux --file=Solution.lua codeblocks
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/include" "../../../include"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/StdUtils" "../../../Libraries/StdUtils"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/AnimatSim" "../../../Libraries/AnimatSim"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/bin" "../../../bin"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>C:/Projects/AnimatLabSDK/3rdParty/stlsoft-1.9.117/include</Include>" "<Include>../../../../3rdParty/stlsoft-1.9.117/include</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>C:/Projects/AnimatLabSDK/3rdParty/DynamixelSDK/linux/import</Include>" "<Include>../../../../3rdParty/DynamixelSDK/linux/import</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>C:/Projects/AnimatLabSDK/3rdParty/openFrameworksArduino/src</Include>" "<Include>../../../../3rdParty/openFrameworksArduino/src</Include>"



@pause