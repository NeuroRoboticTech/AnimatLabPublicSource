-- A solution contains projects, and defines the available configurations
solution "HybridInterfaceSim"
	configurations { "Debug", "Release", "Debug_Double", "Release_Double" }
	platforms {"x32"}
	dofile "Projects.lua"