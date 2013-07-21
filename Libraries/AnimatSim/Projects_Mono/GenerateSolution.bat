echo "Generating AnimatSim Mono Solution"

premake4m --os=linux --file=Solution.lua monodevelop

..\..\..\bin\fart *.cproj "<Include>..\..\..\..\include</Include>" "<Include>../../../../include</Include>"
..\..\..\bin\fart *.cproj "<LibPath>..\..\..\..\lib</LibPath>" "<LibPath>../../../../lib</LibPath>"
..\..\..\bin\fart *.cproj "<Include>..\..\..\StdUtils</Include>" "<Include>../../../StdUtils</Include>"

@pause