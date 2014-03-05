
	project "StdUtils"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../include", 
					  "../../../../3rdParty/boost_1_54_0" }	  
	  
		configuration { "Debug", "windows" }
			libdirs { "../../../lib", 
					  "../../../../3rdParty/boost_1_54_0/lib" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "STDUTILS_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdUtils_vc10D")
			postbuildcommands { "Copy $(OutDir)StdUtils_vc10D.lib ..\\..\\..\\lib\\StdUtils_vc10D.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin",
								"Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }
	 
		configuration { "Release", "windows" }
			libdirs { "../../../lib", 
					  "../../../../3rdParty/boost_1_54_0/lib" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "STDUTILS_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdUtils_vc10")
			postbuildcommands { "Copy $(OutDir)StdUtils_vc10.lib ..\\..\\..\\lib\\StdUtils_vc10.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin", 
								"Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }

		configuration { "Debug_x64", "windows" }
			libdirs { "../../../lib", 
					  "../../../../3rdParty/boost_1_54_0/lib_x64" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "STDUTILS_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug_x64")
			targetname ("StdUtils_vc10D_x64")
			postbuildcommands { "Copy $(OutDir)StdUtils_vc10D_x64.lib ..\\..\\..\\lib\\StdUtils_vc10D_x64.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin_x64",
								"Copy $(TargetPATH) ..\\..\\..\\unit_test_bin_x64\\$(TargetName)$(TargetExt)" }
	 
		configuration { "Release_x64", "windows" }
			libdirs { "../../../lib", 
					  "../../../../3rdParty/boost_1_54_0/lib_x64" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "STDUTILS_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release_x64")
			targetname ("StdUtils_vc10_x64")
			postbuildcommands { "Copy $(OutDir)StdUtils_vc10_x64.lib ..\\..\\..\\lib\\StdUtils_vc10_x64.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\binx64", 
								"Copy $(TargetPATH) ..\\..\\..\\unit_test_binx64\\$(TargetName)$(TargetExt)" }
								
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

		configuration { "Debug_x64", "windows" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "STDCLASSFACTORYTESTER_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdClassFactoryTester_x64")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\unit_test_binx64\\$(TargetName)$(TargetExt)" }
	 
		configuration { "Release_x64", "windows" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "STDCLASSFACTORYTESTER_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdClassFactoryTester_x64")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\unit_test_binx64\\$(TargetName)$(TargetExt)" }
			
	project "StdUtils_UnitTests"
		language "C++"
		kind     "ConsoleApp"
		files  { "../StdUtils_UnitTests/*.h",
				 "../StdUtils_UnitTests/*.cpp"}
		includedirs { "../../../include", 
					  "../../StdUtils", 
					  "../../../../3rdParty/boost_1_54_0" }	  
		links { "StdUtils" }
		
		configuration { "Debug", "windows" }
			libdirs { "../../../lib", 
					  "../../../../3rdParty/boost_1_54_0/lib" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "_CRT_SECURE_NO_WARNINGS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdUtils_UnitTests")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }
	 
		configuration { "Release", "windows" }
			libdirs { "../../../lib", 
					  "../../../../3rdParty/boost_1_54_0/lib" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdUtils_UnitTests")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\unit_test_bin\\$(TargetName)$(TargetExt)" }

		configuration { "Debug_x64", "windows" }
			libdirs { "../../../lib", 
					  "../../../../3rdParty/boost_1_54_0/lib_x64" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "_CRT_SECURE_NO_WARNINGS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdUtils_UnitTests_x64")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\unit_test_binx64\\$(TargetName)$(TargetExt)" }
	 
		configuration { "Release_x64", "windows" }
			libdirs { "../../../lib", 
					  "../../../../3rdParty/boost_1_54_0/lib_x64" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdUtils_UnitTests_x64")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\unit_test_binx64\\$(TargetName)$(TargetExt)" }
