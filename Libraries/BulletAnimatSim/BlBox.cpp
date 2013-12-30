// BlBox.cpp: implementation of the BlBox class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "BlJoint.h"
#include "BlMotorizedJoint.h"
#include "BlRigidBody.h"
#include "BlBox.h"
#include "BlSimulator.h"

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

BlBox::BlBox()
{
	SetThisPointers();
}

BlBox::~BlBox()
{
	try
	{
		DeleteGraphics();
		DeletePhysics(false);
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of BlBox\r\n", "", -1, false, true);}
}

void BlBox::CreateGraphicsGeometry()
{
    m_osgGeometry = CreateBoxGeometry(Length(), Height(), Width(), LengthSegmentSize(), HeightSegmentSize(), WidthSegmentSize());
}

void BlBox::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
    {
        DeleteCollisionGeometry();

        CalculateVolumeAndAreas();
        m_eBodyType = BOX_SHAPE_PROXYTYPE;
        m_btCollisionShape = new btBoxShape( btVector3( (m_fltLength/2.0f), (m_fltHeight/2.0f), (m_fltWidth/2.0f) ) );
    }
}

void BlBox::CalculateVolumeAndAreas()
{
    m_fltVolume = m_fltLength * m_fltHeight * m_fltWidth;
    m_vArea.x = m_fltHeight * m_fltWidth;
    m_vArea.y = m_fltLength * m_fltWidth;
    m_vArea.z = m_fltLength * m_fltHeight;

    if(m_fltMass < 0)
    {
        float fltMass = m_fltVolume * m_fltDensity;
        Mass(fltMass, false, false);
    }
}

void BlBox::CreateParts()
{
	CreateGeometry();

	BlRigidBody::CreateItem();
	Box::CreateParts();
}

void BlBox::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Box::CreateJoints();
	BlRigidBody::Initialize();
}


		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
