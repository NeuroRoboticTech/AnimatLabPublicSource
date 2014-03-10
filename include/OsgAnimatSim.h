#ifndef __OSG_ANIMAT_LIB_DLL_H__
#define __OSG_ANIMAT_LIB_DLL_H__

#ifndef _WIN64
	#ifdef _DEBUG
	    #pragma comment(lib, "OsgAnimatSim_vc10D.lib")
    #else
	    #pragma comment(lib, "OsgAnimatSim_vc10.lib")
	#endif      // _DEBUG  
#else
	#ifdef _DEBUG
	    #pragma comment(lib, "OsgAnimatSim_vc10D_x64.lib")
    #else
	    #pragma comment(lib, "OsgAnimatSim_vc10_x64.lib")
	#endif      // _DEBUG  
#endif          // _WIN64

#ifdef WIN32
	#define ANIMAT_OSG_PORT __declspec( dllimport )
#else
    #define ANIMAT_OSG_PORT
#endif

#include "StdUtils.h"
#include "AnimatSim.h"

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
#include <osg/AutoTransform>
#include <osg/StateAttribute>
#include <osg/AlphaFunc>
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

#include "OsgWorldCoordinateNodeVisitor.h"

using namespace osgGA;

#include <OpenThreads/Thread>

#ifdef WIN32
    #include <osgViewer/api/win32/GraphicsWindowWin32>
#endif

#include "OsgAnimatSimConstants.h"

//Simulation Objects
namespace OsgAnimatSim
{
	class OsgMeshMgr;
    class OsgMatrixUtils;
    class OsgSimulator;

	namespace Environment
	{
        class OsgLight;
        class OsgBody;
		class OsgJoint;
        class OsgRigidBody;
		class OsgOrganism;
		class OsgStructure;
        class OsgLine;

        namespace Bodies
        {
            class OsgAttachment;
            class OsgMouth;
            class OsgOdorSensor;
        }

        namespace Joints
        {
            class OsgHinge;
            class OsgHingeLimit;
            class OsgPrismatic;
            class OsgPrismaticLimit;
            class OsgFreeJoint;
        }
	}

	namespace Visualization
	{
		class OsgCameraManipulator;
		class OsgDragger;
		class OsgDraggerHandler;
		class OsgHud;
		class OsgHudItem;
		class OsgHudText;
		class OsgMouseSpring;
		class OsgUserData;
		class OsgUserDataVisitor;
		class OsgSimulationWindow;
		class OsgSimulationWindowMgr;
		class OsgTrackballDragger;
		class OsgTranslateAxisDragger;
		class OsgWorldCoordinateNodeVisitor;
        class OsgPyramid;
	}
}

using namespace OsgAnimatSim;
using namespace OsgAnimatSim::Environment;
using namespace OsgAnimatSim::Environment::Joints;
using namespace OsgAnimatSim::Environment::Bodies;
using namespace OsgAnimatSim::Visualization;

#include "OsgMatrixUtil.h"
#include "OsgMeshMgr.h"

#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgRigidBody.h"
#include "OsgJoint.h"
#include "OsgLight.h"
#include "OsgLine.h"
#include "OsgHinge.h"
#include "OsgHingeLimit.h"
#include "OsgPrismatic.h"
#include "OsgPrismaticLimit.h"
#include "OsgFreeJoint.h"
#include "OsgAttachment.h"
#include "OsgMouth.h"
#include "OsgOdorSensor.h"
#include "OsgPyramid.h"

#include "OsgOrganism.h"
#include "OsgStructure.h"
#include "OsgTrackballDragger.h"
#include "OsgTranslateAxisDragger.h"
#include "OsgWorldCoordinateNodeVisitor.h"
#include "OsgLinearPath.h"
#include "OsgHudText.h"
#include "OsgHud.h"
#include "OsgUserData.h"
#include "OsgUserDataVisitor.h"
#include "OsgDragger.h"
#include "OsgDraggerHandler.h"
#include "OsgMouseSpring.h"
#include "OsgCameraManipulator.h"
#include "OsgSimulationWindow.h"
#include "OsgScriptedSimulationWindow.h"
#include "OsgSimulationWindowMgr.h"
#include "OsgSimulator.h"

#endif // __OSG_ANIMAT_LIB_DLL_H__
