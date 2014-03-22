
	project "RoboticsAnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
			 "../*.cpp"}
		includedirs { "../../../../include", 
			      "../../../StdUtils", 
		   	      "../../../AnimatSim"}
		libdirs { "../../../../bin" }
		links { "dl" }
	  
		configuration { "Debug or Debug_Double", "linux" }
			defines { "_DEBUG", "ROBOTICSANIMATLIBRARY_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("RoboticsAnimatSim_vc10D")
			links { "StdUtils_vc10D", 
				"AnimatSim_vc10D",
				"OpenThreads" }
			postbuildcommands { "cp Debug/libRoboticsAnimatSim_vc10D.so ../../../bin" }
	 
		configuration { "Release or Release_Double", "linux" }
			defines { "NDEBUG", "ROBOTICSANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("RoboticsAnimatSim_vc10")
			links { "StdUtils_vc10",
				"AnimatSim_vc10", 
				"OsgAnimatSim_vc10",
				"OpenThreads"}					
			postbuildcommands { "cp Release/libBulletAnimatSim_vc10.so ../../../bin" }


	project "Robotics_UnitTests"
		language "C++"
		kind     "ConsoleApp"
		files  { "../Robotics_UnitTests/*.h",
                 "../Robotics_UnitTests/*.cpp"}
		targetdir ("../../../../bin")
		targetname ("Robotics_UnitTests")				
		includedirs { "../include",
			      "../Libraries/StdUtils",
			      "../Libraries/AnimatSim"}	  
		libdirs { ".",
			  "../../../../bin" }
		links { "boost_system", 
			"boost_filesystem",
			"boost_unit_test_framework" }
		
		configuration { "Debug or Debug_Double", "linux" }
			defines { "_DEBUG" }
			flags   { "Symbols", "SEH" }
			links { "StdUtils_vc10D", 
				"AnimatSim_vc10D",
				"OpenThreads"}
	 
		configuration { "Release or Release_Double", "linux" }
			defines { "NDEBUG" }
			flags   { "Optimize", "SEH" }
			links { "StdUtils_vc10",
				"AnimatSim_vc10",
				"OpenThreads"}					
					

								
