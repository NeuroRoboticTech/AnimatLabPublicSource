
	project "VortexAnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include", 
					  "../../../Vortex_5_1/include",
					  "../../../Vortex_5_1/3rdparty/osg-2.8.3/include",
					  "../../../StdUtils", 
					  "../../../AnimatSim",
					  "../../../OsgAnimatSim" }
		libdirs { "../../../../bin", 
				  "../../../Vortex_5_1/3rdparty/osg-2.8.3/lib",
				  "../../../Vortex_5_1/lib",
				  "../../../Vortex_5_1/3rdparty/boost-1.45.0/lib" }
		links { "dl", 
				"Vx51", 
				"VxController51", 
				"VxControllerPersistence51", 
				"VxGraphics51", 
				"VxPersistence51", 
				"VxPs51", 
				"VxPsExtraOSG51", 
				"VxVehicle51", 
				"VxVehiclePersistence51", 
				"VxExtra51", 
				"VxExtraOSG51", 
				"VxVehicleExtra51", 
				"VxVehicleExtraOSG51", 
				"VxOSG51-2.8.3", 
				"boostVx_filesystem", 
				"boostVx_regex", 
				"boostVx_serialization", 
				"boostVx_system" }
	  
		configuration { "Debug", "linux" }
			defines { "_DEBUG", "VORTEXANIMATLIBRARY_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("VortexAnimatSim_vc10D")
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
					"osgWidgetd" }
			postbuildcommands { "cp Debug/libVortexAnimatSim_vc10D.so ../../../bin",
								"cp Debug/libVortexAnimatSim_vc10D.so ../../../unit_test_bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG", "VORTEXANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("VortexAnimatSim_vc10")
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
					"osgWidget" }					
			postbuildcommands { "cp Release/libVortexAnimatSim_vc10.so ../../../bin", 
								"cp Release/libVortexAnimatSim_vc10.so ../../../unit_test_bin" }


	project "Vortex_UnitTests"
		language "C++"
		kind     "ConsoleApp"
		files  { "../Vortex_UnitTests/*.h",
				 "../Vortex_UnitTests/*.cpp"}
		includedirs { "../../../../include", 
					  "../../../Vortex_5_1/include",
					  "../../../Vortex_5_1/3rdparty/osg-2.8.3/include",
					  "../../../StdUtils",
					  "../../../AnimatSim",
					  "../../../OsgAnimatSim",
					  "../../../VortexAnimatSim",
					  "../../../../../3rdParty/boost_1_54_0"}	  
		libdirs { "../../../../bin",
				  "../../../Vortex_5_1/3rdparty/osg-2.8.3/lib",
				  "../../../Vortex_5_1/lib",
				  "../../../Vortex_5_1/3rdparty/boost-1.45.0/lib",
				  "../../../../../3rdParty/boost_1_54_0/lib" }
		targetdir ("../../../../bin")
		targetname ("Vortex_UnitTests")
		links { "Vx51", 
				"VxController51", 
				"VxControllerPersistence51", 
				"VxGraphics51", 
				"VxPersistence51", 
				"VxPs51", 
				"VxPsExtraOSG51", 
				"VxVehicle51", 
				"VxVehiclePersistence51", 
				"VxExtra51", 
				"VxExtraOSG51", 
				"VxVehicleExtra51", 
				"VxVehicleExtraOSG51", 
				"VxOSG51-2.8.3", 
				"boostVx_filesystem", 
				"boostVx_regex", 
				"boostVx_serialization", 
				"boostVx_system" }
		
		configuration { "Debug", "linux" }
			defines { "_DEBUG" }
			flags   { "Symbols", "SEH" }
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
					"osgWidgetd" }
	 
		configuration { "Release", "linux" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL" }
			flags   { "Optimize", "SEH" }
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
					"osgWidget" }					

								