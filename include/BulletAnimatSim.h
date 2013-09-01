#ifndef __BULLET_ANIMAT_LIB_DLL_H__
#define __BULLET_ANIMAT_LIB_DLL_H__

#ifdef _DEBUG
	#pragma comment(lib, "BulletAnimatSim_vc10D.lib")
#else
	#pragma comment(lib, "BulletAnimatSim_vc10.lib")
#endif

#ifdef WIN32
	#define BULLET_PORT __declspec( dllimport )
#else
	#define BULLET_PORT
#endif

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
#include "BulletCollision/CollisionShapes/btShapeHull.h"
#include "BulletCollision/CollisionShapes/btHeightfieldTerrainShape.h"

//Simulation Objects
namespace BulletAnimatSim
{
	class BlClassFactory;
	class BlSimulator;

	namespace Environment
	{
		class BlJoint;
		class BlMotorizedJoint;
		class BlLine;
		class BlMaterialType;
        class BlConstraintRelaxation;
        class BlConstraintFriction;
		class BlOrganism;
		class BlRigidBody;
		class BlStructure;
        class BlMatrixUtil;

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
			class BlTorus;
			class BlEllipsoid;
			class BlMouth;
			class BlFluidPlane;
			class BlMeshBase;
			class BlMesh;
			class BlTerrain;
		}

		namespace Joints
		{
			class BlBallSocket;
			class BlHinge;
			class BlHingeLimit;
			class BlPrismatic;
			class BlPrismaticLimit;
			class BlUniversal;
		}
	}

	namespace ExternalStimuli
	{
		class BlForceStimulus;
		class BlMotorVelocityStimulus;
	}

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

#include "BlClassFactory.h"

#include "BlOsgGeometry.h"
#include "BlConstraintRelaxation.h"
#include "BlConstraintFriction.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlPlane.h"
#include "BlBox.h"
#include "BlCylinder.h"
#include "BlCone.h" 
#include "BlSphere.h"
#include "BlTorus.h"
#include "BlEllipsoid.h"
#include "BlFluidPlane.h"
#include "BlMeshBase.h"
#include "BlMesh.h"
#include "BlTerrain.h"

#include "BlHinge.h"
#include "BlHingeLimit.h"
#include "BlPrismatic.h"
#include "BlPrismaticLimit.h"
#include "BlBallSocket.h"
#include "BlUniversal.h"

#include "BlLine.h"
#include "BlLinearHillMuscle.h"
#include "BlLinearHillStretchReceptor.h"
#include "BlSpring.h"

#include "BlSimulator.h"

#include "BlMaterialType.h"

#include "BlForceStimulus.h"
#include "BlIntersectionEvent.h"


#endif // __BULLET_ANIMAT_LIB_DLL_H__
