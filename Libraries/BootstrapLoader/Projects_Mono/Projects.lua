
	project "BootstrapLoader"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include" }
		libdirs { "../../../../bin" }
		links { "dl", 
			    "pthread" }
	  
		configuration { "Debug", "linux" }
			defines { "_DEBUG", "BOOTSTRAPLOADER_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("BootstrapLoader_vc10D")
			links { "StdUtils_vc10D", 
					"AnimatSim_vc10D"}
			postbuildcommands { "cp Debug/libBootstrapLoader_vc10D.so ../../../bin",
								"cp Debug/libBootstrapLoader_vc10D.so ../../../unit_test_bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG", "BOOTSTRAPLOADER_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("BootstrapLoader_vc10")
			links { "StdUtils_vc10",
					"AnimatSim_vc10"}
			postbuildcommands { "cp Release/libBootstrapLoader_vc10.so ../../../bin", 
								"cp Release/libBootstrapLoader_vc10.so ../../../unit_test_bin" }
