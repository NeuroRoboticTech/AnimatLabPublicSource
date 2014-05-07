#ifndef __ROBOTICS_ANIMAT_LIB_DLL_H__
#define __ROBOTICS_ANIMAT_LIB_DLL_H__

#ifndef _WIN64
	#ifdef _DEBUG
	    #pragma comment(lib, "RoboticsAnimatSim_vc10D.lib")
    #else
	    #pragma comment(lib, "RoboticsAnimatSim_vc10.lib")
	#endif      // _DEBUG  
#else
	#ifdef _DEBUG
	    #pragma comment(lib, "RoboticsAnimatSim_vc10D_x64.lib")
    #else
	    #pragma comment(lib, "RoboticsAnimatSim_vc10_x64.lib")
	#endif      // _DEBUG  
#endif          // _WIN64

#ifdef WIN32
	#define ROBOTICS_PORT __declspec( dllimport )
    #define ARDUINO_PORT __declspec( dllimport )
#else
	#define ROBOTICS_PORT
    #define ARDUINO_PORT
#endif

#include "StdUtils.h"
#include "AnimatSim.h"

#include "RoboticsAnimatSimConstants.h"
#include "dynamixel.h"

#include "ofArduino.h"

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

        namespace RobotIOControls
        {
			namespace DynamixelUSB
			{
				class RbDynamixelUSB;
				class RbDynamixelUSBMotorUpdateData;
				class RbDynamixelUSBServo;
				class RbDynamixelUSBHinge;
				class RbDynamixelUSBPrismatic;
			}

			namespace Firmata
			{
				class RbFirmataController;
				class RbFirmataPart;
				class RbFirmataAnalogInput;
				class RbFirmataAnalogOutput;
				class RbFirmataDigitalInput;
				class RbFirmataDigitalOutput;
				class RbFirmataHingeServo;
				class RbFirmataPrismaticServo;
				class RbFirmataPWMOutput;
			}
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
using namespace RoboticsAnimatSim::Robotics::RobotIOControls;
using namespace RoboticsAnimatSim::Robotics::RobotIOControls::DynamixelUSB;
using namespace RoboticsAnimatSim::Robotics::RobotIOControls::Firmata;


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
#include "RbDynamixelUSB.h"
#include "RbDynamixelUSBServo.h"
#include "RbDynamixelUSBHinge.h"
#include "RbDynamixelUSBPrismatic.h"

#include "RbFirmataController.h"
#include "RbFirmataPart.h"
#include "RbFirmataAnalogInput.h"
#include "RbFirmataAnalogOutput.h"
#include "RbFirmataDigitalInput.h"
#include "RbFirmataDigitalOutput.h"
#include "RbFirmataHingeServo.h"
#include "RbFirmataPrismaticServo.h"
#include "RbFirmataPWMOutput.h"

#endif // __ROBOTICS_ANIMAT_LIB_DLL_H__
