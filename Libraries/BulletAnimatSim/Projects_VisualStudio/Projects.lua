
	project "BulletAnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		
		configuration { "Debug", "windows" }
			includedirs { "../../../include",
						  "../../../../3rdParty/OpenSceneGraph-3.0.1/include",
						  "../../../../3rdParty/osgWorks_03_00_00/include",
						  "../../../../3rdParty/Bullet-2.82/src",
						  "../../../../3rdParty/osgBullet_03_00_00/include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../OsgAnimatSim",
						  "../../../../3rdParty/boost_1_54_0"}	  
			libdirs { "../../../lib",
					  "$(OutDir)",
					  "../../../../3rdParty/OpenSceneGraph-3.0.1/lib",
					  "../../../../3rdParty/osgWorks_03_00_00/lib",
					  "../../../../3rdParty/Bullet-2.82/lib",
					  "../../../../3rdParty/osgBullet_03_00_00/lib", 
					  "../../../../3rdParty/boost_1_54_0/lib" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "OSGBULLET_STATIC", "BULLETANIMATLIBRARY_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("BulletAnimatSim_vc10D_single")
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
					"osgbDynamicsd_single",
					"osgbCollisiond_single",
					"osgwControlsd",
					"osgwQueryd",
					"osgwToolsd",
					"BulletDynamics_vs2010_single_debug",
					"BulletCollision_vs2010_single_debug",
					"LinearMath_vs2010_single_debug",
					"BulletSoftBody_vs2010_single_debug",
					"wsock32", 
					"netapi32", 
					"comctl32", 
					"wbemuuid" }
			postbuildcommands { "Copy $(OutDir)BulletAnimatSim_vc10D_single.lib ..\\..\\..\\lib\\BulletAnimatSim_vc10D_single.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin" }

		configuration { "Debug_Double", "windows" }
			includedirs { "../../../include",
						  "../../../../3rdParty/OpenSceneGraph-3.0.1/include",
						  "../../../../3rdParty/osgWorks_03_00_00/include",
						  "../../../../3rdParty/Bullet-2.82/src",
						  "../../../../3rdParty/osgBullet_03_00_00/include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../OsgAnimatSim",
						  "../../../../3rdParty/boost_1_54_0"}	  
			libdirs { "../../../lib",
					  "$(OutDir)",
					  "../../../../3rdParty/OpenSceneGraph-3.0.1/lib",
					  "../../../../3rdParty/osgWorks_03_00_00/lib",
					  "../../../../3rdParty/Bullet-2.82/lib",
					  "../../../../3rdParty/osgBullet_03_00_00/lib", 
					  "../../../../3rdParty/boost_1_54_0/lib" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "OSGBULLET_STATIC", "BULLETANIMATLIBRARY_EXPORTS", "BT_USE_DOUBLE_PRECISION", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("BulletAnimatSim_vc10D_double")
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
					"osgbDynamicsd_double",
					"osgbCollisiond_double",
					"osgwControlsd",
					"osgwQueryd",
					"osgwToolsd",
					"BulletDynamics_vs2010_double_debug",
					"BulletCollision_vs2010_double_debug",
					"LinearMath_vs2010_double_debug",
					"BulletSoftBody_vs2010_double_debug",
					"wsock32", 
					"netapi32", 
					"comctl32", 
					"wbemuuid" }
			postbuildcommands { "Copy $(OutDir)BulletAnimatSim_vc10D_double.lib ..\\..\\..\\lib\\BulletAnimatSim_vc10D_double.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin" }								
								
		configuration { "Release", "windows" }
			includedirs { "../../../include",
						  "../../../../3rdParty/OpenSceneGraph-3.0.1/include",
						  "../../../../3rdParty/osgWorks_03_00_00/include",
						  "../../../../3rdParty/Bullet-2.82/src",
						  "../../../../3rdParty/osgBullet_03_00_00/include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../OsgAnimatSim",
						  "../../../../3rdParty/boost_1_54_0"}	  
			libdirs { "../../../lib",
					  "$(OutDir)",
					  "../../../../3rdParty/OpenSceneGraph-3.0.1/lib",
					  "../../../../3rdParty/osgWorks_03_00_00/lib",
					  "../../../../3rdParty/Bullet-2.82/lib",
					  "../../../../3rdParty/osgBullet_03_00_00/lib", 
					  "../../../../3rdParty/boost_1_54_0/lib" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "OSGBULLET_STATIC", "BULLETANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("BulletAnimatSim_vc10_single")
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
					"osgbDynamics_single",
					"osgbCollision_single",
					"osgwControls",
					"osgwQuery",
					"osgwTools",
					"BulletDynamics_vs2010_single",
					"BulletCollision_vs2010_single",
					"LinearMath_vs2010_single",
					"BulletSoftBody_vs2010_single",
					"wsock32",
					"netapi32",
					"comctl32",
					"wbemuuid" }
			postbuildcommands { "Copy $(OutDir)BulletAnimatSim_vc10_single.lib ..\\..\\..\\lib\\BulletAnimatSim_vc10_single.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin" }
								
		configuration { "Release_Double", "windows" }
			includedirs { "../../../include",
						  "../../../../3rdParty/OpenSceneGraph-3.0.1/include",
						  "../../../../3rdParty/osgWorks_03_00_00/include",
						  "../../../../3rdParty/Bullet-2.82/src",
						  "../../../../3rdParty/osgBullet_03_00_00/include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../OsgAnimatSim",
						  "../../../../3rdParty/boost_1_54_0"}	  
			libdirs { "../../../lib",
					  "$(OutDir)",
					  "../../../../3rdParty/OpenSceneGraph-3.0.1/lib",
					  "../../../../3rdParty/osgWorks_03_00_00/lib",
					  "../../../../3rdParty/Bullet-2.82/lib",
					  "../../../../3rdParty/osgBullet_03_00_00/lib", 
					  "../../../../3rdParty/boost_1_54_0/lib" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "OSGBULLET_STATIC", "BULLETANIMATLIBRARY_EXPORTS", "BT_USE_DOUBLE_PRECISION" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("BulletAnimatSim_vc10_double")
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
					"osgbDynamics_double",
					"osgbCollision_double",
					"osgwControls",
					"osgwQuery",
					"osgwTools",
					"BulletDynamics_vs2010_double",
					"BulletCollision_vs2010_double",
					"LinearMath_vs2010_double",
					"BulletSoftBody_vs2010_double",
					"wsock32",
					"netapi32",
					"comctl32",
					"wbemuuid" }
			postbuildcommands { "Copy $(OutDir)BulletAnimatSim_vc10_double.lib ..\\..\\..\\lib\\BulletAnimatSim_vc10_double.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin" }
