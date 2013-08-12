// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#ifdef WIN32
    #define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
    // Windows Header Files:
    #include <windows.h>

    #define ANIMAT_OSG_PORT __declspec( dllexport )
#else
    #define ANIMAT_OSG_PORT
#endif

//#define STD_TRACING_ON

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
#include "OsgMatrixUtil.h"

//Simulation Objects
namespace OsgAnimatSim
{
	class OsgMeshMgr;
    class OsgMatrixUtil;
    class OsgSimulator;

	namespace Environment
	{
        class OsgLight;
        class OsgBody;
        class OsgRigidBody;
		class OsgJoint;
		class OsgOrganism;
		class OsgStructure;
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
	}
}

using namespace OsgAnimatSim;
using namespace OsgAnimatSim::Environment;
using namespace OsgAnimatSim::Visualization;
