echo "Generating AnimatLabSimCode Visual Studio Solution"
premake4 --os=windows --file=AnimatLabSimCode.lua vs2010

cd ..\Libraries\BootstrapLoader\Projects_VisualStudio
@call GenerateSolution.bat

cd ..\..\StdUtils\Projects_VisualStudio
@call GenerateSolution.bat

cd ..\..\FiringRateSim\Projects_VisualStudio
@call GenerateSolution.bat

cd ..\..\IntegrateFireSim\Projects_VisualStudio
@call GenerateSolution.bat

cd ..\..\VortexAnimatSim\Projects_VisualStudio
@call GenerateSolution.bat

@pause