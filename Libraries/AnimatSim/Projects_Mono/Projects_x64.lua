
	project "AnimatSim_x64"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include", 
					  "../../../StdUtils" }
		libdirs { "../../../../bin_x64" }
		links { "dl"}
	  
		configuration { "Debug_x64 or Debug_Double_x64", "linux" }
			defines { "_DEBUG", "ANIMATLIBRARY_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug_x64")
			targetname ("AnimatSim_vc10D_x64")
			links { "StdUtils_vc10D_x64"}
			postbuildcommands { "cp Debug/libAnimatSim_vc10D_x64.so ../../../bin_x64" }
	 
		configuration { "Release_x64 or Release_Double_x64", "linux" }
			defines { "NDEBUG", "ANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release_x64")
			targetname ("AnimatSim_vc10_x64")
			links { "StdUtils_vc10_x64"}
			postbuildcommands { "cp Release/libAnimatSim_vc10_x64.so ../../../bin_x64" }
