
	project "AnimatCarlSimCUDA"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp",
				 "../*.cu"}
		includedirs { "../../../include", 
					  "../../StdUtils",
					  "../../AnimatSim",
					  "../../../../3rdParty/boost_1_54_0",
					  "$(NVCUDASAMPLES_ROOT)/common/inc",
					  "../../../../3rdParty/Carlsim/src"}	  
		libdirs { "../../../lib", 
				  "../../../../3rdParty/boost_1_54_0/lib",
				  "../../../../3rdParty/CARLsim/lib",
				  "$(CudaToolkitLibDir)"}
		links { "cudart" } 
				  
		configuration { "Debug or Debug_Double", "windows" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "ANIMATCARLSIMCUDA_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("AnimatCarlSimCUDA_vc10D")
			links { "CARLsim_debug" } 
			postbuildcommands { "Copy $(OutDir)AnimatCarlSim_vc10D.lib ..\\..\\..\\lib\\AnimatCarlSim_vc10D.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin" }
	 
		configuration { "Release or Release_Double", "windows" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "ANIMATCARLSIMCUDA_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("AnimatCarlSimCUDA_vc10")
			links { "CARLsim" } 
			postbuildcommands { "Copy $(OutDir)AnimatCarlSim_vc10.lib ..\\..\\..\\lib\\AnimatCarlSim_vc10.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin" }
