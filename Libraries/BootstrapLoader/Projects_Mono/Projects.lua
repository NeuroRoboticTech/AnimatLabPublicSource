
	project "BootstrapLoader"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include" }
		libdirs { "../../../../bin" }
		links { "dl", 
			    "pthread" }
	  
		configuration { "Debug or Debug_Double", "linux" }
			defines { "_DEBUG", "BOOTSTRAPLOADER_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("BootstrapLoader_debug")
			postbuildcommands { "cp Debug/libBootstrapLoader_debug.so ../../../bin" }
	 
		configuration { "Release or Release_Double", "linux" }
			defines { "NDEBUG", "BOOTSTRAPLOADER_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("BootstrapLoader")
			postbuildcommands { "cp Release/libBootstrapLoader.so ../../../bin" }
