
	project "StdUtils"
		language "C++"
		kind     "SharedLib"
		files  { "../MarkupSTL.h",
				 "../StdAfx.h",
				 "../StdClassFactory.h",
				 "../StdColor.h",
				 "../StdErrorInfo.h",
				 "../StdFixed.h",
				 "../StdFont.h",
				 "../StdIncludes.h",
				 "../StdLookupTable.h",
				 "../StdPostFixEval.h",
				 "../StdSerialize.h",
				 "../StdUtilFunctions.h",
				 "../StdVariable.h",
				 "../StdVariant.h",
				 "../StdXml.h",
				 "../MarkupSTL.cpp",
				 "../MersenneTwister.cpp",
				 "../StdAfx.cpp",
				 "../StdClassFactory.cpp",
				 "../StdColor.cpp",
				 "../StdErrorInfo.cpp",
				 "../StdFixed.cpp",
				 "../StdFont.cpp",
				 "../StdLookupTable.cpp",
				 "../StdPostFixEval.cpp",
				 "../StdSerialize.cpp",
				 "../StdUtilFunctions.cpp",
				 "../StdUtils.cpp",
				 "../StdVariable.cpp",
				 "../StdVariant.cpp",
				 "../StdXml.cpp" }
		includedirs { "../../../../include" }	  
		libdirs { "../../../../lib" }
	  
		configuration { "Debug", "linux" }
			defines { "_DEBUG", "STDUTILS_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdUtilsD")
			postbuildcommands { "cp Debug/libStdUtilsD.so ../../../bin",
								"cp Debug/libStdUtilsD.so ../../../unit_test_bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG", "STDUTILS_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdUtils")
			postbuildcommands { "cp libStdUtils.so ../../../bin", 
								"cp libStdUtils.so ../../../unit_test_bin" }

	project "StdClassFactoryTester"
		language "C++"
		kind     "SharedLib"
		files  { "../StdClassFactoryTester/*.h",
				 "../StdClassFactoryTester/*.cpp"}
		includedirs { "../../../../include", 
					  "../../../StdUtils" }	  
		libdirs { "../../../../lib" }
		links { "StdUtils" }
		
		configuration { "Debug", "linux" }
			defines { "_DEBUG", "STDCLASSFACTORYTESTER_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdClassFactoryTester")
			postbuildcommands { "cp libStdClassFactoryTester.so ../../../unit_test_bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG", "STDCLASSFACTORYTESTER_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdClassFactoryTester")
			postbuildcommands { "cp libStdClassFactoryTester.so ../../../unit_test_bin" }

	project "StdUtils_UnitTests"
		language "C++"
		kind     "ConsoleApp"
		files  { "../StdUtils_UnitTests/*.h",
				 "../StdUtils_UnitTests/*.cpp"}
		includedirs { "../../../../include", 
					  "../../../StdUtils", 
					  "$(BOOST_ROOT)" }	  
		libdirs { "../../../../lib", 
				  "$(BOOST_ROOT)/lib" }
		links { "StdUtils" }
		
		configuration { "Debug", "linux" }
			defines { "_DEBUG"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdUtils_UnitTests")
			postbuildcommands { "Copy StdUtils_UnitTests.exe ../../../unit_test_bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdUtils_UnitTests")
			postbuildcommands { "Copy StdUtils_UnitTests.exe ../../../unit_test_bin" }
						