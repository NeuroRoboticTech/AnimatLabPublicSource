
	project "VortexAnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../include", 
					  "../../Vortex_5_1/include",
					  "../../Vortex_5_1/3rdparty/osg-2.8.3/include",
					  "../../Vortex_5_1/3rdparty/sdl-1.2.14/include",
					  "../../StdUtils",
					  "../../AnimatSim"}	  
		libdirs { "../../../lib",
				  "$(OutDir)",
				  "../../Vortex_5_1/3rdparty/osg-2.8.3/lib",
				  "../../Vortex_5_1/lib",
				  "../../Vortex_5_1/3rdparty/boost-1.45.0/lib",
				  "../../Vortex_5_1/3rdparty/sdl-1.2.14/lib" }
		
		configuration { "Debug", "windows" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "VORTEXANIMATLIBRARY_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("VortexAnimatSim_vc10D")
			links { "libVx51d", 
					"libVxController51d", 
					"libVxControllerPersistence51d", 
					"libVxGraphics51d", 
					"libVxPersistence51d", 
					"libVxPs51d", 
					"libVxPsExtraOSG51d", 
					"libVxVehicle51d", 
					"libVxVehiclePersistence51d", 
					"libVxExtra51d", 
					"libVxExtraOSG51d", 
					"libVxVehicleExtra51d", 
					"libVxVehicleExtraOSG51d", 
					"libVxOSG51d-2.8.3", 
					"libboostVx_filesystem-vc100-mt-gd", 
					"libboostVx_regex-vc100-mt-gd", 
					"libboostVx_serialization-vc100-mt-gd", 
					"libboostVx_system-vc100-mt-gd", 
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
					"SDL", 
					"opengl32", 
					"wsock32", 
					"netapi32", 
					"comctl32", 
					"wbemuuid" }
			postbuildcommands { "Copy $(OutDir)VortexAnimatSim_vc10D.lib ..\\..\\..\\lib\\VortexAnimatSim_vc10D.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin",
								"Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }
	 
		configuration { "Release", "windows" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "VORTEXANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("VortexAnimatSim_vc10")
			links { "libVx51", 
					"libVxController51",
					"libVxControllerPersistence51",
					"libVxGraphics51",
					"libVxPersistence51",
					"libVxPs51",
					"libVxPsExtraOSG51",
					"libVxVehicle51",
					"libVxVehiclePersistence51",
					"libVxExtra51",
					"libVxExtraOSG51",
					"libVxVehicleExtra51",
					"libVxVehicleExtraOSG51",
					"libVxOSG51-2.8.3",
					"libboostVx_filesystem-vc100-mt",
					"libboostVx_regex-vc100-mt",
					"libboostVx_serialization-vc100-mt",
					"libboostVx_system-vc100-mt",
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
					"SDL",
					"opengl32",
					"wsock32",
					"netapi32",
					"comctl32",
					"wbemuuid" }
			postbuildcommands { "Copy $(OutDir)VortexAnimatSim_vc10.lib ..\\..\\..\\lib\\VortexAnimatSim_vc10.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin", 
								"Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }
