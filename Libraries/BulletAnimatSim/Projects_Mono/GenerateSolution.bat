echo "Generating VortexAnimatSim Mono Solution"

..\..\..\bin\premake4m --os=linux --file=Solution.lua monodevelop
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\..\include</Include>" "<Include>../../../../include</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<LibPath>..\..\..\..\bin</LibPath>" "<LibPath>../../../../bin</LibPath>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\StdUtils</Include>" "<Include>../../../StdUtils</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\AnimatSim</Include>" "<Include>../../../AnimatSim</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "</OutputPath>" "</OutputPath><ExtraCompilerArguments>-std=c++0x -Wl,-rpath,'$ORIGIN'</ExtraCompilerArguments>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\VortexAnimatSim</Include>" "<Include>../../../VortexAnimatSim</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\..\..\3rdParty\boost_1_54_0</Include>" "<Include>../../../../../3rdParty/boost_1_54_0</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\Vortex_5_1\include</Include>" "<Include>../../../Vortex_5_1/include</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\Vortex_5_1\3rdparty\osg-2.8.3\include</Include>" "<Include>../../../Vortex_5_1/3rdparty/osg-2.8.3/include</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\..\OsgAnimatSim</Include>" "<Include>../../../OsgAnimatSim</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<LibPath>..\..\..\Vortex_5_1\3rdparty\osg-2.8.3\lib</LibPath>" "<LibPath>../../../Vortex_5_1/3rdparty/osg-2.8.3/lib</LibPath>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<LibPath>..\..\..\Vortex_5_1\lib</LibPath>" "<LibPath>../../../Vortex_5_1/lib</LibPath>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<LibPath>..\..\..\Vortex_5_1\3rdparty\boost-1.45.0\lib</LibPath>" "<LibPath>../../../Vortex_5_1/3rdparty/boost-1.45.0/lib</LibPath>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\include</Include>" "<Include>../include</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\Libraries\Vortex_5_1\include</Include>" "<Include>../Libraries/Vortex_5_1/include</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\Libraries\Vortex_5_1\3rdparty\osg-2.8.3\include</Include>" "<Include>../Libraries/Vortex_5_1/3rdparty/osg-2.8.3/include</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\Libraries\StdUtils</Include>" "<Include>../Libraries/StdUtils</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\Libraries\AnimatSim</Include>" "<Include>../Libraries/AnimatSim</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\Libraries\OsgAnimatSim</Include>" "<Include>../Libraries/OsgAnimatSim</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\Libraries\VortexAnimatSim</Include>" "<Include>../Libraries/VortexAnimatSim</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<Include>..\..\3rdParty\boost_1_54_0</Include>" "<Include>../../3rdParty/boost_1_54_0</Include>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<LibPath>..\Libraries\Vortex_5_1\3rdparty\osg-2.8.3\lib</LibPath>" "<LibPath>../Libraries/Vortex_5_1/3rdparty/osg-2.8.3/lib</LibPath>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<LibPath>..\Libraries\Vortex_5_1\lib</LibPath>" "<LibPath>../Libraries/Vortex_5_1/lib</LibPath>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<LibPath>..\Libraries\Vortex_5_1\3rdparty\boost-1.45.0\lib</LibPath>" "<LibPath>../Libraries/Vortex_5_1/3rdparty/boost-1.45.0/lib</LibPath>"
TIMEOUT /T 2
..\..\..\bin\fart *.cproj "<LibPath>..\..\3rdParty\boost_1_54_0\lib</LibPath>" "<LibPath>../../3rdParty/boost_1_54_0/lib</LibPath>"


@pause