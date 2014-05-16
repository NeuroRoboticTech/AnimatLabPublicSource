
	project "RoboticsAnimatSim_x64"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		
		configuration { "Debug_x64 or Debug_Double_x64", "windows" }
			includedirs { "../../../include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../../../3rdParty/boost_1_54_0",
						  "../../../../3rdParty/DynamixelSDK/x64/import",
						  "../../../../3rdParty/openFrameworksArduino/src",
						  "../../../../3rdParty/stlsoft-1.9.117/include"}	  
			libdirs { "../../../lib",
					  "$(OutDir)", 
					  "../../../../3rdParty/boost_1_54_0/lib_x64",
					  "../../../../3rdParty/DynamixelSDK/x64/import" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "ROBOTICSANIMATLIBRARY_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug_x64")
			targetname ("RoboticsAnimatSim_vc10D_x64")
			links { "wsock32", 
					"netapi32", 
					"comctl32", 
					"wbemuuid",
					"dynamixel",
					"openFrameworksArduinoD_x64"}
			postbuildcommands { "Copy $(OutDir)RoboticsAnimatSim_vc10D_x64.lib ..\\..\\..\\lib\\RoboticsAnimatSim_vc10D_x64.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin_x64" }
	 
		configuration { "Release_x64 or Release_Double_x64", "windows" }
			includedirs { "../../../include",
						  "../../StdUtils",
						  "../../AnimatSim",
						  "../../../../3rdParty/boost_1_54_0",
						  "../../../../3rdParty/DynamixelSDK/x64/import",
						  "../../../../3rdParty/openFrameworksArduino/src",
						  "../../../../3rdParty/stlsoft-1.9.117/include"}	  
			libdirs { "../../../lib",
					  "$(OutDir)", 
					  "../../../../3rdParty/boost_1_54_0/lib_x64",
					  "../../../../3rdParty/DynamixelSDK/x64/import" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "ROBOTICSANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release_x64")
			targetname ("RoboticsAnimatSim_vc10_x64")
			links { "wsock32",
					"netapi32",
					"comctl32",
					"wbemuuid",
					"dynamixel",
					"openFrameworksArduino_x64" }
			postbuildcommands { "Copy $(OutDir)RoboticsAnimatSim_vc10_x64.lib ..\\..\\..\\lib\\RoboticsAnimatSim_vc10_x64.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin_x64" }
