
	project "BootstrapLoader"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../include"}	  
		libdirs { "../../../lib" }
		
		configuration { "Debug", "windows" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "BOOTSTRAPLOADER_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("BootstrapLoader_vc10D")
			postbuildcommands { "Copy $(OutDir)BootstrapLoader_vc10D.lib ..\\..\\..\\lib\\BootstrapLoader_vc10D.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin",
								"Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }
	 
		configuration { "Release", "windows" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "BOOTSTRAPLOADER_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("BootstrapLoader_vc10")
			postbuildcommands { "Copy $(OutDir)BootstrapLoader_vc10.lib ..\\..\\..\\lib\\BootstrapLoader_vc10.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin", 
								"Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }
