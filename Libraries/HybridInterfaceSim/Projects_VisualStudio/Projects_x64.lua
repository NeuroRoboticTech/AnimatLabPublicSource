
	project "HybridInterfaceSim_x64"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		
		configuration { "Debug_x64 or Debug_Double_x64", "windows" }
			includedirs { "../../../include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../../../3rdParty/boost_1_54_0",
						  "../../../../3rdParty/OpenSceneGraph-3.0.1_x64/include"}	  
			libdirs { "../../../lib",
					  "$(OutDir)", 
					  "../../../../3rdParty/boost_1_54_0/lib_x64",
					  "../../../../3rdParty/OpenSceneGraph-3.0.1_x64/lib" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "HYBRID_INTERFACE_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug_x64")
			targetname ("HybridInterfaceSim_vc10D_x64")
			links { "wsock32", 
					"netapi32", 
					"comctl32", 
					"wbemuuid",
					"OpenThreadsd", 
					"osgd"}
			postbuildcommands { "Copy $(OutDir)HybridInterfaceSim_vc10D_x64.lib ..\\..\\..\\lib\\HybridInterfaceSim_vc10D_x64.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin_x64" }
	 
		configuration { "Release_x64 or Release_Double_x64", "windows" }
			includedirs { "../../../include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../../../3rdParty/boost_1_54_0",
						  "../../../../3rdParty/OpenSceneGraph-3.0.1_x64/include"}	  
			libdirs { "../../../lib",
					  "$(OutDir)", 
					  "../../../../3rdParty/boost_1_54_0/lib_x64",
					  "../../../../3rdParty/OpenSceneGraph-3.0.1_x64/lib" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "HYBRID_INTERFACE_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release_x64")
			targetname ("HybridInterfaceSim_vc10_x64")
			links { "wsock32",
					"netapi32",
					"comctl32",
					"wbemuuid",
					"OpenThreads", 
					"osg" }
			postbuildcommands { "Copy $(OutDir)HybridInterfaceSim_vc10_x64.lib ..\\..\\..\\lib\\HybridInterfaceSim_vc10_x64.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin_x64" }
