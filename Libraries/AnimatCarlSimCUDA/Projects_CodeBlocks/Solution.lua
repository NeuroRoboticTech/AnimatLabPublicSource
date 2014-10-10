
-- A solution contains projects, and defines the available configurations
solution "AnimatCarlSim"
	configurations { "Debug", "Release", "Debug_Double", "Release_Double" }
	dofile "Projects.lua"
