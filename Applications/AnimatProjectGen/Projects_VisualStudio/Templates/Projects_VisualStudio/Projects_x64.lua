
	project "AnimatSimulator_x64"
		language "C++"
		kind     "ConsoleApp"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../include",
					  "../../../../3rdParty/stlsoft-1.9.117/include"}	  
		libdirs { "../../../lib" }
		
		configuration { "Debug_x64 or Debug_Double_x64", "windows" }
			defines { "WIN32", "_DEBUG", "_CONSOLE", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug_x64")
			targetname ("AnimatSimulatorD_x64")
			postbuildcommands { "Copy $(TargetPath) ..\\..\\..\\bin_x64" }
	 
		configuration { "Release_x64 or Release_Double_x64", "windows" }
			defines { "WIN32", "NDEBUG", "_CONSOLE", "BOOTSTRAPLOADER_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release_x64")
			targetname ("AnimatSimulator_x64")
			postbuildcommands { "Copy $(TargetPath) ..\\..\\..\\bin_x64" }
			