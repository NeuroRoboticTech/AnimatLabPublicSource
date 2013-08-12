// VsLine.cpp: implementation of the VsLine class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsLine.h"
#include "VsBox.h"
#include "VsSimulator.h"


namespace VortexAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsLine::VsLine()
{
}

VsLine::~VsLine()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VsLine\r\n", "", -1, false, true);}
}

void VsLine::SetThisPointers()
{
	VsRigidBody::SetThisPointers();
    OsgLine::SetThisLinePointers();
}


void VsLine::SetupGraphics()
{
	//Add it to the root scene graph because the vertices are in global coords.
	GetVsSimulator()->OSGRoot()->addChild(m_osgNode.get());
	SetVisible(m_lpThisMI->IsVisible());
}

void VsLine::DeleteGraphics()
{
	if(m_osgGeometry.valid())
	{
		m_osgGeometry->setDataVariance(osg::Object::STATIC);
		m_osgGeometry->dirtyBound();
		SetVisible(false);
	}

	VsRigidBody::DeleteGraphics();
}

void VsLine::CreateGraphicsGeometry()
{
	fltA = 0;
	m_osgGeometry = CreateLineGeometry();
}

void VsLine::CreatePhysicsGeometry()
{
	m_vxGeometry = NULL;
}

void VsLine::CreateParts()
{
	CreateGeometry();

	VsRigidBody::CreateItem();
	VsRigidBody::SetBody();
}

void VsLine::StepSimulation(float fltTension)
{
    OsgLine::StepLineSimulation(m_lpThisBP->Enabled(), fltTension);
}

void VsLine::ResetSimulation()
{
	//We do nothing in the reset simulation because we need the attachment points to be reset before we can do anything.
}

void VsLine::AfterResetSimulation()
{
	DrawLine();
}


	}			// Visualization
}				//VortexAnimatSim
