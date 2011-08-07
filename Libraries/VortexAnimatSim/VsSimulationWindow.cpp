#include "stdafx.h"
#include "VsMouseSpring.h"
#include "VsCameraManipulator.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsStructure.h"
#include "VsLight.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsSimulationWindow.h"
#include "VsMouseSpring.h"
#include "VsDraggerHandler.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Visualization
	{

VsSimulationWindow::VsSimulationWindow()
{
	m_lpWinMgr = NULL;
	m_lpTrackBody = NULL;
}

VsSimulationWindow::~VsSimulationWindow(void)
{
	int i = 5;
}

osg::Matrix VsSimulationWindow::GetScreenMatrix()
{
	osg::Matrix matrix = m_osgViewer->getCamera()->getViewMatrix() * m_osgViewer->getCamera()->getProjectionMatrix();
	if(m_osgViewer->getCamera()->getViewport())
		matrix.postMult(m_osgViewer->getCamera()->getViewport()->computeWindowMatrix());
	
	return matrix;
}

osg::Viewport* VsSimulationWindow::GetViewport()
{
	return m_osgViewer->getCamera()->getViewport();
}

void VsSimulationWindow::Update()
{
	if(m_lpTrackBody)
		TrackCamera();

	m_osgViewer->frame(); 
}

void VsSimulationWindow::SetupTrackCamera()
{
	if(m_bTrackCamera)
	{
		m_lpTrackBody = NULL;

		if(!Std_IsBlank(m_strLookAtStructureID))
		{
			Structure *lpStructure = m_lpSim->FindStructureFromAll(m_strLookAtStructureID);

			VsBody *lpVsBody = NULL;
			BodyPart *lpBody=NULL;
			if(!Std_IsBlank(m_strLookAtBodyID))
				lpBody = dynamic_cast<BodyPart *>(lpStructure->FindNode(m_strLookAtBodyID));

			if(lpBody)
				SetCameraLookAt(lpBody->GetCurrentPosition());
			else
				SetCameraLookAt(lpStructure->Position());

			m_lpTrackBody = lpBody;
		} 
	}
	else
	{
		m_lpTrackBody = NULL;
		CStdFPoint vDefault(0, 0, 0);
		SetCameraLookAt(vDefault);
	}
}

void VsSimulationWindow::SetCameraLookAt(CStdFPoint oTarget)
{
	VxVector3 position(oTarget.x, oTarget.y, oTarget.z);

	osg::Vec3d eye, center, up;
	osg::Vec3d target(position.v[0], position.v[1], position.v[2]);
	osg::Vec3d pos(position.v[0]+50, position.v[1]+10, position.v[2]);
	m_osgManip->getHomePosition(eye, center, up);

	up = osg::Vec3d(0, 1, 0);
	m_osgManip->setHomePosition(pos, target, up, false );
	m_osgManip->home(0);
}

void VsSimulationWindow::TrackCamera()
{
	CStdFPoint oPos = m_lpTrackBody->AbsolutePosition();
	VxVector3 position(oPos.x, oPos.y, oPos.z);

	osg::Vec3 v(position[0], position[1], position[2]); 
	m_osgManip->setCenter(v);
}

void VsSimulationWindow::UpdateBackgroundColor() 
{
	CStdColor *vColor = m_lpSim->BackgroundColor();
	if(m_osgViewer.valid())
		m_osgViewer->getCamera()->setClearColor(osg::Vec4(vColor->r(), vColor->g(), vColor->b(), vColor->a()));
}

void VsSimulationWindow::InitEmbedded(Simulator *lpSim, VsSimulator *lpVsSim)
{
    m_osgViewer = new osgViewer::Viewer;

	osg::GraphicsContext::Traits *traits =	new osg::GraphicsContext::Traits();

	traits->inheritedWindowData = new osgViewer::GraphicsWindowWin32::WindowData( m_HWND );
	traits->setInheritedWindowPixelFormat = true;
	traits->doubleBuffer = true;
	traits->windowDecoration = false;
	traits->sharedContext = NULL;
	traits->supportsResize = true;

	RECT rect;
	GetWindowRect( m_HWND, &rect );
	traits->x = 0;
	traits->y = 0;
	traits->width = rect.right - rect.left;
	traits->height = rect.bottom - rect.top;

	osg::GraphicsContext *gc =	osg::GraphicsContext::createGraphicsContext(traits);

	osg::ref_ptr<osg::Camera> osgCamera = new osg::Camera;
	osgCamera->setGraphicsContext(gc);
	osg::ref_ptr<osg::Viewport> osgViewport = new osg::Viewport(traits->x, traits->y, traits->width, traits->height);
	osgCamera->setViewport(osgViewport.get());
	osgCamera->setDrawBuffer(GL_BACK);
	osgCamera->setReadBuffer(GL_BACK);

	CStdColor *vColor = lpSim->BackgroundColor();
	m_osgViewer->getCamera()->setClearColor(osg::Vec4(vColor->r(), vColor->g(), vColor->b(), vColor->a()));
	m_osgViewer->addSlave(osgCamera.get());


 //   //gc->setClearColor(osg::Vec4f(0.2f,0.2f,0.6f,1.0f));
 //   gc->setClearMask(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	//osg::Camera *cam = m_osgViewer->getCamera();
	//cam->setName("MyCam");
	//cam->setGraphicsContext( gc );
	//cam->setViewport( new osg::Viewport( traits->x, traits->y, traits->width, traits->height ) );
	//float *vColor = lpSim->BackgroundColor();
	//cam->setClearColor(osg::Vec4(vColor[0], vColor[1], vColor[2], vColor[3]));
	//m_osgViewer->setName("MyTest");

	m_osgViewer->setSceneData(lpVsSim->OSGRoot());

    //m_osgViewer->addEventHandler( new osgViewer::WindowSizeHandler );
    //m_osgViewer->addEventHandler( new osgViewer::ThreadingHandler );

	m_osgManip = new VsCameraManipulator(lpSim, m_osgViewer.get(), osgViewport.get());
	//m_osgManip = new osgGA::TrackballManipulator();
	m_osgViewer->setCameraManipulator(m_osgManip.get());

	osgGA::GUIEventHandler *lpHandler = new VsDraggerHandler(lpSim, m_osgViewer.get(), osgViewport.get());
    m_osgViewer->addEventHandler(lpHandler);

	m_osgViewer->realize();	

	// correct aspect ratio
	double fovy,aspectRatio,z1,z2;

	m_osgViewer->getCamera()->getProjectionMatrixAsPerspective(fovy,aspectRatio, z1,z2);
	aspectRatio=double(traits->width)/double(traits->height);
	m_osgViewer->getCamera()->setProjectionMatrixAsPerspective(fovy,aspectRatio,z1,z2);
}

void VsSimulationWindow::InitStandalone(Simulator *lpSim, VsSimulator *lpVsSim)
{

	m_osgViewer = new osgViewer::Viewer;

    //m_osgManip = new osgGA::TrackballManipulator;
	m_osgManip = new VsCameraManipulator(lpSim, m_osgViewer.get());
	m_osgViewer->setCameraManipulator(m_osgManip.get());

	osgGA::GUIEventHandler *lpHandler = new VsDraggerHandler(lpSim, m_osgViewer.get());
    m_osgViewer->addEventHandler(lpHandler);

    // Create the window and run the threads.
    m_osgViewer->setUpViewInWindow(m_ptPosition.x, m_ptPosition.y, m_ptSize.x, m_ptSize.y);

	CStdColor *vColor = lpSim->BackgroundColor();
	m_osgViewer->getCamera()->setClearColor(osg::Vec4(vColor->r(), vColor->g(), vColor->b(), vColor->a()));
	m_osgViewer->setSceneData(lpVsSim->OSGRoot());

	int inheritanceMask = 
	  (osgUtil::SceneView::VariablesMask::ALL_VARIABLES &
	  ~osgUtil::SceneView::VariablesMask::CULL_MASK);

	// set mask for upper camera
	m_osgViewer->getCamera()->setInheritanceMask(inheritanceMask);
	m_osgViewer->getCamera()->setCullMask(0x1);

	m_osgViewer->realize();

	// correct aspect ratio
	//double fovy,aspectRatio,z1,z2;

	//m_osgViewer->getCamera()->getProjectionMatrixAsPerspective(fovy,aspectRatio, z1,z2);
	//aspectRatio=double(traits->width)/double(traits->height);
	//m_osgViewer->getCamera()->setProjectionMatrixAsPerspective(fovy,aspectRatio,z1,z2);

	/*osgViewer::Viewer::Windows windows;
    m_osgViewer->getWindows(windows);
    m_osgViewer->stopThreading();
    windows[0]->setWindowName("AnimatSimulator");
    m_osgViewer->startThreading();*/
}

void VsSimulationWindow::Initialize()
{
	SimulationWindow::Initialize();

	m_lpWinMgr = dynamic_cast<VsSimulationWindowMgr *>(m_lpSim->WindowMgr());
	if(!m_lpWinMgr)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsWinMgr, Vs_Err_strUnableToConvertToVsWinMgr);

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	if(m_HWND)
		InitEmbedded(m_lpSim, lpVsSim);
	else
		InitStandalone(m_lpSim, lpVsSim);

	SetupTrackCamera();
}


void VsSimulationWindow::Close()
{
	//Stop threading so we can add this new window to the composite viewer.
	//m_osgViewer->setDone(true);
	m_osgViewer->stopThreading();

	m_lpTrackBody = NULL;  //Do not delete this item.
}

void VsSimulationWindow::OnGetFocus()
{
}

void VsSimulationWindow::OnLoseFocus()
{
}

	}// end Visualization
}// end VortexAnimatSim