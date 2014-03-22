
	project "IntegrateFireSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include", 
					  "../../../StdUtils", 
					  "../../../AnimatSim" }
		libdirs { "../../../../bin" }
		links { "dl"}
	  
		configuration { "Debug or Debug_Double", "linux" }
			defines { "_DEBUG", "REALISTICNEURALNET_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("IntegrateFireSim_vc10D")
			links { "StdUtils_vc10D", 
					"AnimatSim_vc10D"}
			postbuildcommands { "cp Debug/libIntegrateFireSim_vc10D.so ../../../bin" }
	 
		configuration { "Release or Release_Double", "linux" }
			defines { "NDEBUG", "REALISTICNEURALNET_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("IntegrateFireSim_vc10")
			links { "StdUtils_vc10",
					"AnimatSim_vc10"}
			postbuildcommands { "cp Release/libIntegrateFireSim_vc10.so ../../../bin" }
