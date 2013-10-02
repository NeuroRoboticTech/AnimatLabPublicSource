
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
	  
		configuration { "Debug_Static", "linux" }
			defines { "_DEBUG", "ANIMAT_STATIC"}
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("AnimatSimulatorD")
			links { "BootstrapLoader_vc10D", "dl", "pthread", "StdUtils_vc10D", "AnimatSim_vc10D", "OsgAnimatSim_vc10D", "BulletAnimatSim_vc10D" }
			postbuildcommands { "cp Debug/AnimatSimulatorD ../../../bin" }
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("AnimatSimulator")
			links { "BootstrapLoader_vc10", "dl", "pthread" }
			postbuildcommands { "cp Release/AnimatSimulator ../../../bin" }
