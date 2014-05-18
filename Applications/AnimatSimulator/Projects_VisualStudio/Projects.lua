
	project "AnimatSimulator"
		language "C++"
		kind     "ConsoleApp"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../include",
					  "../../../../3rdParty/stlsoft-1.9.117/include"}	  
		libdirs { "../../../lib" }
		
		configuration { "Debug or Debug_Double", "windows" }
			defines { "WIN32", "_DEBUG", "_CONSOLE", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("AnimatSimulatorD")
			postbuildcommands { "Copy $(TargetPath) ..\\..\\..\\bin" }
	 
		configuration { "Release or Release_Double", "windows" }
			defines { "WIN32", "NDEBUG", "_CONSOLE", "BOOTSTRAPLOADER_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("AnimatSimulator")
			postbuildcommands { "Copy $(TargetPath) ..\\..\\..\\bin" }
