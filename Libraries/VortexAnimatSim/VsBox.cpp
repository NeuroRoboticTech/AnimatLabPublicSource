// VsBox.cpp: implementation of the VsBox class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsBox.h"
#include "VsStructure.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"
#include "VsDragger.h"

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
}

void VsBox::CreateGraphicsGeometry()
{
	m_osgGeometry = CreateBoxGeometry(Length(), Height(), Width(), LengthSegmentSize(), HeightSegmentSize(), WidthSegmentSize());
}

void VsBox::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
		m_vxGeometry = new VxBox(m_fltLength, m_fltHeight, m_fltWidth);

	//Lets get the volume and areas
	m_fltVolume = m_fltLength * m_fltHeight * m_fltWidth;
	m_fltXArea = m_fltHeight * m_fltWidth;
	m_fltYArea = m_fltLength * m_fltWidth;
	m_fltZArea = m_fltHeight * m_fltLength;
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
