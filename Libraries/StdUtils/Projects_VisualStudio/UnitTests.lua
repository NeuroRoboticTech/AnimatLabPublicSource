
	project "StdUtils_UnitTests"
		language "C++"
		kind     "ConsoleApp"
		files  { "../StdUtils_UnitTests/*.h",
				 "../StdUtils_UnitTests/*.cpp"}
		includedirs { "../../../include", 
					  "../../StdUtils", 
					  "../../../../3rdParty/boost_1_54_0" }	  
		links { "StdUtils" }
		
		configuration { "Debug or Debug_Double", "windows" }
			libdirs { "../../../lib", 
					  "../../../../3rdParty/boost_1_54_0/lib" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "_CRT_SECURE_NO_WARNINGS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdUtils_UnitTests")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\bin\\$(TargetName)$(TargetExt)" }
	 
		configuration { "Release or Release_Double", "windows" }
			libdirs { "../../../lib", 
					  "../../../../3rdParty/boost_1_54_0/lib" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdUtils_UnitTests")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\bin\\$(TargetName)$(TargetExt)" }
