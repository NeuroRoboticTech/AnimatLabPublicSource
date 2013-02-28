// VsSpring.cpp: implementation of the VsSpring class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsLine.h"
#include "VsSpring.h"
#include "VsSimulator.h"
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

VsSpring::VsSpring()
{
	SetThisPointers();
	m_vxSpring = NULL;
}

VsSpring::~VsSpring()
{
	try
	{
		DeleteGraphics();
		DeletePhysics();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of VsSpring/\r\n", "", -1, FALSE, TRUE);}
}

void VsSpring::NaturalLength(float fltVal, BOOL bUseScaling)
{
	Spring::NaturalLength(fltVal, bUseScaling);
	if(m_vxSpring)
		m_vxSpring->setNaturalLength(m_fltNaturalLength);
}

void VsSpring::Stiffness(float fltVal, BOOL bUseScaling)
{
	Spring::Stiffness(fltVal, bUseScaling);
	if(m_vxSpring)
		m_vxSpring->setStiffness(m_fltStiffness);
}

void VsSpring::Damping(float fltVal, BOOL bUseScaling)
{
	Spring::Damping(fltVal, bUseScaling);
	if(m_vxSpring)
		m_vxSpring->setDamping(m_fltDamping);
}

void VsSpring::InitializeAttachments()
{
	Spring::InitializeAttachments();
	SetupPhysics();
}

void VsSpring::Physics_Resize()
{
	 VsRigidBody::Physics_Resize();
	 SetupPhysics();
}

void VsSpring::DeletePhysics()
{
	if(!m_vxSpring)
		return;

	if(GetVsSimulator() && GetVsSimulator()->Universe())
	{
		GetVsSimulator()->Universe()->removeConstraint(m_vxSpring);
		delete m_vxSpring;
	}

	m_vxSpring = NULL;
}

void VsSpring::SetupPhysics()
{
	if(m_vxSpring)
		DeletePhysics();

	if(m_aryAttachmentPoints.GetSize() == 2)
	{
		Attachment *lpPrimaryAttachment = m_aryAttachmentPoints[0];
		Attachment *lpSecondaryAttachment = m_aryAttachmentPoints[1];

		if(!lpPrimaryAttachment && !lpSecondaryAttachment)
		{
			Enabled(FALSE);
			return;
		}

		VsRigidBody *lpVsPrimary = dynamic_cast<VsRigidBody *>(lpPrimaryAttachment->Parent());
		VsRigidBody *lpVsSecondary = dynamic_cast<VsRigidBody *>(lpSecondaryAttachment->Parent());

		if(!lpVsPrimary && !lpVsSecondary)
		{
			Enabled(FALSE);
			return;
		}

		m_vxSpring = new AnimatVxSpring(lpVsPrimary->Part(), lpVsSecondary->Part(), m_fltNaturalLength, m_fltStiffness, m_fltDamping); // attached to the reference frame.

		CStdFPoint vPrimPos = lpPrimaryAttachment->AbsolutePosition();
		CStdFPoint vSecPos = lpSecondaryAttachment->AbsolutePosition();
		
	    m_vxSpring->setPartAttachmentPosition(0, vPrimPos.x, vPrimPos.y, vPrimPos.z);
	    m_vxSpring->setPartAttachmentPosition(1, vSecPos.x, vSecPos.y, vSecPos.z);

		m_vxSpring->enable(m_bEnabled);
		GetVsSimulator()->Universe()->addConstraint(m_vxSpring);
	}
	else
		Enabled(FALSE);
}

void VsSpring::CreateJoints()
{
	Spring::CreateJoints();
	VsLine::CreateParts();

	SetupPhysics();
}

void VsSpring::Enabled(BOOL bVal)
{
	Spring::Enabled(bVal);

	if(m_vxSpring)
	{
		m_vxSpring->enable(m_bEnabled);
		m_vxSpring->EnableBodies();
	}
}

void VsSpring::Physics_CollectData()
{
	if(m_vxSpring)
	{
		m_fltLength = CalculateLength();
		if(m_bEnabled)
			m_fltDisplacement = (m_fltLength - m_fltNaturalLengthNotScaled);
		else
			m_fltDisplacement = 0;

		m_fltTension = m_fltStiffnessNotScaled * m_fltDisplacement;
		m_fltEnergy = 0.5f*m_fltStiffnessNotScaled*m_fltDisplacement*m_fltDisplacement;
	}
}

void VsSpring::ResetSimulation()
{
	Spring::ResetSimulation();
	VsLine::ResetSimulation();
}

void VsSpring::AfterResetSimulation()
{
	Spring::AfterResetSimulation();
	VsLine::AfterResetSimulation();
}

void VsSpring::StepSimulation()
{
	Spring::StepSimulation();
	VsLine::DrawLine();
}


		}		//Joints
	}			// Environment
}				//VortexAnimatSim
