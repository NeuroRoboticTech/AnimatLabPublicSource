-- A solution contains projects, and defines the available configurations
solution "StdUtilsGen"
	configurations { "Debug", "Release" }

	project "StdUtilsGen"
		language "C++"
		kind     "SharedLib"
		files  { "MarkupSTL.h",
				 "StdAfx.h",
				 "StdAvi.h",
				 "StdBitmap.h",
				 "StdClassFactory.h",
				 "StdColor.h",
				 "StdCriticalSection.h",
				 "StdErrorInfo.h",
				 "StdFixed.h",
				 "StdFont.h",
				 "StdIncludes.h",
				 "StdLookupTable.h",
				 "StdPostFixEval.h",
				 "StdSerialize.h",
				 "StdTimer.h",
				 "StdUtilFunctions.h",
				 "StdVariable.h",
				 "StdVariant.h",
				 "StdXml.h",
				 "XYTrace.h",
				 "MarkupSTL.cpp",
				 "MersenneTwister.cpp",
				 "StdAfx.cpp",
				 "StdAvi.cpp",
				 "StdBitmap.cpp",
				 "StdClassFactory.cpp",
				 "StdColor.cpp",
				 "StdCriticalSection.cpp",
				 "StdErrorInfo.cpp",
				 "StdFixed.cpp",
				 "StdFont.cpp",
				 "StdLookupTable.cpp",
				 "StdPostFixEval.cpp",
				 "StdSerialize.cpp",
				 "StdTimer.cpp",
				 "StdUtilFunctions.cpp",
				 "StdUtils.cpp",
				 "StdVariable.cpp",
				 "StdVariant.cpp",
				 "StdXml.cpp",
				 "XYTrace.cpp", }
		includedirs { "../../include" }	  
		libdirs { "../../lib" }
	  
		configuration { "Debug", "windows" }
			defines { "WIN32", "_DEBUG", "_WINDOWS", "_USRDLL", "STDUTILS_EXPORTS", "_CRT_SECURE_NO_WARNINGS" }
			flags   { "Symbols", "SEH" }
			targetdir ("Debug")
			targetname ("StdUtils_vc10D")
			postbuildcommands { "Copy $(OutDir)StdUtils_vc10D.lib ../../lib/StdUtils_vc10D.lib", 
			                    "Copy $(TargetPath) ../../bin",  }
	 
		configuration { "Release", "windows" }
			defines { "WIN32", "NDEBUG", "_WINDOWS", "_USRDLL", "STDUTILS_EXPORTS" }
			flags   { "Optimize", "SEH" }
			targetdir ("Release")
			targetname ("StdUtils_vc10")
			postbuildcommands { "Copy $(OutDir)StdUtils_vc10.lib ../../lib/StdUtils_vc10.lib", 
			                    "Copy $(TargetPath) ../../bin",  }

		configuration { "Debug", "linux" }
			defines { "_DEBUG", "STDUTILS_EXPORTS"	}
			flags   { "Symbols" }
			targetdir ("Debug")
			targetname ("StdUtils_vc10D")
	 
		configuration { "Release", "linux" }
			defines { "NDEBUG", "STDUTILS_EXPORTS" }
			flags   { "Optimize" }
			targetdir ("Release")
			targetname ("StdUtils_vc10")
			