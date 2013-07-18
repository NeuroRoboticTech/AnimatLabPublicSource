
	project "StdUtils"
		language "C++"
		kind     "SharedLib"
		files  { "../MarkupSTL.h",
				 "../StdAfx.h",
				 "../StdClassFactory.h",
				 "../StdColor.h",
				 "../StdCriticalSection.h",
				 "../StdErrorInfo.h",
				 "../StdFixed.h",
				 "../StdFont.h",
				 "../StdIncludes.h",
				 "../StdLookupTable.h",
				 "../StdPostFixEval.h",
				 "../StdSerialize.h",
				 "../StdTimer.h",
				 "../StdUtilFunctions.h",
				 "../StdVariable.h",
				 "../StdVariant.h",
				 "../StdXml.h",
				 "../XYTrace.h",
				 "../MarkupSTL.cpp",
				 "../MersenneTwister.cpp",
				 "../StdAfx.cpp",
				 "../StdClassFactory.cpp",
				 "../StdColor.cpp",
				 "../StdCriticalSection.cpp",
				 "../StdErrorInfo.cpp",
				 "../StdFixed.cpp",
				 "../StdFont.cpp",
				 "../StdLookupTable.cpp",
				 "../StdPostFixEval.cpp",
				 "../StdSerialize.cpp",
				 "../StdTimer.cpp",
				 "../StdUtilFunctions.cpp",
				 "../StdUtils.cpp",
				 "../StdVariable.cpp",
				 "../StdVariant.cpp",
				 "../StdXml.cpp",
				 "../XYTrace.cpp" }
		includedirs { "../../../include", 
					  "$(BOOST_ROOT)" }	  
		libdirs { "../../../lib", 
				  "$(BOOST_ROOT)/lib" }
	  
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
					  "../../StdUtils", 
					  "$(BOOST_ROOT)" }	  
		libdirs { "../../../lib", 
				  "$(BOOST_ROOT)/lib" }
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
					  "$(BOOST_ROOT)" }	  
		libdirs { "../../../lib", 
				  "$(BOOST_ROOT)/lib" }
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
