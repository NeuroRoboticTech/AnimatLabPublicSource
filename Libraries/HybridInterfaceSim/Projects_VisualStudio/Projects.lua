
	project "HybridInterfaceSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		
		configuration { "Debug or Debug_Double", "windows" }
			includedirs { "../../../include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../../../3rdParty/boost_1_54_0",
						  "../../../../3rdParty/OpenSceneGraph-3.0.1/include"}	  
			libdirs { "../../../lib",
					  "$(OutDir)", 
					  "../../../../3rdParty/boost_1_54_0/lib",
					  "../../../../3rdParty/OpenSceneGraph-3.0.1/lib" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "HYBRID_INTERFACE_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("HybridInterfaceSim_vc10D")
			links { "wsock32", 
					"netapi32", 
					"comctl32", 
					"wbemuuid",
					"OpenThreadsd", 
					"osgd" }
			postbuildcommands { "Copy $(OutDir)HybridInterfaceSim_vc10D.lib ..\\..\\..\\lib\\HybridInterfaceSim_vc10D.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin" }
	 
		configuration { "Release or Release_Double", "windows" }
			includedirs { "../../../include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../../../3rdParty/boost_1_54_0",
						  "../../../../3rdParty/OpenSceneGraph-3.0.1/include"}	  
			libdirs { "../../../lib",
					  "$(OutDir)", 
					  "../../../../3rdParty/boost_1_54_0/lib",
					  "../../../../3rdParty/OpenSceneGraph-3.0.1/lib" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "HYBRID_INTERFACE_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("HybridInterfaceSim_vc10")
			links { "wsock32",
					"netapi32",
					"comctl32",
					"wbemuuid",
					"OpenThreads", 
					"osg" }
			postbuildcommands { "Copy $(OutDir)HybridInterfaceSim_vc10.lib ..\\..\\..\\lib\\HybridInterfaceSim_vc10.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin" }
