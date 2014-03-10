-- A solution contains projects, and defines the available configurations
solution "AnimatSimCode"
	configurations { "Debug", "Release", "Debug_x64", "Release_x64" }
	platforms {"x32", "x64"}
	dofile "..\\Applications\\AnimatSimulator\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\BootstrapLoader\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\StdUtils\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\AnimatSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\FiringRateSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\IntegrateFireSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\OsgAnimatSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\BulletAnimatSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\RoboticsAnimatSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\VortexAnimatSim\\Projects_VisualStudio\\Projects.lua"
