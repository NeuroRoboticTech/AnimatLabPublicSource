// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#ifdef WIN32
    #define _SCL_SECURE_NO_WARNINGS
    #define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
    // Windows Header Files:
    #include <windows.h>

    #define ROBOTICS_PORT __declspec( dllexport )
#else
    #define ROBOTICS_PORT
#endif

//#define STD_TRACING_ON

#include "StdUtils.h"
#include "AnimatSim.h"

#include "RoboticsAnimatSimConstants.h"

//Simulation Objects
namespace RoboticsAnimatSim
{
	class RbClassFactory;
	class RbSimulator;

	namespace Environment
	{
		class RbJoint;
		class RbMotorizedJoint;
		class RbMaterialType;
        class RbConstraintRelaxation;
        class RbConstraintFriction;
		class RbRigidBody;

		namespace Bodies
		{
			class RbBox;
			class RbCone;
			class RbCylinder;
			class RbLinearHillMuscle;
			class RbLinearHillStretchReceptor;
			class RbPlane;
			class RbSphere;
			class RbSpring;
		}

		namespace Joints
		{
			class RbBallSocket;
			class RbHinge;
			class RbHingeLimit;
			class RbPrismatic;
			class RbPrismaticLimit;
		}
	}

	namespace ExternalStimuli
	{
		class RbForceStimulus;
		class RbMotorVelocityStimulus;
	}

}

using namespace RoboticsAnimatSim;
using namespace RoboticsAnimatSim::ExternalStimuli;
using namespace RoboticsAnimatSim::Environment;
using namespace RoboticsAnimatSim::Environment::Bodies;
using namespace RoboticsAnimatSim::Environment::Joints;
