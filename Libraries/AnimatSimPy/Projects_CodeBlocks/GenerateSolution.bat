echo "Generating AnimatSimPy CodeBlocks Solution"

..\..\..\bin\premake4m --os=linux --file=AnimatSimPy.lua codeblocks
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/include" "../../../include"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/StdUtils" "../../../Libraries/StdUtils"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/AnimatSim" "../../../Libraries/AnimatSim"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/FiringRateSim" "../../../Libraries/FiringRateSim"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/bin" "../../../bin"

@pause