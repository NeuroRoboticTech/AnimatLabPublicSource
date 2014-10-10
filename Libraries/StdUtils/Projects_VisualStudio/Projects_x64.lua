
	project "StdUtils_x64"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../include", 
					  "../../../../3rdParty/boost_1_54_0" }	  
	  
		configuration { "Debug_x64 or Debug_Double_x64", "windows" }
			libdirs { "../../../lib", 
					  "../../../../3rdParty/boost_1_54_0/lib_x64" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "STDUTILS_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug_x64")
			targetname ("StdUtils_vc10D_x64")
			postbuildcommands { "Copy $(OutDir)StdUtils_vc10D_x64.lib ..\\..\\..\\lib\\StdUtils_vc10D_x64.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin_x64" }
	 
		configuration { "Release_x64 or Release_Double_x64", "windows" }
			libdirs { "../../../lib", 
					  "../../../../3rdParty/boost_1_54_0/lib_x64" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "STDUTILS_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release_x64")
			targetname ("StdUtils_vc10_x64")
			postbuildcommands { "Copy $(OutDir)StdUtils_vc10_x64.lib ..\\..\\..\\lib\\StdUtils_vc10_x64.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin_x64" }
