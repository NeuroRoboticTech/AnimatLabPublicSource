#include "StdAfx.h"
#include "OsgCameraManipulator.h"
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgRigidBody.h"
#include "OsgJoint.h"
#include "OsgMouseSpring.h"
#include "OsgStructure.h"
#include "OsgLight.h"
#include "OsgUserData.h"
#include "OsgSimulationWindow.h"
#include "OsgSimulationWindowMgr.h"
#include "OsgDragger.h"
#include "OsgSimulator.h"

namespace OsgAnimatSim
{
	namespace Visualization
	{

OsgSimulationWindowMgr::OsgSimulationWindowMgr(void)
{
}

OsgSimulationWindowMgr::~OsgSimulationWindowMgr(void)
{
}

void OsgSimulationWindowMgr::Initialize()
{
	SimulationWindowMgr::Initialize();

	int iCount = m_aryWindows.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryWindows[iIndex]->Initialize();
}

void OsgSimulationWindowMgr::Realize()
{
	SetupCameras();
//	m_osgViewer->realize();
//
//    osgViewer::Viewer::Windows windows;
//    m_osgViewer->getWindows(windows);
//	osgViewer::GraphicsWindow *win = windows[0];
//
//	if(win)
//	    win->setWindowName("Animat Simulator");
}

	}// end Visualization
}// end OsgAnimatSim