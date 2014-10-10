
	project "AnimatBootstrapLoader"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		buildoptions { "-std=c++0x" }
		includedirs { "../../../include" }
		libdirs { "../../../bin" }
		links { "dl", 
			    "pthread",
				"boost_system", 
				"boost_filesystem" }
	  
		configuration { "Debug or Debug_Double", "linux" }
			defines { "_DEBUG", "BOOTSTRAPLOADER_EXPORTS"	}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("AnimatBootstrapLoader_debug")
			postbuildcommands { "cp Debug/libAnimatBootstrapLoader_debug.so ../../../bin" }
	 
		configuration { "Release or Release_Double", "linux" }
			defines { "NDEBUG", "BOOTSTRAPLOADER_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("AnimatBootstrapLoader")
			postbuildcommands { "cp Release/libAnimatBootstrapLoader.so ../../../bin" }
