
	project "AnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include", 
					  "../../../StdUtils" }
		libdirs { "../../../../bin" }
		links { "dl"}
	  
		configuration { "Debug or Debug_Double", "linux" }
			defines { "_DEBUG", "ANIMATLIBRARY_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("AnimatSim_vc10D")
			links { "StdUtils_vc10D"}
			postbuildcommands { "cp Debug/libAnimatSim_vc10D.so ../../../bin" }
	 
		configuration { "Release or Release_Double", "linux" }
			defines { "NDEBUG", "ANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("AnimatSim_vc10")
			links { "StdUtils_vc10"}
			postbuildcommands { "cp Release/libAnimatSim_vc10.so ../../../bin" }
