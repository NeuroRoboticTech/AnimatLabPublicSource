
	project "BulletAnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../include",
					  "../../Vortex_5_1/3rdparty/osg-2.8.3/include",
					  "../../StdUtils",
					  "../../AnimatSim",
					  "../../OsgAnimatSim"}	  
		libdirs { "../../../lib",
				  "$(OutDir)",
				  "../../Vortex_5_1/3rdparty/osg-2.8.3/lib" }
		
		configuration { "Debug", "windows" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "BULLETANIMATLIBRARY_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("BulletAnimatSim_vc10D")
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
			postbuildcommands { "Copy $(OutDir)BulletAnimatSim_vc10D.lib ..\\..\\..\\lib\\BulletAnimatSim_vc10D.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin",
								"Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }
	 
		configuration { "Release", "windows" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "BULLETANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("BulletAnimatSim_vc10")
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
			postbuildcommands { "Copy $(OutDir)BulletAnimatSim_vc10.lib ..\\..\\..\\lib\\BulletAnimatSim_vc10.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin", 
								"Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }

	project "Bullet_UnitTests"
		language "C++"
		kind     "ConsoleApp"
		files  { "../Bullet_UnitTests/*.h",
				 "../Bullet_UnitTests/*.cpp"}
		includedirs { "../../../include",
					  "../../Vortex_5_1/3rdparty/osg-2.8.3/include",
					  "../../StdUtils",
					  "../../AnimatSim",
					  "../../OsgAnimatSim",
					  "../../BulletAnimatSim",
					  "../../../../3rdParty/boost_1_54_0"}	  
		libdirs { "../../../lib",
				  "$(OutDir)",
				  "../../Vortex_5_1/3rdparty/osg-2.8.3/lib",
				  "../../../../3rdParty/boost_1_54_0/lib" }
		targetdir ("../../../bin")
		targetname ("Bullet_UnitTests")
		
		configuration { "Debug", "windows" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "_CRT_SECURE_NO_WARNINGS"	}
			flags   { "Symbols", "SEH" }
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
	 
		configuration { "Release", "windows" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL" }
			flags   { "Optimize", "SEH" }
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

								