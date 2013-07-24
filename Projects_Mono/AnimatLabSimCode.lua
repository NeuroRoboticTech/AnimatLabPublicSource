-- A solution contains projects, and defines the available configurations
solution "AnimatSimCode"
	configurations { "Debug", "Release" }
	dofile "..\\Libraries\\BootstrapLoader\\Projects_Mono\\Projects.lua"
	dofile "..\\Libraries\\StdUtils\\Projects_Mono\\Projects.lua"
	dofile "..\\Libraries\\AnimatSim\\Projects_Mono\\Projects.lua"
	dofile "..\\Libraries\\FiringRateSim\\Projects_Mono\\Projects.lua"
	dofile "..\\Libraries\\IntegrateFireSim\\Projects_Mono\\Projects.lua"
	dofile "..\\Libraries\\VortexAnimatSim\\Projects_Mono\\Projects.lua"
