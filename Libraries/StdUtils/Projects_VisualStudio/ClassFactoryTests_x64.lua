	
	project "StdClassFactoryTester_x64"
		language "C++"
		kind     "SharedLib"
		files  { "../StdClassFactoryTester/*.h",
				 "../StdClassFactoryTester/*.cpp"}
		includedirs { "../../../include", 
					  "../../StdUtils" }	  
		libdirs { "../../../lib" }
		links { "StdUtils_x64" }
	  
		configuration { "Debug_x64 or Debug_Double_x64", "windows" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "STDCLASSFACTORYTESTER_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug_x64")
			targetname ("StdClassFactoryTester_x64")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\binx64\\$(TargetName)$(TargetExt)" }
	 
		configuration { "Release_x64 or Release_Double_x64", "windows" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "STDCLASSFACTORYTESTER_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release_x64")
			targetname ("StdClassFactoryTester_x64")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\binx64\\$(TargetName)$(TargetExt)" }
	