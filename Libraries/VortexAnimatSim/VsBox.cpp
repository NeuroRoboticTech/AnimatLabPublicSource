// VsBox.cpp: implementation of the VsBox class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsBox.h"
#include "VsSimulator.h"

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsBox::VsBox()
{
	SetThisPointers();
}

VsBox::~VsBox()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsBox\r\n", "", -1, false, true);}
}

void VsBox::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateBoxGeometry(Length(), Height(), Width(), LengthSegmentSize(), HeightSegmentSize(), WidthSegmentSize());
}

void VsBox::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
		m_vxGeometry = new VxBox(m_fltLength, m_fltHeight, m_fltWidth);
}

void VsBox::CreateParts()
{
	CreateGeometry();

	VsRigidBody::CreateItem();
	Box::CreateParts();
	VsRigidBody::SetBody();
}

void VsBox::CreateJoints()
{
	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint();

	Box::CreateJoints();
	VsRigidBody::Initialize();
}

void VsBox::ResizePhysicsGeometry()
{
	if(m_vxGeometry)
	{
		VxBox *vxBox = dynamic_cast<VxBox *>(m_vxGeometry);

		if(!vxBox)
			THROW_TEXT_ERROR(Vs_Err_lGeometryMismatch, Vs_Err_strGeometryMismatch, m_lpThisAB->Name());
		
		vxBox->setDimensions(m_fltLength, m_fltHeight, m_fltWidth);
	}
}


		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
