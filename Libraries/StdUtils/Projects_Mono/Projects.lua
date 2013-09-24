
	project "StdUtils"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include" }
		links { "dl", 
				"boost_system", 
				"boost_filesystem",
				"boost_unit_test_framework" }
	  
		configuration { "Debug", "linux" }
			defines { "_DEBUG", "STDUTILS_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdUtils_vc10D")
			postbuildcommands { "cp Debug/libStdUtils_vc10D.so ../../../bin",
								"cp Debug/libStdUtils_vc10D.so ../../../unit_test_bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG", "STDUTILS_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdUtils_vc10")
			postbuildcommands { "cp Release/libStdUtils_vc10.so ../../../bin", 
								"cp Release/libStdUtils_vc10.so ../../../unit_test_bin" }

	project "StdClassFactoryTester"
		language "C++"
		kind     "SharedLib"
		files  { "../StdClassFactoryTester/*.h",
				 "../StdClassFactoryTester/*.cpp"}
		includedirs { "../../../../include", 
					  "../../../StdUtils" }	  
		links {"dl"}
		
		configuration { "Debug", "linux" }
			defines { "_DEBUG", "STDCLASSFACTORYTESTER_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdClassFactoryTester")
			postbuildcommands { "cp Debug/libStdClassFactoryTester.so ../../../unit_test_bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG", "STDCLASSFACTORYTESTER_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdClassFactoryTester")
			postbuildcommands { "cp Release/libStdClassFactoryTester.so ../../../unit_test_bin" }

	project "StdUtils_UnitTests"
		language "C++"
		kind     "ConsoleApp"
		files  { "../StdUtils_UnitTests/*.h",
				 "../StdUtils_UnitTests/*.cpp"}
		includedirs { "../../../../include", 
			      "../../../StdUtils" }	  
		libdirs { "../../../../bin" }
		links { "boost_system", 
				"boost_filesystem",
				"boost_unit_test_framework" }
		
		configuration { "Debug", "linux" }
			defines { "_DEBUG"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdUtils_UnitTests")
			links { "StdUtils_vc10D" }
			postbuildcommands { "cp Debug/StdUtils_UnitTests ../../../unit_test_bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdUtils_UnitTests")
			links { "StdUtils_vc10" }
			postbuildcommands { "cp Release/StdUtils_UnitTests ../../../unit_test_bin" }
						
