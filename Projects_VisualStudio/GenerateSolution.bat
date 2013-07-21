echo "Generating AnimatLabSimCode Visual Studio Solution"
premake4 --os=windows --file=AnimatLabSimCode.lua vs2010

..\Libraries\StdUtils\Projects_Mono\GenerateSolution.bat
..\Libraries\AnimatSim\Projects_Mono\GenerateSolution.bat

@pause