// RbFirmataHingeServo.cpp: implementation of the RbFirmataHingeServo class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbHingeLimit.h"
#include "RbHinge.h"
#include "RbRigidBody.h"
#include "RbStructure.h"
#include "RbFirmataPart.h"
#include "RbFirmataHingeServo.h"
#include "RbFirmataController.h"

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{
			namespace Firmata
			{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbFirmataHingeServo::RbFirmataHingeServo() 
{
    m_lpJoint = NULL;
	m_iMaxPulse = 2400;
	m_iMinPulse = 544;
	m_bResetToStartPos = true;

	m_iMaxAngle = 179;
	m_iMinAngle  = 0;
	m_iCenterAngle = 90;

	m_fltMaxAngle = RB_PI/2;
	m_fltMinAngle = -RB_PI/2;

	m_fltPosFloatToFPSlope = (m_iMaxAngle-m_iMinAngle+1)/(m_fltMaxAngle-m_fltMinAngle);
	m_fltPosFloatToFPIntercept = m_iCenterAngle;

	m_fltPosFPToFloatSlope = 1/m_fltPosFloatToFPSlope;
	m_fltPosFPToFloatIntercept = -(m_fltPosFPToFloatSlope*m_iCenterAngle);

	m_iLastGoalPos = -1;
}

RbFirmataHingeServo::~RbFirmataHingeServo()
{
	try
	{
        //Do not delete because we do not own it.
        m_lpJoint = NULL;
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbDynamixelCM5USBUARTHingeController\r\n", "", -1, false, true);}
}

int RbFirmataHingeServo::MaxPulse() {return m_iMaxPulse;}

void RbFirmataHingeServo::MaxPulse(int iPulse)
{
	Std_IsAboveMin(m_iMinPulse, iPulse, true, "MaxPulse");
	m_iMaxPulse = iPulse;
}

int RbFirmataHingeServo::MinPulse() {return m_iMinPulse;}

void RbFirmataHingeServo::MinPulse(int iPulse)
{
	Std_IsAboveMin(0, iPulse, true, "MinPulse");
	Std_IsBelowMax(m_iMaxPulse, iPulse, true, "MinPulse");
	m_iMinPulse = iPulse;
}

bool RbFirmataHingeServo::ResetToStartPos() {return m_bResetToStartPos;}

void RbFirmataHingeServo::ResetToStartPos(bool bVal) {m_bResetToStartPos = bVal;}

float RbFirmataHingeServo::ConvertPosFPToRad(int iPos)
{
	float fltPos = (m_fltPosFPToFloatSlope*iPos) + m_fltPosFPToFloatIntercept;
	return fltPos;
}

int RbFirmataHingeServo::ConvertPosRadToFP(float fltPos)
{
	int iPos = (m_fltPosFloatToFPSlope*fltPos) + m_fltPosFloatToFPIntercept;
	return iPos;
}
/**
\brief	Sets the fixed point value of the servo goal position. This is the exact value that will be sent to the servo. 

\author	dcofer
\date	4/25/2014

\param	iPos	The new value fixed point position.
**/
void RbFirmataHingeServo::SetGoalPosition_FP(int iPos)
{
	//Verify we are not putting invalid values in.
	if(iPos < m_iMinAngle) iPos = m_iMinAngle;
	if(iPos > m_iMaxAngle) iPos = m_iMaxAngle;

	//If the position we are setting is the same as we just set then no point sending it.
	if(m_iLastGoalPos == iPos)
		return;

	m_iLastGoalPos = iPos;

	std::cout << "Servo: " << iPos << "\r\n";

	m_lpFirmata->sendServo(m_iIOComponentID, iPos, true);
}

/**
\brief	Sets the floating point value of the servo goal position. This is the position in radians. It will be converted to a fixed
point value based on the motor configuration and then used to set the position. This method assumes that center value is 0 and that
it uses +/- values on either side.

\author	dcofer
\date	4/25/2014

\param	fltPos	The new value position in radians.
**/
void RbFirmataHingeServo::SetGoalPosition(float fltPos)
{
	int iPos = ConvertPosRadToFP(fltPos);

	SetGoalPosition_FP(iPos);
}

#pragma region DataAccesMethods

float *RbFirmataHingeServo::GetDataPointer(const std::string &strDataType)
{
	//std::string strType = Std_CheckString(strDataType);

	//if(strType == "IOVALUE")
	//	return &m_fltIOValue;
	//else
		return RobotPartInterface::GetDataPointer(strDataType);
}

bool RbFirmataHingeServo::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(strType == "MAXPULSE")
	{
		MaxPulse((int) atoi(strValue.c_str()));
		return true;
	}

	if(strType == "MINPULSE")
	{
		MinPulse((int) atoi(strValue.c_str()));
		return true;
	}

	if(strType == "RESETTOSTARTPOS")
	{
		ResetToStartPos(Std_ToBool(strValue));
		return true;
	}

	return RobotPartInterface::SetData(strDataType, strValue, bThrowError);
}

void RbFirmataHingeServo::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RobotPartInterface::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("MaxPulse", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("MinPulse", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("ResetToStartPos", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));
}

#pragma endregion

void RbFirmataHingeServo::Initialize()
{
	RobotPartInterface::Initialize();

	m_lpJoint = dynamic_cast<MotorizedJoint *>(m_lpPart);
}

void RbFirmataHingeServo::SetupIO()
{
	if(!m_lpSim->InSimulation())
	{
		m_lpFirmata->sendServoAttach(m_iIOComponentID);

		if(m_bResetToStartPos)
		{
			SetGoalPosition_FP(m_iCenterAngle);
			m_iIOValue = m_iCenterAngle;
		}
	}
}

void RbFirmataHingeServo::StepIO(int iPartIdx)
{
	if(!m_lpSim->InSimulation() && m_iIOValue != m_iLastGoalPos)
		SetGoalPosition_FP(m_iIOValue);
}

void RbFirmataHingeServo::StepSimulation()
{
	RobotPartInterface::StepSimulation();

	if(m_lpJoint)
	{
		//Here we need to get the set velocity for this motor that is coming from the neural controller, and then make the real motor go that speed.
		float fltSetPosition = m_lpJoint->JointPosition();
		m_iIOValue = ConvertPosRadToFP(fltSetPosition);

		if(m_iIOValue > 179)
			m_iIOValue = m_iIOValue;

		m_fltIOValue = m_iIOValue;
	}
}

void RbFirmataHingeServo::Load(CStdXml &oXml)
{
	RobotPartInterface::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element
	MinPulse(oXml.GetChildInt("MinPulse", m_iMinPulse));
	MaxPulse(oXml.GetChildInt("MaxPulse", m_iMaxPulse));
	ResetToStartPos(oXml.GetChildBool("ResetToStartPos", m_bResetToStartPos));
	oXml.OutOfElem(); //OutOf RigidBody Element
}

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

