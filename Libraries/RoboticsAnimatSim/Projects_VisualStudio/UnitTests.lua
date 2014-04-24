
	project "Robotics_UnitTests"
		language "C++"
		kind     "ConsoleApp"
		files  { "../Robotics_UnitTests/*.h",
				 "../Robotics_UnitTests/*.cpp"}
		
		configuration { "Debug or Debug_Double", "windows" }
			includedirs { "../../../include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../RoboticsAnimatSim",
						  "../../../../3rdParty/boost_1_54_0"}	  
			libdirs { "../../../lib",
					  "$(OutDir)",
					  "../../../../3rdParty/OpenSceneGraph-3.0.1/lib",
					  "../../../../3rdParty/boost_1_54_0/lib" }
			targetdir ("../../../bin")
			targetname ("Robotics_UnitTests")
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "_CRT_SECURE_NO_WARNINGS"	}
			flags   { "Symbols", "SEH" }
			links { "wsock32", 
					"netapi32", 
					"comctl32", 
					"wbemuuid" }
	 
		configuration { "Release or Release_Double", "windows" }
			includedirs { "../../../include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../RoboticsAnimatSim",
						  "../../../../3rdParty/boost_1_54_0"}	  
			libdirs { "../../../lib",
					  "$(OutDir)",
					  "../../../../3rdParty/OpenSceneGraph-3.0.1/lib",
					  "../../../../3rdParty/boost_1_54_0/lib" }
			targetdir ("../../../bin")
			targetname ("Robotics_UnitTests")
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL" }
			flags   { "Optimize", "SEH" }
			links { "wsock32",
					"netapi32",
					"comctl32",
					"wbemuuid" }

