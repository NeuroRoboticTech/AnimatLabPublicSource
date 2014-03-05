-- A solution contains projects, and defines the available configurations
solution "BulletAnimatSim"
	configurations { "Debug", "Debug_double", "Debug_x64", "Release", "Release_double" }
	platforms {"x32", "x64"}
	dofile "Projects.lua"
	