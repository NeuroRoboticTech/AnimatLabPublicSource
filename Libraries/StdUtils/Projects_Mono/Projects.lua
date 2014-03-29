
	project "StdUtils"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include" }
		links { "dl",
				"pthread",
				"boost_system", 
				"boost_filesystem",
				"boost_unit_test_framework",
				"boost_thread-mt" }
	  
		configuration { "Debug or Debug_Double", "linux" }
			defines { "_DEBUG", "STDUTILS_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdUtils_debug")
			postbuildcommands { "cp Debug/libStdUtils_debug.so ../../../bin" }
	 
		configuration { "Release or Release_Double", "linux" }
			defines { "NDEBUG", "STDUTILS_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdUtils")
			postbuildcommands { "cp Release/libStdUtils.so ../../../bin" }

	project "StdClassFactoryTester"
		language "C++"
		kind     "SharedLib"
		files  { "../StdClassFactoryTester/*.h",
				 "../StdClassFactoryTester/*.cpp"}
		includedirs { "../../../../include", 
					  "../../../StdUtils" }	  
		links {"dl",
  		       "pthread" }
		
		configuration { "Debug or Debug_Double", "linux" }
			defines { "_DEBUG", "STDCLASSFACTORYTESTER_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdClassFactoryTester")
			postbuildcommands { "cp Debug/libStdClassFactoryTester.so ../../../bin" }
	 
		configuration { "Release or Release_Double", "linux" }
			defines { "NDEBUG", "STDCLASSFACTORYTESTER_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdClassFactoryTester")
			postbuildcommands { "cp Release/libStdClassFactoryTester.so ../../../bin" }

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
				"boost_unit_test_framework",
				"boost_thread-mt" }
		
		configuration { "Debug or Debug_Double", "linux" }
			defines { "_DEBUG"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdUtils_UnitTests")
			links { "StdUtils_debug" }
			postbuildcommands { "cp Debug/StdUtils_UnitTests ../../../bin" }
	 
		configuration { "Release or Release_Double", "linux" }
			defines { "NDEBUG" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdUtils_UnitTests")
			links { "StdUtils" }
			postbuildcommands { "cp Release/StdUtils_UnitTests ../../../bin" }
						
