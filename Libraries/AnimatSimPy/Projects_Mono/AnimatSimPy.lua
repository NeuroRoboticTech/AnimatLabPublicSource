
-- A solution contains projects, and defines the available configurations
solution "AnimatSimPy"
	configurations { "Debug", "Release" }

	project "AnimatSimPy"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp",
				 "../*.cxx"}
		includedirs { "../../../../include", 
					  "../../../StdUtils",
					  "../../../AnimatSim",
					  "../../../FiringRateSim" }
		libdirs { "../../../../bin" }
		links { "dl", 
				"boost_thread",
				"boost_chrono"}
	  
		configuration { "Debug", "linux" }
			defines { "_DEBUG"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("AnimatSimPy_d")
			links { "StdUtils_debug", 
					"AnimatSim_debug", 
					"FiringRateSim_debug",
					"python27_d" }
			postbuildcommands { "cp Debug/libAnimatSimPy_d.so ../../../bin",
							    "cp ../AnimatSimPy.py ../../../bin/AnimatSimPy.py" }
	  
		configuration { "Release", "linux" }
			defines { "NDEBUG" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("AnimatSimPy")
			links { "StdUtils", 
					"AnimatSim", 
					"FiringRateSim",
					"python27" }
			postbuildcommands { "cp Release/libAnimatSimPy.so ../../../bin",
							    "cp ../AnimatSimPy.py ../../../bin/PyAnimatSimPy.py" }
								
								