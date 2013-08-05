echo "Generating AnimatLabSimCode Mono Solution"

premake4m --os=linux --file=AnimatLabSimCode.lua monodevelop

cd ..\Libraries\BootstrapLoader\Projects_Mono
@call GenerateSolution.bat

cd ..\..\StdUtils\Projects_Mono
@call GenerateSolution.bat

cd ..\..\FiringRateSim\Projects_Mono
@call GenerateSolution.bat

cd ..\..\IntegrateFireSim\Projects_Mono
@call GenerateSolution.bat

cd ..\..\OsgAnimatSim\Projects_Mono
@call GenerateSolution.bat

cd ..\..\VortexAnimatSim\Projects_Mono
@call GenerateSolution.bat

cd ..\..\..\Applications\AnimatSimulator\Projects_Mono
@call GenerateSolution.bat

@pause