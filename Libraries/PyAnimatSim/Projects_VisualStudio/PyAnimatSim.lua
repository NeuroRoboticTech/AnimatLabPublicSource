-- A solution contains projects, and defines the available configurations
solution "PyAnimatSim"
	configurations { "Release" }
	platforms {"x32"}

	project "PyAnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp",
				 "../*.cxx"}
		
		configuration { "Release", "windows" }
			includedirs { "../../../include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../../../3rdParty/boost_1_54_0",
						  "$(PYTHON_ROOT)/include"}	  
			libdirs { "../../../lib",
					  "$(OutDir)",
					  "../../../../3rdParty/boost_1_54_0/lib",
					  "$(PYTHON_ROOT)/libs" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("PyAnimatSim")
			postbuildcommands { "Copy $(TargetPath) ..\\..\\..\\bin\\_PyAnimatSim.pyd",
								"Copy ..\\PyAnimatSim.py ..\\..\\..\\bin\\PyAnimatSim.py" }
	