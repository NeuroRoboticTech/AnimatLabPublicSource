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

    #define BULLET_PORT __declspec( dllexport )
#else
    #define BULLET_PORT
#endif

//#define STD_TRACING_ON

#include "StdUtils.h"
#include "AnimatSim.h"

// Vx includes
#include "Vx/VxQuaternion.h"
#include "Vx/VxPrecision.h"
#include "Vx/VxFrame.h"
#include "Vx/VxUniverse.h"
#include "Vx/VxAssembly.h"
#include "Vx/VxPart.h"

#include "Vx/VxBox.h"
#include "Vx/VxCylinder.h"
#include "Vx/VxSphere.h"
#include "Vx/VxPlane.h"
#include "Vx/VxTerrain.h"
#include "Vx/VxTriangleMesh.h"
#include "Vx/VxConvexMesh.h"
#include "Vx/VxCollisionGeometry.h"
#include "Vx/VxHeightField.h"

#include "Vx/VxConstraint.h"
#include "Vx/VxHinge.h"
#include "Vx/VxPrismatic.h"
#include "Vx/VxBallAndSocket.h"
#include "Vx/VxHomokinetic.h"
#include "Vx/VxUniversal.h"
#include "Vx/VxDistanceJoint.h"
#include "Vx/VxLinear1.h"
#include "Vx/VxLinear2.h"
#include "Vx/VxLinear3.h"
#include "Vx/VxSpring.h"
#include "Vx/VxRPRO.h"

#include "Vx/VxMaterialTable.h"
#include "Vx/VxContactProperties.h"
#include "Vx/VxConstraintController.h"
#include "Vx/VxVector3.h"
#include "VxOSG/VxOSG.h"

#include "Vx/VxConstraintController.h"
#include "Vx/VxEventSubscriber.h"
#include "Vx/VxSceneGraphInterface.h"
#include "Vx/VxTransform.h"
#include "Vx/VxDynamicsResponseInput.h"
#include "Vx/VxIntersectResult.h"
#include "Vx/VxFluidInteraction.h"
#include "Vx/VxSolverParameters.h"

#include "VxVehicle/VxVehicleBase.h"
#include "VxVehicle/VxCommonVehicleSub.h"

#include <VxPersistence/Persistence.h>
#include "Vx/VxEulerAngles.h"

#include "OsgAnimatSim.h"
#include "BulletAnimatSimConstants.h"

//Simulation Objects
namespace BulletAnimatSim
{
	class BlClassFactory;
	class BlSimulator;

	namespace ExternalStimuli
	{
		class BlForceStimulus;
		class BlMotorVelocityStimulus;
	}

	namespace Environment
	{
		class BlJoint;
		class BlMotorizedJoint;
		class BlMaterialType;
        class BlConstraintRelaxation;
        class BlConstraintFriction;
		class BlRigidBody;

		namespace Bodies
		{
			class BlBox;
			class BlCone;
			class BlCylinder;
			class BlLinearHillMuscle;
			class BlLinearHillStretchReceptor;
			class BlPlane;
			class BlSphere;
			class BlSpring;
		}

		namespace Joints
		{
			class BlBallSocket;
			class BlHinge;
			class BlHingeLimit;
			class BlPrismatic;
			class BlPrismaticLimit;
		}
	}

	namespace ExternalStimuli
	{
		class BlForceStimulus;
		class BlMotorVelocityStimulus;
	}

	//namespace Recording
	//{
	//	namespace KeyFrames
	//	{
	//		class VsSnapshotKeyFrame;
	//		class VsVideoKeyFrame;
	//	}

	//	class VsSimulationRecorder;
	//}

	namespace Visualization
	{
		class BlIntersectionEvent;
	}
}

using namespace BulletAnimatSim;
using namespace BulletAnimatSim::ExternalStimuli;
using namespace BulletAnimatSim::Environment;
using namespace BulletAnimatSim::Environment::Bodies;
using namespace BulletAnimatSim::Environment::Joints;
using namespace BulletAnimatSim::Visualization;
using namespace Vx;
