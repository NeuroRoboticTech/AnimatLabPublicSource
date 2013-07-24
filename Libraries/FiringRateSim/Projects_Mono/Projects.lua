
	project "FiringRateSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include", 
					  "../../../StdUtils", 
					  "../../../AnimatSim" }
		libdirs { "../../../../bin" }
		links { "dl"}
	  
		configuration { "Debug", "linux" }
			defines { "_DEBUG", "FASTNEURALNET_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("FiringRateSim_vc10D")
			links { "StdUtils_vc10D", 
					"AnimatSim_vc10D"}
			postbuildcommands { "cp Debug/libFiringRateSim_vc10D.so ../../../bin",
								"cp Debug/libFiringRateSim_vc10D.so ../../../unit_test_bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG", "FASTNEURALNET_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("FiringRateSim_vc10")
			links { "StdUtils_vc10",
					"AnimatSim_vc10"}
			postbuildcommands { "cp Release/libFiringRateSim_vc10.so ../../../bin", 
								"cp Release/libFiringRateSim_vc10.so ../../../unit_test_bin" }
