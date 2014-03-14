-- A solution contains projects, and defines the available configurations
solution "StdUtils"
	configurations { "Debug", "Release", "Debug_Double", "Release_Double" }
	platforms {"x32"}
	dofile "Projects.lua"
	dofile "UnitTests.lua"
	dofile "ClassFactoryTests.lua"
