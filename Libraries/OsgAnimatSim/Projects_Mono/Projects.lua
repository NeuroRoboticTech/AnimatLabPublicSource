
	project "OsgAnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include", 
 			      "../../../StdUtils", 
			      "../../../AnimatSim" }
		libdirs { "../../../../bin" }
		links { "dl"}
	  
		configuration { "Debug", "linux" }
			defines { "_DEBUG", "OSGANIMATLIBRARY_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("OsgAnimatSim_vc10D")
			links { "StdUtils_vc10D", 
				"AnimatSim_vc10D",
				"OpenThreadsd", 
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
				"osgWidgetd" }

			postbuildcommands { "cp Debug/libOsgAnimatSim_vc10D.so ../../../bin",
					    "cp Debug/libOsgAnimatSim_vc10D.so ../../../unit_test_bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG", "OSGANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("OsgAnimatSim_vc10")
			links { "StdUtils_vc10",
				"AnimatSim_vc10", 
				"OpenThreads",
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
				"osgWidget" }
			postbuildcommands { "cp Release/libOsgAnimatSim_vc10.so ../../../bin", 
					    "cp Release/libOsgAnimatSim_vc10.so ../../../unit_test_bin" }
