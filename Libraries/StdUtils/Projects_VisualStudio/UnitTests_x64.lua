
	project "StdUtils_UnitTests_x64"
		language "C++"
		kind     "ConsoleApp"
		files  { "../StdUtils_UnitTests/*.h",
				 "../StdUtils_UnitTests/*.cpp"}
		includedirs { "../../../include", 
					  "../../StdUtils", 
					  "../../../../3rdParty/boost_1_54_0" }	  
		links { "StdUtils_x64" }

		configuration { "Debug_x64 or Debug_Double_x64", "windows" }
			libdirs { "../../../lib", 
					  "../../../../3rdParty/boost_1_54_0/lib_x64" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "_CRT_SECURE_NO_WARNINGS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug_x64")
			targetname ("StdUtils_UnitTests_x64")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\binx64\\$(TargetName)$(TargetExt)" }
	 
		configuration { "Release_x64 or Release_Double_x64", "windows" }
			libdirs { "../../../lib", 
					  "../../../../3rdParty/boost_1_54_0/lib_x64" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release_x64")
			targetname ("StdUtils_UnitTests_x64")
			postbuildcommands { "Copy $(TargetPATH) ..\\..\\..\\binx64\\$(TargetName)$(TargetExt)" }
