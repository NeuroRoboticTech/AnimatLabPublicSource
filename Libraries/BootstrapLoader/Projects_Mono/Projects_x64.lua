
	project "BootstrapLoader_x64"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include" }
		libdirs { "../../../../bin_x64" }
		links { "dl", 
			    "pthread" }
	  
		configuration { "Debug_x64 or Debug_Double_x64", "linux" }
			defines { "_DEBUG", "BOOTSTRAPLOADER_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug_x64")
			targetname ("BootstrapLoader_vc10D_x64")
			links { "StdUtils_vc10D_x64", 
					"AnimatSim_vc10D_x64"}
			postbuildcommands { "cp Debug/libBootstrapLoader_vc10D_x64.so ../../../bin_x64" }
	 
		configuration { "Release_x64 or Release_Double_x64", "linux" }
			defines { "NDEBUG", "BOOTSTRAPLOADER_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release_x64")
			targetname ("BootstrapLoader_vc10_x64")
			links { "StdUtils_vc10_x64",
					"AnimatSim_vc10_x64"}
			postbuildcommands { "cp Release/libBootstrapLoader_vc10_x64.so ../../../bin_x64" }
