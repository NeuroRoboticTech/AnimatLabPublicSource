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
	m_vxSpring = NULL;
}


void VsSpring::CreateParts()
{
	//We do nothing in createparts because we cannot build the line until after all parts are created
	//so we can get a handle to the attachment points.
}

void VsSpring::CreateJoints()
{
	Spring::CreateJoints();
	VsLine::CreateParts();

	if(m_aryAttachmentPoints.GetSize() == 2)
	{
		Attachment *lpPrimaryAttachment = m_aryAttachmentPoints[0];
		Attachment *lpSecondaryAttachment = m_aryAttachmentPoints[1];

		VsRigidBody *lpVsPrimary = dynamic_cast<VsRigidBody *>(lpPrimaryAttachment->Parent());
		VsRigidBody *lpVsSecondary = dynamic_cast<VsRigidBody *>(lpSecondaryAttachment->Parent());

		m_vxSpring = new VxSpring(lpVsPrimary->Part(), lpVsSecondary->Part(), m_fltNaturalLength, m_fltStiffness, m_fltDamping); // attached to the reference frame.

		CStdFPoint vPrimPos = lpPrimaryAttachment->AbsolutePosition();
		CStdFPoint vSecPos = lpSecondaryAttachment->AbsolutePosition();
		
	    m_vxSpring->setPartAttachmentPosition(0, vPrimPos.x, vPrimPos.y, vPrimPos.z);
	    m_vxSpring->setPartAttachmentPosition(1, vSecPos.x, vSecPos.y, vSecPos.z);

		m_vxSpring->enable(m_bEnabled);
		GetVsSimulator()->Universe()->addConstraint(m_vxSpring);
	}
	else
		m_bEnabled = FALSE;

}

void VsSpring::Enabled(BOOL bVal)
{
	Spring::Enabled(bVal);

	if(m_vxSpring)
		m_vxSpring->enable(m_bEnabled);
}

void VsSpring::Physics_CollectData()
{
	if(m_vxSpring)
	{
		m_fltLength = CalculateLength();
		m_fltDisplacement = (m_fltLength - m_fltNaturalLength) * m_lpSim->DistanceUnits();
		m_fltLength *= m_lpSim->DistanceUnits();

		m_fltEnergy = 0.5f*m_fltStiffness*m_fltDisplacement*m_fltDisplacement;

		Vx::VxReal3 vForce;
		m_vxSpring->getPartForce(0, vForce);
		m_fltTension = V3_MAG(vForce) * m_lpSim->MassUnits() * m_lpSim->DistanceUnits();
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
