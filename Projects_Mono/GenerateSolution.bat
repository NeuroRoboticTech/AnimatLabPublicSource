echo "Generating AnimatLabSimCode Mono Solution"

premake4m --os=linux --file=AnimatLabSimCode.lua monodevelop

..\Libraries\StdUtils\Projects_Mono\GenerateSolution.bat
..\Libraries\AnimatSim\Projects_Mono\GenerateSolution.bat

@pause