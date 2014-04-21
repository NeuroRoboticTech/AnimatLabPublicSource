echo "Generating AnimatSim Mono Solution"

..\..\..\bin\premake4m --os=linux --file=Solution.lua codeblocks
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/include" "../../../include"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/StdUtils" "../../../Libraries/StdUtils"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/bin" "../../../bin"

@pause