echo "Generating PyAnimatSim Visual Studio Solution"

..\..\..\bin\premake4 --os=windows --file=PyAnimatSim.lua vs2010

@pause