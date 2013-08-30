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
		DeletePhysics();
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
        m_fltMass = m_fltDensity * m_fltLength * m_fltWidth * m_fltHeight;
        m_btCollisionShape = new btBoxShape( btVector3( (m_fltLength/2.0f), (m_fltHeight/2.0f), (m_fltWidth/2.0f) ) );
    }
}

void BlBox::CreateParts()
{
	CreateGeometry();

	BlRigidBody::CreateItem();
	Box::CreateParts();
	BlRigidBody::SetBody();
}

void BlBox::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Box::CreateJoints();
	BlRigidBody::Initialize();
}

void BlBox::ResizePhysicsGeometry()
{
 //FIX PHYSICS
	//if(m_vxGeometry)
	//{
	//	VxBox *vxBox = dynamic_cast<VxBox *>(m_vxGeometry);

	//	if(!vxBox)
	//		THROW_TEXT_ERROR(Bl_Err_lGeometryMismatch, Bl_Err_strGeometryMismatch, m_lpThisAB->Name());
	//	
	//	vxBox->setDimensions(m_fltLength, m_fltHeight, m_fltWidth);
	//}
}


		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
