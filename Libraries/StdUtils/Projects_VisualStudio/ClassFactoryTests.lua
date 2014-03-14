	
	project "StdClassFactoryTester"
		language "C++"
		kind     "SharedLib"
		files  { "../StdClassFactoryTester/*.h",
				 "../StdClassFactoryTester/*.cpp"}
		includedirs { "../../../include", 
					  "../../StdUtils" }	  
		libdirs { "../../../lib" }
		links { "StdUtils" }
	  
		configuration { "Debug or Debug_Double", "windows" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "STDCLASSFACTORYTESTER_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdClassFactoryTester")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\bin\\$(TargetName)$(TargetExt)" }
	 
		configuration { "Release or Release_Double", "windows" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "STDCLASSFACTORYTESTER_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdClassFactoryTester")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\bin\\$(TargetName)$(TargetExt)" }
