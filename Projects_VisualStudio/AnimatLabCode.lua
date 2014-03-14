-- A solution contains projects, and defines the available configurations
solution "AnimatLabCode"
	platforms {"x32"}
	dofile "..\\Libraries\\AnimatGUI\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\AnimatGUICtrls\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\FiringRateGUI\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\IntegrateFireGUI\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\LicensedAnimatGUI\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\ManagedAnimatInterfaces\\Projects_VisualStudio\\Projects.lua"

	dofile "..\\Libraries\\StdUtils\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\AnimatSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\FiringRateSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\IntegrateFireSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\OsgAnimatSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\BulletAnimatSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\RoboticsAnimatSim\\Projects_VisualStudio\\Projects.lua"
	dofile "..\\Libraries\\VortexAnimatSim\\Projects_VisualStudio\\Projects.lua"
