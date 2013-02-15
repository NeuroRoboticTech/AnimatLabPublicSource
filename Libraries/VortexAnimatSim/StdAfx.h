// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once


#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
// Windows Header Files:
#include <windows.h>

#define VORTEX_PORT __declspec( dllexport )

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

// OSG includes
#include <osg/Group>
#include <osg/Geode>
#include <osg/Geometry>
#include <osg/Matrix>
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
#include <osg/StateAttribute>
#include <osg/TexMat>

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
//#include "VsStatsHandler.h"

using namespace osgGA;

#include <OpenThreads/Thread>

#include "VortexAnimatSimConstants.h"

//Simulation Objects
namespace VortexAnimatSim
{
	class VsClassFactory;
	class VsSimulator;
	class VsMeshMgr;

	namespace ExternalStimuli
	{
		class VsForceStimulus;
		class VsInverseMuscleCurrent;
		class VsMotorVelocityStimulus;
	}

	namespace Environment
	{
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
