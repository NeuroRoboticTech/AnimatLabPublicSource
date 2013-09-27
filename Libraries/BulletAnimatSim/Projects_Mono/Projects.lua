
	project "BulletAnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
			 "../*.cpp"}
		includedirs { "../../../../include", 
			      "../../../StdUtils", 
		   	      "../../../AnimatSim",
			      "../../../OsgAnimatSim",
			      "/usr/local/include/bullet" }
		libdirs { "../../../../bin" }
		links { "dl" }
	  
		configuration { "Debug", "linux" }
			defines { "_DEBUG", "OSGBULLET_STATIC", "BULLETANIMATLIBRARY_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("BulletAnimatSim_vc10D")
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
			postbuildcommands { "cp Debug/libBulletAnimatSim_vc10D.so ../../../bin",
					    "cp Debug/libBulletAnimatSim_vc10D.so ../../../unit_test_bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG", "OSGBULLET_STATIC", "BULLETANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("BulletAnimatSim_vc10")
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
				"osgWidget",
				"osgbDynamics",
				"osgbCollision",
				"osgwControls",
				"osgwQuery",
				"osgwTools",
				"BulletDynamics",
				"BulletCollision",
				"LinearMath",
				"BulletSoftBody"}					
			postbuildcommands { "cp Release/libBulletAnimatSim_vc10.so ../../../bin", 
					    "cp Release/libBulletAnimatSim_vc10.so ../../../unit_test_bin" }


	project "Bullet_UnitTests"
		language "C++"
		kind     "ConsoleApp"
		files  { "../Bullet_UnitTests/*.h",
                 "../Bullet_UnitTests/*.cpp"}
		targetdir ("../../../../bin")
		targetname ("Bullet_UnitTests")				
		includedirs { "../include",
			      "../Libraries/StdUtils",
			      "../Libraries/AnimatSim",
			      "../Libraries/OsgAnimatSim",
			      "../Libraries/BulletAnimatSim",
			      "/usr/local/include/bullet",
			      "../../3rdParty/boost_1_54_0"}	  
		libdirs { ".",
			  "../../../../bin" }
		links { "boost_system", 
			"boost_filesystem",
			"boost_unit_test_framework" }
		
		configuration { "Debug", "linux" }
			defines { "_DEBUG", "OSGBULLET_STATIC" }
			flags   { "Symbols", "SEH" }
			links { "StdUtils_vc10D", 
				"AnimatSim_vc10D",
				"OsgAnimatSim_vc10D",
				"BulletAnimatSim_vc10D",
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
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG", "OSGBULLET_STATIC", "OSGBULLET_STATIC" }
			flags   { "Optimize", "SEH" }
			links { "StdUtils_vc10",
				"AnimatSim_vc10", 
				"OsgAnimatSim_vc10", 
				"BulletAnimatSim_vc10", 
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
				"osgbDynamics",
				"osgbCollision",
				"osgwControls",
				"osgwQuery",
				"osgwTools",
				"BulletDynamics",
				"BulletCollision",
				"LinearMath",
				"BulletSoftBody"}					
					

								
