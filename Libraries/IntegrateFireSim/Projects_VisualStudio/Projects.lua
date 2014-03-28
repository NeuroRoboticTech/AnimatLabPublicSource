
	project "IntegrateFireSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../include", 
					  "../../StdUtils",
					  "../../AnimatSim",
					  "../../../../3rdParty/boost_1_54_0"}	  
			libdirs { "../../../lib", 
					  "../../../../3rdParty/boost_1_54_0/lib" }
		
		configuration { "Debug or Debug_Double", "windows" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "REALISTICNEURALNET_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("IntegrateFireSim_vc10D")
			postbuildcommands { "Copy $(OutDir)IntegrateFireSim_vc10D.lib ..\\..\\..\\lib\\IntegrateFireSim_vc10D.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin" }
	 
		configuration { "Release or Release_Double", "windows" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "REALISTICNEURALNET_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("IntegrateFireSim_vc10")
			postbuildcommands { "Copy $(OutDir)IntegrateFireSim_vc10.lib ..\\..\\..\\lib\\IntegrateFireSim_vc10.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin" }
