
	project "Robotics_UnitTests_x64"
		language "C++"
		kind     "ConsoleApp"
		files  { "../Robotics_UnitTests/*.h",
				 "../Robotics_UnitTests/*.cpp"}
		
		configuration { "Debug_x64 or Debug_Double_x64", "windows" }
			includedirs { "../../../include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../RoboticsAnimatSim",
						  "../../../../3rdParty/OpenSceneGraph-3.0.1_x64/include",
						  "../../../../3rdParty/boost_1_54_0"}	  
			libdirs { "../../../lib",
					  "$(OutDir)",
					  "../../../../3rdParty/OpenSceneGraph-3.0.1_x64/lib",
					  "../../../../3rdParty/boost_1_54_0/lib_x64" }
			targetdir ("../../../bin_x64")
			targetname ("Robotics_UnitTests_x64")
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "_CRT_SECURE_NO_WARNINGS"	}
			flags   { "Symbols", "SEH" }
			links { "OpenThreadsd",
					"osgd", 
					"wsock32", 
					"netapi32", 
					"comctl32", 
					"wbemuuid" }
	 
		configuration { "Release_x64 or Release_Double_x64", "windows" }
			includedirs { "../../../include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../RoboticsAnimatSim",
						  "../../../../3rdParty/OpenSceneGraph-3.0.1_x64/include",
						  "../../../../3rdParty/boost_1_54_0"}	  
			libdirs { "../../../lib",
					  "$(OutDir)",
					  "../../../../3rdParty/OpenSceneGraph-3.0.1_x64/lib",
					  "../../../../3rdParty/boost_1_54_0/lib_x64" }
			targetdir ("../../../bin_x64")
			targetname ("Robotics_UnitTests_x64")
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL" }
			flags   { "Optimize", "SEH" }
			links { "OpenThreads",
					"osg",
					"wsock32",
					"netapi32",
					"comctl32",
					"wbemuuid" }

								
