
	project "AnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		buildoptions { "-std=c++11" }
		includedirs { "../../../include", 
					  "../../StdUtils" }
		libdirs { "../../../bin" }
		links { "dl"}
	  
		configuration { "Debug or Debug_Double", "linux" }
			defines { "_DEBUG", "ANIMATLIBRARY_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("AnimatSim_debug")
			links { "StdUtils_debug"}
			postbuildcommands { "cp Debug/libAnimatSim_debug.so ../../../bin" }
	 
		configuration { "Release or Release_Double", "linux" }
			defines { "NDEBUG", "ANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("AnimatSim")
			links { "StdUtils"}
			postbuildcommands { "cp Release/libAnimatSim.so ../../../bin" }
