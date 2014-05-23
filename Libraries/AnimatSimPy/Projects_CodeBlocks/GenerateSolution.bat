echo "Generating AnimatSimPy Mono Solution"

..\..\..\bin\premake4m --os=linux --file=AnimatSimPy.lua codeblocks
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/include" "../../../include"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/Libraries/StdUtils" "../../../Libraries/StdUtils"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/AnimatLabPublicSource/bin" "../../../bin"
TIMEOUT /T 2
..\..\..\bin\fart *.cbp "C:/Projects/AnimatLabSDK/3rdParty/stlsoft-1.9.117/include" "../../../../3rdParty/stlsoft-1.9.117/include"

@pause