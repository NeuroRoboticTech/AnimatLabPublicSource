
	project "OsgAnimatSim_x64"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}

		configuration { "Debug_x64 or Debug_Double_x64", "windows" }
			includedirs { "../../../include",
						  "../../../../3rdParty/OpenSceneGraph-3.0.1_x64/include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../../../3rdParty/boost_1_54_0",
					  "../../../../3rdParty/stlsoft-1.9.117/include"}	  
			libdirs { "../../../lib",
					  "../../../../3rdParty/OpenSceneGraph-3.0.1_x64/lib", 
					  "../../../../3rdParty/boost_1_54_0/lib_x64" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "OSGANIMATLIBRARY_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug_x64")
			targetname ("OsgAnimatSim_vc10D_x64")
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
		postbuildcommands { "Copy $(OutDir)OsgAnimatSim_vc10D_x64.lib ..\\..\\..\\lib\\OsgAnimatSim_vc10D_x64.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin_x64" }
	 
		configuration { "Release_x64 or Release_Double_x64", "windows" }
			includedirs { "../../../include",
						  "../../../../3rdParty/OpenSceneGraph-3.0.1_x64/include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../../../3rdParty/boost_1_54_0",
						  "../../../../3rdParty/stlsoft-1.9.117/include"}	  
			libdirs { "../../../lib",
					  "../../../../3rdParty/OpenSceneGraph-3.0.1_x64/lib", 
					  "../../../../3rdParty/boost_1_54_0/lib_x64" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "OSGANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release_x64")
			targetname ("OsgAnimatSim_vc10_x64")
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
			postbuildcommands { "Copy $(OutDir)OsgAnimatSim_vc10_x64.lib ..\\..\\..\\lib\\OsgAnimatSim_vc10_x64.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin_x64" }
