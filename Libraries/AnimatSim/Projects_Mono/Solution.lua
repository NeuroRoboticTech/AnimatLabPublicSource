
-- A solution contains projects, and defines the available configurations
solution "AnimatSim"
	configurations { "Debug", "Release" }
	dofile "..\..\StdUtils\Projects_Mono\Projects.lua"
	dofile "Projects.lua"
