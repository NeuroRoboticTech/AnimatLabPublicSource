
	project "AnimatCarlSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp",
				 "../*.cu"}
		buildoptions { "-std=c++0x" }
		includedirs { "../../../include", 
					  "../../StdUtils", 
					  "../../AnimatSim",
					  "../../../../3rdParty/CARLsim/src" }
		libdirs { "../../../bin",
				  "../../../../3rdParty/CARLsim/bin" }
		links { "dl", "cudart" }
	  
		configuration { "Debug", "linux" }
			defines { "_DEBUG", "ANIMATCARLSIM_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("AnimatCarlSim_debug")
			links { "StdUtils_debug", 
					"AnimatSim_debug", 
					"CarlSim_debug"}
			postbuildcommands { "cp Debug/libAnimatCarlSim_debug.so ../../../bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG", "ANIMATCARLSIM_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("AnimatCarlSim")
			links { "StdUtils",
					"AnimatSim", 
					"CarlSim"}
			postbuildcommands { "cp Release/libAnimatCarlSim.so ../../../bin" }
