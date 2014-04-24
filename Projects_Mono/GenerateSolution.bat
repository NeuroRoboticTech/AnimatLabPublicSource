echo "Generating AnimatLabSimCode Mono Solution"

..\bin\premake4m --os=linux --file=AnimatLabSimCode.lua monodevelop
..\bin\premake4m --os=linux --file=RoboticsAnimatLabSimCode.lua monodevelop

@pause