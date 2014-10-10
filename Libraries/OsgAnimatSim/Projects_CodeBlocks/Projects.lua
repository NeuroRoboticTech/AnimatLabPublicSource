
	project "OsgAnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		buildoptions { "-std=c++0x" }
		includedirs { "../../../include", 
 			      "../../StdUtils", 
			      "../../AnimatSim" }
		libdirs { "../../../bin" }
		links { "dl"}
	  
		configuration { "Debug or Debug_Double", "linux" }
			defines { "_DEBUG", "OSGANIMATLIBRARY_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("OsgAnimatSim_debug")
			links { "StdUtils_debug", 
				"AnimatSim_debug",
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

			postbuildcommands { "cp Debug/libOsgAnimatSim_debug.so ../../../bin" }
	 
		configuration { "Release or Release_Double", "linux" }
			defines { "NDEBUG", "OSGANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("OsgAnimatSim")
			links { "StdUtils",
				"AnimatSim", 
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
			postbuildcommands { "cp Release/libOsgAnimatSim.so ../../../bin" }
