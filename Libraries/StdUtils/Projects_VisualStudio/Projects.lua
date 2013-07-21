
	project "StdUtils"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../include", 
					  "../../../../3rdParty/boost_1_54_0" }	  
		libdirs { "../../../lib", 
				  "../../../../3rdParty/boost_1_54_0/lib" }
	  
		configuration { "Debug", "windows" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "STDUTILS_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdUtils_vc10D")
			postbuildcommands { "Copy $(OutDir)StdUtils_vc10D.lib ..\\..\\..\\lib\\StdUtils_vc10D.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin",
								"Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }
	 
		configuration { "Release", "windows" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "STDUTILS_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdUtils_vc10")
			postbuildcommands { "Copy $(OutDir)StdUtils_vc10.lib ..\\..\\..\\lib\\StdUtils_vc10.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin", 
								"Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }

	project "StdClassFactoryTester"
		language "C++"
		kind     "SharedLib"
		files  { "../StdClassFactoryTester/*.h",
				 "../StdClassFactoryTester/*.cpp"}
		includedirs { "../../../include", 
					  "../../StdUtils" }	  
		libdirs { "../../../lib" }
		links { "StdUtils" }
	  
		configuration { "Debug", "windows" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "STDCLASSFACTORYTESTER_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdClassFactoryTester")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }
	 
		configuration { "Release", "windows" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "STDCLASSFACTORYTESTER_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdClassFactoryTester")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }

	project "StdUtils_UnitTests"
		language "C++"
		kind     "ConsoleApp"
		files  { "../StdUtils_UnitTests/*.h",
				 "../StdUtils_UnitTests/*.cpp"}
		includedirs { "../../../include", 
					  "../../StdUtils", 
					  "../../../../3rdParty/boost_1_54_0" }	  
		libdirs { "../../../lib", 
				  "../../../../3rdParty/boost_1_54_0/lib" }
		links { "StdUtils" }
		
		configuration { "Debug", "windows" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "_CRT_SECURE_NO_WARNINGS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdUtils_UnitTests")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }
	 
		configuration { "Release", "windows" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdUtils_UnitTests")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }
