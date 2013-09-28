
	project "AnimatSimulator"
		language "C++"
		kind     "ConsoleApp"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include" }
		libdirs { "../../../../bin" }
	  
		configuration { "Debug", "linux" }
			defines { "_DEBUG"}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("AnimatSimulatorD")
			links { "BootstrapLoader_vc10D", "dl", "pthread" }
			postbuildcommands { "cp Debug/AnimatSimulatorD ../../../bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("AnimatSimulator")
			links { "BootstrapLoader_vc10", "dl", "pthread" }
			postbuildcommands { "cp Release/AnimatSimulator ../../../bin" }
