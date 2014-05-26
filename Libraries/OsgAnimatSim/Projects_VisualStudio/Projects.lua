
	project "OsgAnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		
		configuration { "Debug or Debug_Double", "windows" }
			includedirs { "../../../include",
						  "../../../../3rdParty/OpenSceneGraph-3.0.1/include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../../../3rdParty/boost_1_54_0"}	  
			libdirs { "../../../lib",
					  "../../../../3rdParty/OpenSceneGraph-3.0.1/lib", 
					  "../../../../3rdParty/boost_1_54_0/lib" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "OSGANIMATLIBRARY_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("OsgAnimatSim_vc10D")
			links { "OpenThreadsd", 
					"osgAnimationd", 
					"osgd", 
					"osgDBd", 
					"osgFXd", 
					"osgGAd", 
					"osgManipulatord", 
					"osgParticled", 
					"osgShadowd", 
					"osgSimd", 
					"osgTerraind", 
					"osgTextd", 
					"osgUtild", 
					"osgViewerd", 
					"osgVolumed", 
					"osgWidgetd", 
					"opengl32", 
					"wsock32", 
					"netapi32", 
					"comctl32", 
					"wbemuuid" }
		postbuildcommands { "Copy $(OutDir)OsgAnimatSim_vc10D.lib ..\\..\\..\\lib\\OsgAnimatSim_vc10D.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin" }
	 
		configuration { "Release or Release_Double", "windows" }
			includedirs { "../../../include",
						  "../../../../3rdParty/OpenSceneGraph-3.0.1/include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../../../3rdParty/boost_1_54_0"}	  
			libdirs { "../../../lib",
					  "../../../../3rdParty/OpenSceneGraph-3.0.1/lib", 
					  "../../../../3rdParty/boost_1_54_0/lib" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "OSGANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("OsgAnimatSim_vc10")
			links { "OpenThreads",
					"osgAnimation",
					"osg",
					"osgDB",
					"osgFX",
					"osgGA",
					"osgManipulator",
					"osgParticle",
					"osgShadow",
					"osgSim",
					"osgTerrain",
					"osgText",
					"osgUtil",
					"osgViewer",
					"osgVolume",
					"osgWidget",
					"opengl32",
					"wsock32",
					"netapi32",
					"comctl32",
					"wbemuuid" }
			postbuildcommands { "Copy $(OutDir)OsgAnimatSim_vc10.lib ..\\..\\..\\lib\\OsgAnimatSim_vc10.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin" }
