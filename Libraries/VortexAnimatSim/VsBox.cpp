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

void VsBox::CalculateEstimatedMassAndVolume()
{
    m_fltEstimatedVolume = (m_fltLength * m_fltHeight * m_fltWidth)*pow(m_lpSim->DistanceUnits(), (float) 3.0);;
    m_fltEstimatedMass = (m_fltDensity * m_fltLength * m_fltHeight * m_fltWidth) * m_lpSim->DisplayMassUnits();
}

void VsBox::CreatePhysicsGeometry()
{
	if(IsCollisionObject())
		m_vxGeometry = new VxBox(m_fltLength, m_fltHeight, m_fltWidth);

    CalculateEstimatedMassAndVolume();
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

        CalculateEstimatedMassAndVolume();
	}
}


		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
