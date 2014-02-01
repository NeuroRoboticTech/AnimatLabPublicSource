#ifndef __ROBOTICS_ANIMAT_LIB_DLL_H__
#define __ROBOTICS_ANIMAT_LIB_DLL_H__

#ifdef _DEBUG
	#pragma comment(lib, "RoboticsAnimatSim_vc10D.lib")
#else
	#pragma comment(lib, "RoboticsAnimatSim_vc10.lib")
#endif

#ifdef WIN32
	#define ROBOTICS_PORT __declspec( dllimport )
#else
	#define ROBOTICS_PORT
#endif

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
		class RbMovableItem;
		class RbBody;
		class RbJoint;
		class RbMotorizedJoint;
		class RbLine;
		class RbMaterialType;
        class RbConstraintRelaxation;
        class RbConstraintFriction;
		class RbOrganism;
		class RbRigidBody;
		class RbStructure;
        class RbMatrixUtil;

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
			class RbTorus;
			class RbEllipsoid;
			class RbMouth;
			class RbFluidPlane;
			class RbMeshBase;
			class RbMesh;
			class RbTerrain;
		}

		namespace Joints
		{
			class RbBallSocket;
			class RbHinge;
			class RbHingeLimit;
			class RbPrismatic;
			class RbPrismaticLimit;
			class RbUniversal;
		}
	}

	namespace ExternalStimuli
	{
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


#include "RbClassFactory.h"

#include "RbConstraintRelaxation.h"
#include "RbConstraintFriction.h"
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbRigidBody.h"
#include "RbBox.h"
#include "RbCylinder.h"
#include "RbCone.h" 
#include "RbSphere.h"
#include "RbTorus.h"
#include "RbEllipsoid.h"
#include "RbMesh.h"

#include "RbHinge.h"
#include "RbHingeLimit.h"
#include "RbPrismatic.h"
#include "RbPrismaticLimit.h"
#include "RbBallSocket.h"
#include "RbUniversal.h"

#include "RbLine.h"
#include "RbLinearHillMuscle.h"
#include "RbLinearHillStretchReceptor.h"
#include "RbSpring.h"

#include "RbSimulator.h"
#include "RbMaterialType.h"

#include "RbLANWirelessInterface.h"
#include "RbDynamixelCM5USBUARTHingeController.h"
#include "RbDynamixelCM5USBUARTPrismaticController.h"
#include "RbSwitchInputSensor.h"

#endif // __ROBOTICS_ANIMAT_LIB_DLL_H__
