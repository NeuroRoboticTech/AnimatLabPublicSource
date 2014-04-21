
	project "RoboticsAnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
			 "../*.cpp"}
		buildoptions { "-std=c++11" }
		includedirs { "../../../include", 
			      "../../StdUtils", 
		   	      "../../AnimatSim"}
		libdirs { "../../../bin" }
		links { "dl" }
	  
		configuration { "Debug or Debug_Double", "linux" }
			defines { "_DEBUG", "ROBOTICSANIMATLIBRARY_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("RoboticsAnimatSim_debug")
			links { "StdUtils_debug", 
				"AnimatSim_debug",
				"OpenThreads" }
			postbuildcommands { "cp Debug/libRoboticsAnimatSim_debug.so ../../../bin" }
	 
		configuration { "Release or Release_Double", "linux" }
			defines { "NDEBUG", "ROBOTICSANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("RoboticsAnimatSim")
			links { "StdUtils",
				"AnimatSim", 
				"OsgAnimatSim",
				"OpenThreads"}					
			postbuildcommands { "cp Release/libRoboticsAnimatSim.so ../../../bin" }


	project "Robotics_UnitTests"
		language "C++"
		kind     "ConsoleApp"
		files  { "../Robotics_UnitTests/*.h",
                 "../Robotics_UnitTests/*.cpp"}
		targetdir ("../../../bin")
		targetname ("Robotics_UnitTests")				
		includedirs { "../../../include", 
			      "../../StdUtils", 
		   	      "../../AnimatSim"}
		libdirs { "../../../bin" }
		links { "boost_system", 
			"boost_filesystem",
			"boost_unit_test_framework" }
		
		configuration { "Debug or Debug_Double", "linux" }
			defines { "_DEBUG" }
			flags   { "Symbols", "SEH" }
			links { "StdUtils_debug", 
				"AnimatSim_debug",
				"OpenThreads"}
	 
		configuration { "Release or Release_Double", "linux" }
			defines { "NDEBUG" }
			flags   { "Optimize", "SEH" }
			links { "StdUtils",
				"AnimatSim",
				"OpenThreads"}					
					

								
