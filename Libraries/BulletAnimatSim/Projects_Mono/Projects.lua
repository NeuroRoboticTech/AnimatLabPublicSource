
	project "BulletAnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
			 "../*.cpp"}
		includedirs { "../../../../include",
			      "../../../../../3rdParty/osgWorks_03_00_00/include",
			      "../../../../../3rdParty/Bullet-2.82/src",
			      "../../../../../3rdParty/osgBullet_03_00_00/include",
			      "../../../StdUtils", 
		   	      "../../../AnimatSim",
			      "../../../OsgAnimatSim" }
		libdirs { "../../../../bin",
			  "../../../../../3rdParty/osgBullet_03_00_00/bin/lib/x86_64-linux-gnu" }
		links { "dl" }
	  
		configuration { "Debug", "linux" }
			defines { "_DEBUG", "OSGBULLET_STATIC", "BULLETANIMATLIBRARY_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("BulletAnimatSim_vc10D_single")
			links { "StdUtils_vc10D", 
				"AnimatSim_vc10D",
				"OsgAnimatSim_vc10D",
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
				"osgbCollision_single_debug"}
			postbuildcommands { "cp Debug/libBulletAnimatSim_vc10D_single.so ../../../bin" }

		configuration { "Debug_Double", "linux" }
			defines { "_DEBUG", "OSGBULLET_STATIC", "BULLETANIMATLIBRARY_EXPORTS", "BT_USE_DOUBLE_PRECISION" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("BulletAnimatSim_vc10D_double")
			links { "StdUtils_vc10D", 
				"AnimatSim_vc10D",
				"OsgAnimatSim_vc10D",
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
				"osgbCollision_double_debug" }
			postbuildcommands { "cp Debug/libBulletAnimatSim_vc10D_double.so ../../../bin" }
			
		configuration { "Release", "linux" }
			defines { "NDEBUG", "OSGBULLET_STATIC", "BULLETANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("BulletAnimatSim_vc10")
			links { "StdUtils_vc10",
				"AnimatSim_vc10", 
				"OsgAnimatSim_vc10",
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
				"osgbCollision_single"}					
			postbuildcommands { "cp Release/libBulletAnimatSim_vc10.so ../../../bin", 
					    "cp Release/libBulletAnimatSim_vc10.so ../../../unit_test_bin" }

		configuration { "Release_Double", "linux" }
			defines { "_DEBUG", "OSGBULLET_STATIC", "BULLETANIMATLIBRARY_EXPORTS", "BT_USE_DOUBLE_PRECISION" }
			flags   { "Symbols", "SEH" }
			targetdir ("Release")
			targetname ("BulletAnimatSim_vc10_double")
			links { "StdUtils_vc10", 
				"AnimatSim_vc10",
				"OsgAnimatSim_vc10",
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
				"osgbCollision_double" }
			postbuildcommands { "cp Debug/libBulletAnimatSim_vc10_double.so ../../../bin" }

