
	project "AnimatSim_x64"
		language "C++"
		kind     "SharedLib"
		files  { "../*.h",
				 "../*.cpp"}
		includedirs { "../../../include", 
					  "../../StdUtils", 
					  "../../../../3rdParty/boost_1_54_0",
					  "../../../../3rdParty/stlsoft-1.9.117/include" }	  
		libdirs { "../../../lib", 
			      "../../../../3rdParty/boost_1_54_0/lib_x64" }
		
		configuration { "Debug_x64 or Debug_Double_x64", "windows" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "ANIMATLIBRARY_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug_x64")
			targetname ("AnimatSim_vc10D_x64")
			postbuildcommands { "Copy $(OutDir)AnimatSim_vc10D_x64.lib ..\\..\\..\\lib\\AnimatSim_vc10D_x64.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin_x64" }
	 
		configuration { "Release_x64 or Release_Double_x64", "windows" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "ANIMATLIBRARY_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release_x64")
			targetname ("AnimatSim_vc10_x64")
			postbuildcommands { "Copy $(OutDir)AnimatSim_vc10_x64.lib ..\\..\\..\\lib\\AnimatSim_vc10_x64.lib", 
			                    "Copy $(TargetPath) ..\\..\\..\\bin_x64" }
