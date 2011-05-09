#ifndef __VORTEX_ANIMAT_LIB_DLL_H__
#define __VORTEX_ANIMAT_LIB_DLL_H__

#ifdef _DEBUG
	#pragma comment(lib, "VortexAnimatSim_vc9D.lib")
#else
	#pragma comment(lib, "VortexAnimatSim_vc9.lib")
#endif

#define VORTEX_PORT __declspec( dllimport )

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

// OSG includes
#include <osg/Group>
#include <osg/Geode>
#include <osg/Geometry>
#include <osg/Matrixd>
#include <osg/MatrixTransform>
#include <osg/Material> 
#include <osg/Math>
#include <osg/Node>
#include <osg/PolygonMode>
#include <osg/PolygonOffset>
#include <osg/ShapeDrawable>
#include <osg/StateSet>
#include <osg/TexGen>
#include <osg/Plane>
#include <osg/Texture>
#include <osg/Texture1D>
#include <osg/Texture2D>
#include <osg/Texture3D>
#include <osg/TextureCubeMap>
#include <osg/TextureRectangle>
#include <osg/CullFace>
#include <osgText/Text>
#include <osg/ref_ptr>
#include <osg/CoordinateSystemNode>
#include <osg/ClusterCullingCallback>
#include <osg/Camera>
#include <osg/io_utils>
#include <osg/LineWidth>
#include <osg/Autotransform>

#include <osgDB/ReadFile>
#include <osgDB/WriteFile>
#include <osgDB/FileUtils>

#include <osgFX/BumpMapping>
#include <osgSim/DOFTransform>

#include <osgUtil/Optimizer>
#include <osgUtil/SmoothingVisitor>

#include <osgViewer/GraphicsWindow>
#include <osgViewer/Viewer>
#include <osgViewer/ViewerEventHandlers>
#include <osgViewer/api/win32/GraphicsWindowWin32>
#include <osgViewer/CompositeViewer>

#include <osgGA/GUIEventAdapter>
#include <osgGA/GUIActionAdapter>
#include <osgGA/StateSetManipulator>
#include <osgGA/TrackballManipulator>
#include <osgGA/MatrixManipulator>

#include <osgManipulator/CommandManager>
#include <osgManipulator/TabBoxDragger>
#include <osgManipulator/TabPlaneDragger>
#include <osgManipulator/TabPlaneTrackballDragger>
#include <osgManipulator/TrackballDragger>
#include <osgManipulator/Translate1DDragger>
#include <osgManipulator/Translate2DDragger>
#include <osgManipulator/TranslateAxisDragger>
#include <osgManipulator/AntiSquish>

#include "WorldCoordinateNodeVisitor.h"

using namespace osgGA;

#include <OpenThreads/Thread>

#include "VortexAnimatSimConstants.h"

//Simulation Objects
namespace VortexAnimatSim
{
	class VsClassFactory;
	class VsSimulator;

	namespace Environment
	{
		class VsMovableItem;
		class VsBody;
		class VsJoint;
		class VsMotorizedJoint;
		class VsLine;
		class VsMaterialPair;
		class VsOrganism;
		class VsRigidBody;
		class VsStructure;

		namespace Bodies
		{
			class VsAttachment;
			class VsBox;
			class VsCone;
			class VsCylinder;
			class VsLinearHillMuscle;
			class VsLinearHillStretchReceptor;
			class VsPlane;
			class VsSphere;
			class VsSpring;
		}

		namespace Joints
		{
			class VsBallSocket;
			class VsHinge;
			class VsHingeLimit;
			class VsPrismatic;
			class VsPrismaticLimit;
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
		class VsCameraManipulator;
		class VsDragger;
		class VsDraggerHandler;
		class VsHud;
		class VsHudItem;
		class VsHudText;
		class VsIntersectionEvent;
		class VsMouseSpring;
		class VsOsgUserData;
		class VsOsgUserDataVisitor;
		class VsSimulationWindow;
		class VsSimulationWindowMgr;
		class VsTrackballDragger;
		class VsTranslateAxisDragger;
		class WorldCoordinateNodeVisitor;
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

#include "VsMovableItem.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsPlane.h"
#include "VsBox.h"
#include "VsCylinder.h"
#include "VsCone.h" 
#include "VsSphere.h"

#include "VsHinge.h"
#include "VsHingeLimit.h"
#include "VsPrismatic.h"
#include "VsPrismaticLimit.h"
#include "VsBallSocket.h"

#include "VsAttachment.h"
#include "VsLine.h"
#include "VsLinearHillMuscle.h"
#include "VsLinearHillStretchReceptor.h"
#include "VsSpring.h"

#include "VsOrganism.h"
#include "VsStructure.h"
#include "VsSimulator.h"

#include "VsMaterialPair.h"
//
//#include "VsVideoKeyFrame.h"
//#include "VsSnapshotKeyFrame.h"
//#include "VsSimulationRecorder.h"

#include "VsMotorVelocityStimulus.h"
#include "VsForceStimulus.h"
#include "VsInverseMuscleCurrent.h"

#include "VsTrackballDragger.h"
#include "VsTranslateAxisDragger.h"
#include "WorldCoordinateNodeVisitor.h"

#include "VsHudItem.h"
#include "VsHudText.h"
#include "VsHud.h"
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"
#include "VsDragger.h"
#include "VsDraggerHandler.h"
#include "VsMouseSpring.h"
#include "VsCameraManipulator.h"
#include "VsIntersectionEvent.h"
#include "VsSimulationWindow.h"
#include "VsSimulationWindowMgr.h"


#endif // __VORTEX_ANIMAT_LIB_DLL_H__
