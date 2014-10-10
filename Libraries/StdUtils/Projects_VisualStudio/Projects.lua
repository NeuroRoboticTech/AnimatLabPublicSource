
	project "StdUtils"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp",
				 "../*.cxx"}
		includedirs { "../../../include", 
					  "../../../../3rdParty/boost_1_54_0" }	  
	  
		configuration { "Debug or Debug_Double", "windows" }
			libdirs { "../../../lib", 
					  "../../../../3rdParty/boost_1_54_0/lib" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "STDUTILS_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdUtils_vc10D")
			postbuildcommands { "Copy $(OutDir)StdUtils_vc10D.lib ..\\..\\..\\lib\\StdUtils_vc10D.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin" }
	 
		configuration { "Release or Release_Double", "windows" }
			libdirs { "../../../lib", 
					  "../../../../3rdParty/boost_1_54_0/lib" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "STDUTILS_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdUtils_vc10")
			postbuildcommands { "Copy $(OutDir)StdUtils_vc10.lib ..\\..\\..\\lib\\StdUtils_vc10.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin" }
