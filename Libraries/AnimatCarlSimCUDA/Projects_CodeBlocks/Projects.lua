
	project "AnimatCarlSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		buildoptions { "-std=c++0x" }
		includedirs { "../../../include", 
					  "../../StdUtils", 
					  "../../AnimatSim" }
		libdirs { "../../../bin" }
		links { "dl"}
	  
		configuration { "Debug or Debug_Double", "linux" }
			defines { "_DEBUG", "ANIMATCARLSIM_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("AnimatCarlSim_debug")
			links { "StdUtils_debug", 
					"AnimatSim_debug"}
			postbuildcommands { "cp Debug/libAnimatCarlSim_debug.so ../../../bin" }
	 
		configuration { "Release or Release_Double", "linux" }
			defines { "NDEBUG", "ANIMATCARLSIM_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("AnimatCarlSim")
			links { "StdUtils",
					"AnimatSim"}
			postbuildcommands { "cp Release/libAnimatCarlSim.so ../../../bin" }
