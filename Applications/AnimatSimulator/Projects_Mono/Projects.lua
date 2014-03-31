
	project "AnimatSimulator"
		language "C++"
		kind     "ConsoleApp"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include" }
		libdirs { "../../../../bin" }
	  
		configuration { "Debug or Debug_Double", "linux" }
			defines { "_DEBUG"}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("animatsimulator_debug")
			links { "AnimatBootstrapLoader_debug", "dl", "pthread" }
			postbuildcommands { "cp Debug/animatsimulator_debug ../../../bin" }
	  
		configuration { "Debug_Static", "linux" }
			defines { "_DEBUG", "OSGBULLET_STATIC", "ANIMAT_STATIC"}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("animatsimulator_debug")
			includedirs { "../../../../Libraries/StdUtils",
				      "../../../../Libraries/AnimatSim",
				      "../../../../Libraries/OsgAnimatSim",
				      "../../../../Libraries/BulletAnimatSim",
				      "/usr/local/include/bullet" }
			links { "AnimatBootstrapLoader_debug", 
				"dl", 
				"pthread",
				"StdUtils_debug", 
				"AnimatSim_debug",
				"OsgAnimatSim_debug",
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
				"osgWidgetd",
				"osgbDynamics_single_debug",
				"osgbCollision_single_debug",
				"osgwControls_animat_debug",
				"osgwQuery_animat_debug",
				"osgwTools_animat_debug",
				"BulletCollision_single_debug",
				"BulletDynamics_single_debug",
				"LinearMath_single_debug",
				"BulletSoftBody_single_debug"}
			postbuildcommands { "cp Debug/animatsimulator_debug ../../../bin" }
	 
		configuration { "Release or Release_Double", "linux" }
			defines { "NDEBUG" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("animatsimulator")
			links { "AnimatBootstrapLoader", "dl", "pthread" }
			postbuildcommands { "cp Release/animatsimulator ../../../bin" }
