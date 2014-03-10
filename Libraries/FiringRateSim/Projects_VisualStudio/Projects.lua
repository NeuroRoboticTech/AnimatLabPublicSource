
	project "FiringRateSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../include", 
					  "../../StdUtils",
					  "../../AnimatSim"}	  
		libdirs { "../../../lib" }
		
		configuration { "Debug", "windows" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "FASTNEURALNET_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("FiringRateSim_vc10D")
			postbuildcommands { "Copy $(OutDir)FiringRateSim_vc10D.lib ..\\..\\..\\lib\\FiringRateSim_vc10D.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin",
								"Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }
	 
		configuration { "Release", "windows" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "FASTNEURALNET_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("FiringRateSim_vc10")
			postbuildcommands { "Copy $(OutDir)FiringRateSim_vc10.lib ..\\..\\..\\lib\\FiringRateSim_vc10.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin", 
								"Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }
		
		configuration { "Debug_x64", "windows" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "FASTNEURALNET_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug_x64")
			targetname ("FiringRateSim_vc10D_x64")
			postbuildcommands { "Copy $(OutDir)FiringRateSim_vc10D_x64.lib ..\\..\\..\\lib\\FiringRateSim_vc10D_x64.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin_x64",
								"Copy $(TargetPATH) ..\\..\\..\\unit_test_bin_x64\\$(TargetName)$(TargetExt)" }
	 
		configuration { "Release_x64", "windows" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "FASTNEURALNET_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release_x64")
			targetname ("FiringRateSim_vc10_x64")
			postbuildcommands { "Copy $(OutDir)FiringRateSim_vc10_x64.lib ..\\..\\..\\lib\\FiringRateSim_vc10_x64.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin_x64", 
								"Copy $(TargetPATH) ..\\..\\..\\unit_test_bin_x64\\$(TargetName)$(TargetExt)" }
