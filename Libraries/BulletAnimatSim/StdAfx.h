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

#include "OsgAnimatSim.h"
#include "BulletAnimatSimConstants.h"

#include <osgbDynamics/MotionState.h>
#include <osgbDynamics/CreationRecord.h>
#include <osgbDynamics/RigidBody.h>
#include <osgbCollision/CollisionShapes.h>
#include <osgbCollision/RefBulletObject.h>
#include <osgbDynamics/GroundPlane.h>
#include <osgbCollision/GLDebugDrawer.h>
#include <osgbCollision/Utils.h>
#include <osgbInteraction/DragHandler.h>
#include <osgbInteraction/LaunchHandler.h>
#include <osgbInteraction/SaveRestoreHandler.h>

#include <osgwTools/InsertRemove.h>
#include <osgwTools/FindNamedNode.h>
#include <osgwTools/GeometryOperation.h>
#include <osgwTools/GeometryModifier.h>
#include <osgwTools/Shapes.h>

#include <btBulletDynamicsCommon.h>

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
