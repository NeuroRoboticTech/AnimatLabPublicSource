/**
\file	VsPrismatic.cpp

\brief	Implements the vs prismatic class.
**/

#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsPrismatic.h"
#include "VsSimulator.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

/**
\brief	Default constructor.

\author	dcofer
\date	3/31/2011
**/
VsPrismatic::VsPrismatic()
{
	m_lpThis = this;
	m_lpThisJoint = this;
	m_lpPhysicsBody = this;
	m_lpPhysicsMotorJoint = this;
	m_lpThisMotorJoint = this;
	m_vxPrismatic = NULL;
	m_fltConstraintLow = -0.5*VX_PI;
	m_fltConstraintHigh = 0.5*VX_PI;
	m_fltDistanceUnits = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/31/2011
**/
VsPrismatic::~VsPrismatic()
{

}
//
//void VsPrismatic::Selected(BOOL bValue, BOOL bSelectMultiple)  
//{
//	Prismatic::Selected(bValue, bSelectMultiple);
//	VsJoint::Selected(bValue, bSelectMultiple);
//}

//If this is a servo motor then the "velocity" signal is not really a velocity signal in this case. 
//It is the desired position and we must convert it to the velocity needed to reach and maintian that position.
//void VsPrismatic::CalculateServoVelocity()
//{
//	if(!m_vxPrismatic)
//		return;
//
//	float fltError = m_fltDesiredVelocity - m_fltPosition;
///*
//	if(m_bEnableLimits)
//	{
//		if(m_fltDesiredVelocity>m_fltConstraintHigh)
//			m_fltDesiredVelocity = m_fltConstraintHigh;
//		if(m_fltDesiredVelocity<m_fltConstraintLow)
//			m_fltDesiredVelocity = m_fltConstraintLow;
//
//		float fltProp = fltError / (m_fltConstraintHigh-m_fltConstraintLow);
//
//		m_fltDesiredVelocity = fltProp * m_ftlServoGain; 
//	}
//	else
//		m_fltDesiredVelocity = fltError * m_fltMaxVelocity; */
//}
//
//void VsPrismatic::SetVelocityToDesired()
//{
//	if(m_bEnableMotor)
//	{			
//		if(m_bServoMotor)
//			CalculateServoVelocity();
//
//		if(m_fltDesiredVelocity>m_fltMaxVelocity)
//			m_fltDesiredVelocity = m_fltMaxVelocity;
//
//		if(m_fltDesiredVelocity < -m_fltMaxVelocity)
//			m_fltDesiredVelocity = -m_fltMaxVelocity;
//
//		m_fltSetVelocity = m_fltDesiredVelocity;
//		m_fltDesiredVelocity = 0;
//
//		//Only do anything if the velocity value has changed
//		if( fabs(m_fltVelocity - m_fltSetVelocity) > 1e-4)
//		{
//			if(fabs(m_fltSetVelocity) > 1e-4)
//				VsJoint::SetVelocity(m_fltSetVelocity, m_fltMaxForce);
//			else
//				VsJoint::EnableLock(TRUE, m_fltPosition, m_fltMaxForce);
//		}
//		
//		m_fltPrevVelocity = m_fltSetVelocity;
//	}
//}
//
//void VsPrismatic::EnableMotor(BOOL bVal)
//{
//	VsJoint::EnableMotor(bVal, m_fltSetVelocity, m_fltMaxForce);
//	m_bEnableMotor = bVal;
//	m_fltPrevVelocity = -1000000;  //reset the prev velocity for the next usage
//}

void VsPrismatic::CreateJoint()
{
	if(!m_lpParent)
		THROW_ERROR(Al_Err_lParentNotDefined, Al_Err_strParentNotDefined);

	if(!m_lpChild)
		THROW_ERROR(Al_Err_lChildNotDefined, Al_Err_strChildNotDefined);

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpParent);
	if(!lpVsParent)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody);

	VsRigidBody *lpVsChild = dynamic_cast<VsRigidBody *>(m_lpChild);
	if(!lpVsChild)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody);
	VxAssembly *lpAssem = (VxAssembly *) m_lpStructure->Assembly();

	CStdFPoint vChildPos = lpVsChild->GetOSGWorldCoords();
	CStdFPoint vGlobal = vChildPos + m_lpThis->LocalPosition();
	CStdFPoint vLocalRot = m_lpThis->Rotation();

    VxVector3 pos((double) vGlobal.x, (double) vGlobal.y, (double)  vGlobal.z); 
    VxVector3 axis((double) vLocalRot.x, (double) vLocalRot.y, (double) vLocalRot.z); 
	m_vxPrismatic = new VxPrismatic(lpVsParent->Part(), lpVsChild->Part(), pos.v, axis.v); 

	//lpAssem->addConstraint(m_vxHinge);
	lpVsSim->Universe()->addConstraint(m_vxPrismatic);
	m_lpStructure->AddCollisionPair(m_lpParent->ID(), m_lpChild->ID());

	m_vxPrismatic->setLowerLimit(m_vxPrismatic->kLinearCoordinate, m_fltConstraintLow);
	m_vxPrismatic->setUpperLimit(m_vxPrismatic->kLinearCoordinate, m_fltConstraintHigh);
	//m_vxPrismatic->setLimitsActive(m_vxPrismatic->kLinearCoordinate, m_bEnableLimits);

	//m_vxPrismatic->setLowerLimit(m_vxPrismatic->kLinearCoordinate, m_fltConstraintLow, 0,  m_fltRestitution, m_fltStiffness, m_fltDamping);
	//m_vxPrismatic->setUpperLimit(m_vxPrismatic->kLinearCoordinate, m_fltConstraintHigh, 0, m_fltRestitution, m_fltStiffness, m_fltDamping);
	//m_vxPrismatic->setLimitsActive(m_vxPrismatic->kLinearCoordinate, m_bEnableLimits);	

    //m_vxPrismatic->setRelaxationParameters(m_vxPrismatic->kConstraintP1, m_fltStiffness, m_fltDamping, 0, true );
    //m_vxPrismatic->setRelaxationParameters(m_vxPrismatic->kConstraintP2, m_fltStiffness, m_fltDamping, 0, true );
    //m_vxPrismatic->setRelaxationParameters(m_vxPrismatic->kConstraintA0, m_fltStiffness, m_fltDamping, 0, true );
    //m_vxPrismatic->setRelaxationParameters(m_vxPrismatic->kConstraintA1, m_fltStiffness, m_fltDamping, 0, true );
    //m_vxPrismatic->setRelaxationParameters(m_vxPrismatic->kConstraintA2, m_fltStiffness, m_fltDamping, 0, true );

	m_vxJoint = m_vxPrismatic;
	m_iCoordID = m_vxPrismatic->kLinearCoordinate;

	//If the motor is enabled then it will start out with a velocity of	zero.
	if(m_bEnableMotor)
		EnableLock(TRUE, m_fltPosition, m_fltMaxForce);

	m_fltDistanceUnits = m_lpSim->DistanceUnits();
}


float *VsPrismatic::GetDataPointer(string strDataType)
{
	float *lpData=NULL;
	string strType = Std_CheckString(strDataType);

	if(strType == "JOINTROTATION")
		return &m_fltPosition;
	else if(strType == "JOINTPOSITION")
		return &m_fltPosition;
	else if(strType == "JOINTACTUALVELOCITY")
		return &m_fltVelocity;
	else if(strType == "JOINTFORCE")
		return &m_fltForce;
	else if(strType == "JOINTDESIREDVELOCITY")
		return &m_fltSetVelocity;
	else if(strType == "JOINTSETVELOCITY")
		return &m_fltSetVelocity;
	else if(strType == "ENABLE")
		return &m_fltEnabled;
	else if(strType == "CONTACTCOUNT")
		THROW_PARAM_ERROR(Al_Err_lMustBeContactBodyToGetCount, Al_Err_strMustBeContactBodyToGetCount, "JointID", m_strName);
	else
	{
		lpData = Prismatic::GetDataPointer(strDataType);
		if(lpData) return lpData;

		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "JointID: " + STR(m_strName) + "  DataType: " + strDataType);
	}

	return lpData;
}

void VsPrismatic::StepSimulation()
{
	UpdateData();
	SetVelocityToDesired();
}

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
