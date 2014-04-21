
-- A solution contains projects, and defines the available configurations
solution "AnimatLabCode"
	configurations { "Debug", "Release", "Debug_Double", "Release_Double", "Debug_Static" }
	dofile "Projects.lua"
