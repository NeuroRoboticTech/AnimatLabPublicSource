// VsSpring.cpp: implementation of the VsSpring class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
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
	m_vxSpring = NULL;
	m_lpLineBase = this;
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

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	if(m_aryAttachmentPoints.GetSize() == 2)
	{
		m_lpPrimaryAttachment = m_aryAttachmentPoints[0];
		m_lpSecondaryAttachment = m_aryAttachmentPoints[1];

		VsRigidBody *lpVsPrimary = dynamic_cast<VsRigidBody *>(m_lpPrimaryAttachment->Parent());
		VsRigidBody *lpVsSecondary = dynamic_cast<VsRigidBody *>(m_lpSecondaryAttachment->Parent());

		m_vxSpring = new VxSpring(lpVsPrimary->Part(), lpVsSecondary->Part(), m_fltNaturalLength, m_fltStiffness, m_fltDamping); // attached to the reference frame.

		CStdFPoint vPrimPos = m_lpPrimaryAttachment->AbsolutePosition();
		CStdFPoint vSecPos = m_lpSecondaryAttachment->AbsolutePosition();
		
	    m_vxSpring->setPartAttachmentPosition(0, vPrimPos.x, vPrimPos.y, vPrimPos.z);
	    m_vxSpring->setPartAttachmentPosition(1, vSecPos.x, vSecPos.y, vSecPos.z);

		m_vxSpring->enable(m_bEnabled);
		lpVsSim->Universe()->addConstraint(m_vxSpring);
	}
	else
		m_bEnabled = FALSE;

}

void VsSpring::Enabled(BOOL bVal)
{
	m_bEnabled = bVal;
	m_fltEnabled = (float) bVal;

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

float *VsSpring::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);
	float *lpData = NULL;

	if(strType == "SPRINGLENGTH")
		return &m_fltLength;

	if(strType == "DISPLACEMENT")
		return &m_fltDisplacement;

	if(strType == "TENSION")
		return &m_fltTension;

	if(strType == "ENERGY")
		return &m_fltEnergy;

	if(strType == "ENABLE")
		return &m_fltEnabled;

	lpData = Spring::GetDataPointer(strDataType);
	if(lpData) return lpData;

	THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "RigidBodyID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
