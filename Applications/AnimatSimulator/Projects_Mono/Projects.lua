
	project "AnimatSimulator"
		language "C++"
		kind     "ConsoleApp"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include" }
		libdirs { "../../../../bin" }
	  
		configuration { "Debug", "linux" }
			defines { "_DEBUG"}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("AnimatSimulatorD")
			links { "BootstrapLoader_vc10D", "dl", "pthread" }
			postbuildcommands { "cp Debug/AnimatSimulatorD ../../../bin" }
	  
		configuration { "Debug_Static", "linux" }
			defines { "_DEBUG", "OSGBULLET_STATIC", "ANIMAT_STATIC"}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("AnimatSimulatorD")
			includedirs { "../../../../Libraries/StdUtils",
				      "../../../../Libraries/AnimatSim",
				      "../../../../Libraries/OsgAnimatSim",
				      "../../../../Libraries/BulletAnimatSim",
				      "/usr/local/include/bullet" }
			links { "BootstrapLoader_vc10D", 
				"dl", 
				"pthread",
				"StdUtils_vc10D", 
				"AnimatSim_vc10D",
				"OsgAnimatSim_vc10D",
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
				"osgbDynamicsd",
				"osgbCollisiond",
				"osgwControlsd",
				"osgwQueryd",
				"osgwToolsd",
				"BulletDynamics_debug",
				"BulletCollision_debug",
				"LinearMath_debug",
				"BulletSoftBody_debug"}
			postbuildcommands { "cp Debug/AnimatSimulatorD ../../../bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("AnimatSimulator")
			links { "BootstrapLoader_vc10", "dl", "pthread" }
			postbuildcommands { "cp Release/AnimatSimulator ../../../bin" }
