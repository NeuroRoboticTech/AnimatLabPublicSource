-- A solution contains projects, and defines the available configurations
solution "OsgAnimatSimSim"
	configurations { "Debug", "Release", "Debug_x64", "Release_x64" }
	platforms {"x32", "x64"}
	dofile "Projects.lua"
