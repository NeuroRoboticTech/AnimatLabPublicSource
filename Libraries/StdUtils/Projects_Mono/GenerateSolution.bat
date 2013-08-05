echo "Generating StdUtils Mono Solution"

premake4m --os=linux --file=Solution.lua monodevelop
..\..\..\bin\fart *.cproj "<Include>..\..\..\..\include</Include>" "<Include>../../../../include</Include>"
..\..\..\bin\fart *.cproj "<LibPath>..\..\..\..\bin</LibPath>" "<LibPath>../../../../bin</LibPath>"
..\..\..\bin\fart *.cproj "<Include>..\..\..\StdUtils</Include>" "<Include>../../../StdUtils</Include>"
..\..\..\bin\fart *.cproj "<Include>..\..\..\..\..\3rdParty\boost_1_54_0</Include>" "<Include>../../../../../3rdParty/boost_1_54_0</Include>"
..\..\..\bin\fart *.cproj "<LibPath>..\..\..\..\..\3rdParty\boost_1_54_0\lib\static</LibPath>" "<LibPath>../../../../../3rdParty/boost_1_54_0/lib/static</LibPath>"
..\..\..\bin\fart *.cproj "</OutputPath>" "</OutputPath><ExtraCompilerArguments>-std=c++0x -Wl,-rpath,'$ORIGIN'</ExtraCompilerArguments>"

@pause