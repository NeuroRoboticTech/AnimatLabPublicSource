
	project "IntegrateFireSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		buildoptions { "-std=c++11" }
		includedirs { "../../../include", 
					  "../../StdUtils", 
					  "../../AnimatSim",
					  "../../../../3rdParty/stlsoft-1.9.117/include" }
		libdirs { "../../../bin" }
		links { "dl"}
	  
		configuration { "Debug or Debug_Double", "linux" }
			defines { "_DEBUG", "REALISTICNEURALNET_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("IntegrateFireSim_debug")
			links { "StdUtils_debug", 
					"AnimatSim_debug"}
			postbuildcommands { "cp Debug/libIntegrateFireSim_debug.so ../../../bin" }
	 
		configuration { "Release or Release_Double", "linux" }
			defines { "NDEBUG", "REALISTICNEURALNET_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("IntegrateFireSim")
			links { "StdUtils",
					"AnimatSim"}
			postbuildcommands { "cp Release/libIntegrateFireSim.so ../../../bin" }
