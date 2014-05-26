
-- A solution contains projects, and defines the available configurations
solution "AnimatSimPy"
	configurations { "Debug", "Release" }

	project "AnimatSimPy"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include", 
					  "../../../StdUtils",
					  "../../../AnimatSim" }
		libdirs { "../../../../bin" }
		links { "dl", 
				"boost_thread" }
	  
		configuration { "Release", "linux" }
			defines { "NDEBUG" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("PyAnimatSim")
			links { "StdUtils", "AnimatSim"}
			postbuildcommands { "cp Release/libAnimatSim.so ../../../bin/_PyAnimatSim.pyd",
							    "cp ../PyAnimatSim.py ../../../bin/PyAnimatSim.py" }