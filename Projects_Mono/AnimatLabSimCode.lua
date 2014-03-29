-- A solution contains projects, and defines the available configurations
solution "AnimatSimCode"
	configurations { "Debug", "Release", "Debug_Double", "Release_Double", "Debug_Static" }
	dofile "..\\Libraries\\BootstrapLoader\\Projects_Mono\\Projects.lua"
	dofile "..\\Libraries\\StdUtils\\Projects_Mono\\Projects.lua"
	dofile "..\\Libraries\\AnimatSim\\Projects_Mono\\Projects.lua"
	dofile "..\\Libraries\\FiringRateSim\\Projects_Mono\\Projects.lua"
	dofile "..\\Libraries\\IntegrateFireSim\\Projects_Mono\\Projects.lua"
	dofile "..\\Libraries\\OsgAnimatSim\\Projects_Mono\\Projects.lua"
	dofile "..\\Libraries\\BulletAnimatSim\\Projects_Mono\\Projects.lua"
	dofile "..\\Libraries\\RoboticsAnimatSim\\Projects_Mono\\Projects.lua"
	dofile "..\\Applications\\AnimatSimulator\\Projects_Mono\\Projects.lua"
