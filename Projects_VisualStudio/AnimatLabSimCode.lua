-- A solution contains projects, and defines the available configurations
solution "AnimatSimCode"
	configurations { "Debug", "Release" }
	dofile "..\Libraries\StdUtils\Projects_VisualStudio\Projects.lua"
	dofile "..\Libraries\Animatsim\Projects_VisualStudio\Projects.lua"
