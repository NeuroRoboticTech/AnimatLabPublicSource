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

//Include the timer code and openthreads code from osg
#include <osg/Timer>
#include <OpenThreads/Thread>

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

    namespace Robotics
    {
        namespace RobotInterfaces
        {
            class RbLANWirelessInterface;
        }

        namespace MotorControlSystems
        {
            class RbDynamixelCM5USBUARTHingeController;
            class RbDynamixelCM5USBUARTPrismaticController;
        }

        namespace InputSensorSystems
        {
            class RbSwitchInputSensor;
        }
    }
}

using namespace RoboticsAnimatSim;
using namespace RoboticsAnimatSim::ExternalStimuli;
using namespace RoboticsAnimatSim::Environment;
using namespace RoboticsAnimatSim::Environment::Bodies;
using namespace RoboticsAnimatSim::Environment::Joints;
using namespace RoboticsAnimatSim::Robotics;
using namespace RoboticsAnimatSim::Robotics::RobotInterfaces;
using namespace RoboticsAnimatSim::Robotics::MotorControlSystems;
using namespace RoboticsAnimatSim::Robotics::InputSensorSystems;