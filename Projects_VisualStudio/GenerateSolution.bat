echo "Generating AnimatLabSimCode Visual Studio Solution"

..\bin\premake4 --os=windows --file=AnimatLabSimCode.lua vs2010
..\bin\premake4 --os=windows --file=AnimatLabSimCode_x64.lua vs2010
..\bin\premake4 --os=windows --file=RoboticAnimatLabSimCode.lua vs2010
..\bin\premake4 --os=windows --file=RoboticAnimatLabSimCode_x64.lua vs2010

@pause