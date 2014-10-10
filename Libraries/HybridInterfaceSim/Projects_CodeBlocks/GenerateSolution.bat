echo "Generating RoboticsAnimatSim CodeBlocks Solution"

..\..\..\bin\premake4m --os=linux --file=Solution.lua codeblocks
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/include" "../../../include"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/StdUtils" "../../../Libraries/StdUtils"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/AnimatSim" "../../../Libraries/AnimatSim"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/RoboticsAnimatSim" "../../../Libraries/RoboticsAnimatSim"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/bin" "../../../bin"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/3rdParty/stlsoft-1.9.117/include" "../../../../3rdParty/stlsoft-1.9.117/include"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/3rdParty/DynamixelSDK/linux/include" "../../../../3rdParty/DynamixelSDK/linux/include"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/3rdParty/openFrameworksArduino/src" "../../../../3rdParty/openFrameworksArduino/src"

@pause