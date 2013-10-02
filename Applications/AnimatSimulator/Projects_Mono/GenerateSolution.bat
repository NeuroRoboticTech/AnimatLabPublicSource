echo "Generating AnimatSimulator Mono Solution"

..\..\..\bin\premake4m --os=linux --file=Solution.lua monodevelop

..\..\..\bin\fart *.cproj "<Include>..\..\..\..\include</Include>" "<Include>../../../../include</Include>"
..\..\..\bin\fart *.cproj "<LibPath>..\..\..\..\bin</LibPath>" "<LibPath>../../../../bin</LibPath>"
..\..\..\bin\fart *.cproj "</OutputName>" "</OutputName><ExtraCompilerArguments>-std=c++0x -Wl,-rpath,'$ORIGIN'</ExtraCompilerArguments>"

@pause