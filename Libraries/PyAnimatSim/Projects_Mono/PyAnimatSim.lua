
-- A solution contains projects, and defines the available configurations
solution "PyAnimatSim"
	configurations { "Release" }

	project "PyAnimatSim"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../../include", 
					  "../../../StdUtils",
					  "../../../AnimatSim",
				      "../../../../../3rdParty/stlsoft-1.9.117/include" }
		libdirs { "../../../../bin" }
		links { "dl"}
	  
		configuration { "Release", "linux" }
			defines { "NDEBUG" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("PyAnimatSim")
			links { "StdUtils", "AnimatSim"}
			postbuildcommands { "cp Release/libAnimatSim.so ../../../bin/_PyAnimatSim.pyd",
							    "cp ../PyAnimatSim.py ../../../bin/PyAnimatSim.py" }