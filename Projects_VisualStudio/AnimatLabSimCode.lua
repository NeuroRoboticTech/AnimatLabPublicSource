-- A solution contains projects, and defines the available configurations
solution "AnimatLabSimCode"
	configurations { "Debug", "Release", "Debug_Double", "Release_Double" }
	platforms {"x32"}
	dofile "..\\Applications\\AnimatSimulator\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\BootstrapLoader\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\StdUtils\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\StdUtils\\Projects_VisualStudio\\UnitTests.lua"
	dofile "..\\Libraries\\StdUtils\\Projects_VisualStudio\\ClassFactoryTests.lua"
	dofile "..\\Libraries\\AnimatSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\FiringRateSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\IntegrateFireSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\OsgAnimatSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\BulletAnimatSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\BulletAnimatSim\\Projects_VisualStudio\\UnitTests.lua"
	dofile "..\\Libraries\\RoboticsAnimatSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\RoboticsAnimatSim\\Projects_VisualStudio\\UnitTests.lua"
	dofile "..\\Libraries\\VortexAnimatSim\\Projects_VisualStudio\\Projects.lua"
