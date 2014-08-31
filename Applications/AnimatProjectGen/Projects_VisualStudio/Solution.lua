-- A solution contains projects, and defines the available configurations
solution "AnimatProjectGen"
	configurations { "Debug", "Release" }
	platforms {"x32"}
	
	project "AnimatProjectGen"
		language "C#"
		kind     "ConsoleApp"
		files  { "../*.cs"}
		links  { "System" }

		configuration {"../Templates/**.lua" }
			buildaction "Embed"
	
		configuration { "Debug" }
			defines { "DEBUG" }
			flags   { "Symbols" }
	 
		configuration { "Release" }
			 flags { "Optimize" }
