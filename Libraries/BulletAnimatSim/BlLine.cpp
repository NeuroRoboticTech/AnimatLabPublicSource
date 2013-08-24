// BlLine.cpp: implementation of the BlLine class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlLine.h"
#include "BlBox.h"
#include "BlSimulator.h"


namespace BulletAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

BlLine::BlLine()
{
}

BlLine::~BlLine()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of BlLine\r\n", "", -1, false, true);}
}

void BlLine::SetThisPointers()
{
	BlRigidBody::SetThisPointers();
    OsgLine::SetThisLinePointers();
}


void BlLine::SetupGraphics()
{
	//Add it to the root scene graph because the vertices are in global coords.
	GetBlSimulator()->OSGRoot()->addChild(m_osgNode.get());
	SetVisible(m_lpThisMI->IsVisible());
}

void BlLine::DeleteGraphics()
{
	if(m_osgGeometry.valid())
	{
		m_osgGeometry->setDataVariance(osg::Object::STATIC);
		m_osgGeometry->dirtyBound();
		SetVisible(false);
	}

	BlRigidBody::DeleteGraphics();
}

void BlLine::CreateGraphicsGeometry()
{
	fltA = 0;
	m_osgGeometry = CreateLineGeometry();
}

void BlLine::CreatePhysicsGeometry()
{
    //FIX PHYSICS
	//m_vxGeometry = NULL;
}

void BlLine::CreateParts()
{
	CreateGeometry();

	BlRigidBody::CreateItem();
	BlRigidBody::SetBody();
}

void BlLine::StepSimulation(float fltTension)
{
    OsgLine::StepLineSimulation(m_lpThisBP->Enabled(), fltTension);
}

void BlLine::ResetSimulation()
{
	//We do nothing in the reset simulation because we need the attachment points to be reset before we can do anything.
}

void BlLine::AfterResetSimulation()
{
	DrawLine();
}


	}			// Visualization
}				//BulletAnimatSim
