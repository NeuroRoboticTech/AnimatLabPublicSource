
	project "BulletAnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
			 "../*.cpp"}
		buildoptions { "-std=c++0x" }
		includedirs { "../../../include",
			      "../../StdUtils", 
		   	      "../../AnimatSim",
			      "../../OsgAnimatSim",
		   	      "/usr/local/include/bullet" }
		libdirs { "../../../bin" }
		links { "dl" }
	  
		configuration { "Debug", "linux" }
			defines { "_DEBUG", "OSGBULLET_STATIC", "BULLETANIMATLIBRARY_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("BulletAnimatSim_debug_single")
			links { "StdUtils_debug", 
				"AnimatSim_debug",
				"OsgAnimatSim_debug",
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
				"osgWidget",
				"osgbDynamics_single_debug",
				"osgbCollision_single_debug",
				"osgwQuery_animat_debug",
				"osgwTools_animat_debug",
				"BulletCollision_single_debug",
				"BulletDynamics_single_debug",
				"LinearMath_single_debug",
				"BulletSoftBody_single_debug" }
			postbuildcommands { "cp Debug/libBulletAnimatSim_debug_single.so ../../../bin" }

		configuration { "Debug_Double", "linux" }
			defines { "_DEBUG", "OSGBULLET_STATIC", "BULLETANIMATLIBRARY_EXPORTS", "BT_USE_DOUBLE_PRECISION" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("BulletAnimatSim_debug_double")
			links { "StdUtils_debug", 
				"AnimatSim_debug",
				"OsgAnimatSim_debug",
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
				"osgWidget",
				"osgbDynamics_double_debug",
				"osgbCollision_double_debug",
				"osgwQuery_animat_debug",
				"osgwTools_animat_debug",
				"BulletCollision_double_debug",
				"BulletDynamics_double_debug",
				"LinearMath_double_debug",
				"BulletSoftBody_double_debug"  }
			postbuildcommands { "cp Debug/libBulletAnimatSim_debug_double.so ../../../bin" }
			
		configuration { "Release", "linux" }
			defines { "NDEBUG", "OSGBULLET_STATIC", "BULLETANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("BulletAnimatSim_single")
			links { "StdUtils",
				"AnimatSim", 
				"OsgAnimatSim",
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
				"osgWidget",
				"osgbDynamics_single",
				"osgbCollision_single",
				"osgwQuery_animat",
				"osgwTools_animat",
				"BulletCollision_single",
				"BulletDynamics_single",
				"LinearMath_single",
				"BulletSoftBody_single" }					
			postbuildcommands { "cp Release/libBulletAnimatSim_single.so ../../../bin" }

		configuration { "Release_Double", "linux" }
			defines { "_DEBUG", "OSGBULLET_STATIC", "BULLETANIMATLIBRARY_EXPORTS", "BT_USE_DOUBLE_PRECISION" }
			flags   { "Symbols", "SEH" }
			targetdir ("Release")
			targetname ("BulletAnimatSim_double")
			links { "StdUtils", 
				"AnimatSim",
				"OsgAnimatSim",
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
				"osgWidget",
				"osgbDynamics_double",
				"osgbCollision_double",
				"osgwQuery_animat",
				"osgwTools_animat",
				"BulletCollision_double",
				"BulletDynamics_double",
				"LinearMath_double",
				"BulletSoftBody_double" }
			postbuildcommands { "cp Release/libBulletAnimatSim_double.so ../../../bin" }

