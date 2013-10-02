
-- A solution contains projects, and defines the available configurations
solution "AnimatSimulator"
	configurations { "Debug", "Debug_Static", "Release" }
	dofile "Projects.lua"
