
	project "VortexAnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include", 
					  "../../../Vortex_5_1/include",
					  "../../../Vortex_5_1/3rdparty/osg-2.8.3/include",
					  "../../../Vortex_5_1/3rdparty/sdl-1.2.14/include",
					  "../../../StdUtils", 
					  "../../../AnimatSim" }
		libdirs { "../../../../bin", 
				  "../../../Vortex_5_1/3rdparty/osg-2.8.3/lib",
				  "../../../Vortex_5_1/lib",
				  "../../../Vortex_5_1/3rdparty/boost-1.45.0/lib",
				  "../../../Vortex_5_1/3rdparty/sdl-1.2.14/lib" }
		links { "dl"}
	  
		configuration { "Debug", "linux" }
			defines { "_DEBUG", "VORTEXANIMATLIBRARY_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("VortexAnimatSim_vc10D")
			links { "StdUtils_vc10D", 
					"AnimatSim_vc10D",
					"libVx51d", 
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
					"libboostVx_filesystem", 
					"libboostVx_regex", 
					"libboostVx_serialization", 
					"libboostVx_system", 
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
					"SDL" }
			postbuildcommands { "cp Debug/libVortexAnimatSim_vc10D.so ../../../bin",
								"cp Debug/libVortexAnimatSim_vc10D.so ../../../unit_test_bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG", "VORTEXANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("VortexAnimatSim_vc10")
			links { "StdUtils_vc10",
					"AnimatSim_vc10", 
					"libVx51", 
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
					"libboostVx_filesystem",
					"libboostVx_regex",
					"libboostVx_serialization",
					"libboostVx_system",
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
					"SDL" }					
			postbuildcommands { "cp Release/libVortexAnimatSim_vc10.so ../../../bin", 
								"cp Release/libVortexAnimatSim_vc10.so ../../../unit_test_bin" }
