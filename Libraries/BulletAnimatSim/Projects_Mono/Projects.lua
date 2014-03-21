
	project "BulletAnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
			 "../*.cpp"}
		includedirs { "../../../../include", 
			      "../../../../../3rdParty/OpenSceneGraph-3.0.1/include",
			      "../../../../../3rdParty/osgWorks_03_00_00/include",
			      "../../../../../3rdParty/Bullet-2.82/src",
			      "../../../../../3rdParty/osgBullet_03_00_00/include",
			      "../../../StdUtils", 
		   	      "../../../AnimatSim",
			      "../../../OsgAnimatSim" }
		libdirs { "../../../../bin",
				  "../../../../../3rdParty/OpenSceneGraph-3.0.1/bin",
				  "../../../../../3rdParty/osgWorks_03_00_00/bin",
				  "../../../../../3rdParty/Bullet-2.82/bin",
				  "../../../../../3rdParty/osgBullet_03_00_00/bin" }
		links { "dl" }
	  
		configuration { "Debug", "linux" }
			defines { "_DEBUG", "OSGBULLET_STATIC", "BULLETANIMATLIBRARY_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("BulletAnimatSim_vc10D_single")
			links { "StdUtils_vc10D", 
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
				"BulletDynamics_single_debug",
				"BulletCollision_single_debug",
				"LinearMath_single_debug",
				"BulletSoftBody_single_debug"}
			postbuildcommands { "cp Debug/libBulletAnimatSim_vc10D_single.so ../../../bin" }

		configuration { "Debug_Double", "linux" }
			defines { "_DEBUG", "OSGBULLET_STATIC", "BULLETANIMATLIBRARY_EXPORTS", "BT_USE_DOUBLE_PRECISION" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("BulletAnimatSim_vc10D_double")
			links { "StdUtils_vc10D", 
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
				"BulletDynamics_double_debug",
				"BulletCollision_double_debug",
				"LinearMath_double_debug",
				"BulletSoftBody_double_debug"}
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

