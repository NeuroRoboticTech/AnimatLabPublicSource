#include "StdAfx.h"
#include "VsMouseSpring.h"
#include "VsCameraManipulator.h"
#include "VsBody.h"
#include "VsRigidBody.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsSimulationWindow.h"
#include "VsSimulationWindowMgr.h"
#include "VsDragger.h"


namespace VortexAnimatSim
{
	namespace Visualization
	{

VsSimulationWindowMgr::VsSimulationWindowMgr(void)
{
}

VsSimulationWindowMgr::~VsSimulationWindowMgr(void)
{
}

void VsSimulationWindowMgr::Initialize(Simulator *lpSim)
{
	int iCount = m_aryWindows.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryWindows[iIndex]->Initialize(lpSim);
}

void VsSimulationWindowMgr::Realize()
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
}// end VortexAnimatSim