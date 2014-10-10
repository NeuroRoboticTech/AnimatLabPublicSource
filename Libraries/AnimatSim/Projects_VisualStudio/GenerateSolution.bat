echo "Generating Animatsim Visual Studio Solution"

..\..\..\bin\premake4 --os=windows --file=Solution.lua vs2010
..\..\..\bin\premake4 --os=windows --file=Solution_x64.lua vs2010

@pause