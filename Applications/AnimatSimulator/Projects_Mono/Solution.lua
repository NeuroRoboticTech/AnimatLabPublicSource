
-- A solution contains projects, and defines the available configurations
solution "AnimatSimulator"
	configurations { "Debug", "Release", "Debug_Double", "Release_Double", "Debug_Static" }
	dofile "Projects.lua"
