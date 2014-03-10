
	project "AnimatSimulator"
		language "C++"
		kind     "ConsoleApp"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../include"}	  
		libdirs { "../../../lib" }
		
		configuration { "Debug", "windows" }
			defines { "WIN32", "_DEBUG", "_CONSOLE", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("AnimatSimulatorD")
			postbuildcommands { "Copy $(TargetPath) ..\\..\\..\\bin" }
	 
		configuration { "Release", "windows" }
			defines { "WIN32", "NDEBUG", "_CONSOLE", "BOOTSTRAPLOADER_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("AnimatSimulator")
			postbuildcommands { "Copy $(TargetPath) ..\\..\\..\\bin" }

		configuration { "Debug_x64", "windows" }
			defines { "WIN32", "_DEBUG", "_CONSOLE", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug_x64")
			targetname ("AnimatSimulatorD_x64")
			postbuildcommands { "Copy $(TargetPath) ..\\..\\..\\bin_x64" }
	 
		configuration { "Release_x64", "windows" }
			defines { "WIN32", "NDEBUG", "_CONSOLE", "BOOTSTRAPLOADER_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release_x64")
			targetname ("AnimatSimulator_x64")
			postbuildcommands { "Copy $(TargetPath) ..\\..\\..\\bin_x64" }
			