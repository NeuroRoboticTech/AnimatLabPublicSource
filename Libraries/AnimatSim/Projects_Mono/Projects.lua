
	project "AnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include", 
					  "../../../StdIncludes" }
		links {"dl"}
	  
		configuration { "Debug", "linux" }
			defines { "_DEBUG", "ANIMATLIBRARY_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("AnimatSim_vc10D")
			postbuildcommands { "cp Debug/libAnimatSimD.so ../../../bin",
								"cp Debug/libAnimatSimD.so ../../../unit_test_bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG", "ANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("AnimatSim_vc10")
			postbuildcommands { "cp libAnimatSim.so ../../../bin", 
								"cp libAnimatSim.so ../../../unit_test_bin" }
