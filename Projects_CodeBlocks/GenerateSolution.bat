echo "Generating AnimatLabSimCode CodeBlocks Solution"

..\bin\premake4m --os=linux --file=AnimatLabSimCode.lua codeblocks
..\bin\premake4m --os=linux --file=RoboticsAnimatLabSimCode.lua codeblocks

@pause