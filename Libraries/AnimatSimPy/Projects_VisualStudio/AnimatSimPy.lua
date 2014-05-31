-- A solution contains projects, and defines the available configurations
solution "AnimatSimPy"
	configurations { "Debug", "Release" }
	platforms {"x32"}

	project "AnimatSimPy"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp",
				 "../*.cxx"}
		
		configuration { "Debug", "windows" }
			includedirs { "../../../include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../FiringRateSim",
						  "../../../../3rdParty/boost_1_54_0",
						  "../../../../3rdParty/Python-2.7.6/include",
						  "../../../../3rdParty/Python-2.7.6/PC"}	  
			libdirs { "../../../lib",
					  "$(OutDir)",
					  "../../../../3rdParty/boost_1_54_0/lib",
					  "../../../../3rdParty/Python-2.7.6/PCbuild" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "_CRT_SECURE_NO_WARNINGS" }
			buildoptions( "/bigobj" )
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("AnimatSimPy_d")
			links( "python27_d")
			postbuildcommands { "Copy $(OutDir)AnimatSimPy_d.lib ..\\..\\..\\lib\\AnimatSimPy_d.lib",
								"Copy $(TargetPath) ..\\..\\..\\bin\\_AnimatSimPy_d.pyd",
								"Copy ..\\AnimatSimPy.py ..\\..\\..\\bin\\AnimatSimPy.py"}
		
		configuration { "Release", "windows" }
			includedirs { "../../../include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../FiringRateSim",
						  "../../../../3rdParty/boost_1_54_0",
						  "../../../../3rdParty/Python-2.7.6/include",
						  "../../../../3rdParty/Python-2.7.6/PC"}	  
			libdirs { "../../../lib",
					  "$(OutDir)",
					  "../../../../3rdParty/boost_1_54_0/lib",
					  "../../../../3rdParty/Python-2.7.6/PCbuild" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("AnimatSimPy")
			links( "python27")
			postbuildcommands { "Copy $(OutDir)AnimatSimPy.lib ..\\..\\..\\lib\\AnimatSimPy.lib",
								"Copy $(TargetPath) ..\\..\\..\\bin\\_AnimatSimPy.pyd",
								"Copy ..\\AnimatSimPy.py ..\\..\\..\\bin\\AnimatSimPy.py" }
	
	
	project "AnimatSimPyTests"
		language "C++"
		kind     "ConsoleApp"
		files  { "../UnitTests/*.h",
				 "../UnitTests/*.cpp"}
		
		configuration { "Debug", "windows" }
			includedirs { "../../../include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../FiringRateSim",
						  "../",
						  "../../../../3rdParty/boost_1_54_0",
						  "../../../../3rdParty/Python-2.7.6/include",
						  "../../../../3rdParty/Python-2.7.6/PC"}	  
			libdirs { "../../../lib",
					  "$(OutDir)",
					  "../../../../3rdParty/boost_1_54_0/lib",
					  "../../../../3rdParty/Python-2.7.6/PCbuild" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("AnimatSimPyTestsD")
			links( "python27_d")
			postbuildcommands { "Copy $(TargetPath) ..\\..\\..\\bin" }
		
		configuration { "Release", "windows" }
			includedirs { "../../../include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../FiringRateSim",
						  "../",
						  "../../../../3rdParty/boost_1_54_0",
						  "../../../../3rdParty/Python-2.7.6/include",
						  "../../../../3rdParty/Python-2.7.6/PC"}	  
			libdirs { "../../../lib",
					  "$(OutDir)",
					  "../../../../3rdParty/boost_1_54_0/lib",
					  "../../../../3rdParty/Python-2.7.6/PCbuild" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("AnimatSimPyTests")
			links( "python27")
			postbuildcommands { "Copy $(TargetPath) ..\\..\\..\\bin" }
		