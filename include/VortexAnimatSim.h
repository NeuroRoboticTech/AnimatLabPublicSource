#ifndef __VORTEX_ANIMAT_LIB_DLL_H__
#define __VORTEX_ANIMAT_LIB_DLL_H__

#ifdef _DEBUG
	#pragma comment(lib, "VortexAnimatSim_vc10D.lib")
#else
	#pragma comment(lib, "VortexAnimatSim_vc10.lib")
#endif

#ifdef WIN32
	#define VORTEX_PORT __declspec( dllimport )
#else
	#define VORTEX_PORT
#endif

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

#include "VxVehicle/VxVehicleBase.h"
#include "VxVehicle/VxCommonVehicleSub.h"

#include <VxPersistence/Persistence.h>
#include "Vx/VxEulerAngles.h"

#include "OsgAnimatSim.h"
#include "VortexAnimatSimConstants.h"

//Simulation Objects
namespace VortexAnimatSim
{
	class VsClassFactory;
	class VsSimulator;

	namespace Environment
	{
		class VsJoint;
		class VsMotorizedJoint;
		class VsLine;
		class VsMaterialType;
        	class VsConstraintRelaxation;
        	class VsConstraintFriction;
		class VsOrganism;
		class VsRigidBody;
		class VsStructure;
        	class VsMatrixUtil;

		namespace Bodies
		{
			class VsBox;
			class VsCone;
			class VsCylinder;
			class VsLinearHillMuscle;
			class VsLinearHillStretchReceptor;
			class VsPlane;
			class VsSphere;
			class VsSpring;
			class VsTorus;
			class VsEllipsoid;
			class VsMouth;
			class VsFluidPlane;
			class VsMeshBase;
			class VsMesh;
			class VsTerrain;
		}

		namespace Joints
		{
			class VsBallSocket;
			class VsHinge;
			class VsHingeLimit;
			class VsPrismatic;
			class VsPrismaticLimit;
			class VsUniversal;
		}
	}

	namespace ExternalStimuli
	{
		class VsForceStimulus;
		class VsInverseMuscleCurrent;
		class VsMotorVelocityStimulus;
	}

	namespace Recording
	{
		namespace KeyFrames
		{
			class VsSnapshotKeyFrame;
			class VsVideoKeyFrame;
		}

		class VsSimulationRecorder;
	}

	namespace Visualization
	{
		class VsIntersectionEvent;
	}
}

using namespace VortexAnimatSim;
using namespace VortexAnimatSim::ExternalStimuli;
using namespace VortexAnimatSim::Environment;
using namespace VortexAnimatSim::Environment::Bodies;
using namespace VortexAnimatSim::Environment::Joints;
using namespace VortexAnimatSim::Recording;
using namespace VortexAnimatSim::Recording::KeyFrames;
using namespace VortexAnimatSim::Visualization;
using namespace Vx;

#include "VsClassFactory.h"

#include "VsOsgGeometry.h"
#include "VsConstraintRelaxation.h"
#include "VsConstraintFriction.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsPlane.h"
#include "VsBox.h"
#include "VsCylinder.h"
#include "VsCone.h" 
#include "VsSphere.h"
#include "VsTorus.h"
#include "VsEllipsoid.h"
#include "VsFluidPlane.h"
#include "VsMeshBase.h"
#include "VsMesh.h"
#include "VsTerrain.h"

#include "VsHinge.h"
#include "VsHingeLimit.h"
#include "VsPrismatic.h"
#include "VsPrismaticLimit.h"
#include "VsBallSocket.h"
#include "VsUniversal.h"

#include "VsLine.h"
#include "VsLinearHillMuscle.h"
#include "VsLinearHillStretchReceptor.h"
#include "VsSpring.h"

#include "VsSimulator.h"

#include "VsMaterialType.h"
//
//#include "VsVideoKeyFrame.h"
//#include "VsSnapshotKeyFrame.h"
//#include "VsSimulationRecorder.h"

#include "VsMotorVelocityStimulus.h"
#include "VsForceStimulus.h"
#include "VsIntersectionEvent.h"


#endif // __VORTEX_ANIMAT_LIB_DLL_H__
