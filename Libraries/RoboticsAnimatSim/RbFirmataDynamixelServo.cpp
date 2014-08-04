// RbFirmataDynamixelServo.cpp: implementation of the RbFirmataDynamixelServo class.
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
#include "RbFirmataDynamixelServo.h"
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

RbFirmataDynamixelServo::RbFirmataDynamixelServo() 
{
}

RbFirmataDynamixelServo::~RbFirmataDynamixelServo()
{
}

float RbFirmataDynamixelServo::QuantizeServoPosition(float fltPos)
{
	return RbDynamixelServo::QuantizeServoPosition(fltPos);
}

float RbFirmataDynamixelServo::QuantizeServoVelocity(float fltVel)
{
	return RbDynamixelServo::QuantizeServoVelocity(fltVel);
}

void RbFirmataDynamixelServo::IOComponentID(int iID)
{
	Std_IsAboveMin((int) 0, iID, true, "ServoID");
	RbFirmataPart::IOComponentID(iID);
	RbDynamixelServo::ServoID(iID);
}

void RbFirmataDynamixelServo::InitMotorData()
{
	if(m_lpFirmata)
	{
		//If we are querying data from this motor then inform firmata it
		//needs to send back constant updates.
		if(m_bQueryMotorData)
			m_lpFirmata->sendDynamixelServoAttach(m_iServoID);

		//Hook up the dynamixel events to notify this servo object.
		//m_EDynamixelReceived = m_lpFirmata->EDynamixelReceived.connect(boost::bind(&RbFirmataDynamixelServo::DynamixelRecieved, this, _1));
		m_EDynamixelTransmitError = m_lpFirmata->EDynamixelTransmitError.connect(boost::bind(&RbFirmataDynamixelServo::DynamixelTransmitError, this, _1, _2));
	}
}
#pragma region DataAccesMethods

float *RbFirmataDynamixelServo::GetDataPointer(const std::string &strDataType)
{
	float *fltVal = RbDynamixelServo::GetDataPointer(strDataType);
	if(fltVal)
		return fltVal;
	else
		return RbFirmataPart::GetDataPointer(strDataType);
}

bool RbFirmataDynamixelServo::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(RbFirmataPart::SetData(strDataType, strValue, false))
		return true;

	if(RbDynamixelServo::SetData(strDataType, strValue))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RbFirmataDynamixelServo::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RbFirmataPart::QueryProperties(aryProperties);
	RbDynamixelServo::QueryProperties(aryProperties);
}

#pragma endregion

void RbFirmataDynamixelServo::UpdateMotorData()
{	
	if(!m_lpSim->InSimulation() && m_bQueryMotorData && m_lpFirmata)
	{
		if(m_lpFirmata->_dynamixelServos[m_iServoID]._allChanged)
			UpdateAllMotorData();
		else if(m_lpFirmata->_dynamixelServos[m_iServoID]._keyChanged)
			UpdateKeyMotorData();
	}
}

void RbFirmataDynamixelServo::UpdateKeyMotorData()
{
	m_iPresentPos = m_lpFirmata->_dynamixelServos[m_iServoID]._actualPosition;
	m_iPresentVelocity = m_lpFirmata->_dynamixelServos[m_iServoID]._actualSpeed;
		
	m_fltPresentPos = ConvertPosFPToRad(m_iPresentPos);
	m_fltPresentVelocity = ConvertFPVelocity(m_iPresentVelocity);

	m_lpFirmata->_dynamixelServos[m_iServoID]._keyChanged = false;
}

void RbFirmataDynamixelServo::UpdateAllMotorData()
{
	UpdateKeyMotorData();

	m_iLoad = m_lpFirmata->_dynamixelServos[m_iServoID]._load;
	m_iVoltage = m_lpFirmata->_dynamixelServos[m_iServoID]._voltage;
	m_iTemperature = m_lpFirmata->_dynamixelServos[m_iServoID]._temperature;

	m_fltLoad = ConvertFPLoad(m_iLoad);
	m_fltVoltage = m_iVoltage/100.0;
	m_fltTemperature = (float) m_iTemperature;

	m_lpFirmata->_dynamixelServos[m_iServoID]._allChanged = false;
}

//
//void RbFirmataDynamixelServo::DynamixelRecieved(const int &iServoID) 
//{
//	if(iServoID == m_iServoID)
//	{
//
//	}
//}

void RbFirmataDynamixelServo::DynamixelTransmitError(const int &iCmd, const int &iServoID) 
{
	if(iServoID == m_iServoID)
	{
		std::cout << "Transmit error Cmd: " << iCmd << ", servo: " << iServoID << "\r\n";
	}
}

void RbFirmataDynamixelServo::AddMotorUpdate(int iPos, int iSpeed)
{
	if(!m_lpSim->InSimulation() && m_lpFirmata)
		m_lpFirmata->sendDynamixelSynchMoveAdd(m_iServoID, iPos, iSpeed);

	m_fltIOValue = iSpeed;
}

void RbFirmataDynamixelServo::StepIO(int iPartIdx)
{	
	unsigned long long lStepStartTick = m_lpSim->GetTimerTick();

	UpdateMotorData();

	SetMotorPosVel();

	unsigned long long lEndStartTick = m_lpSim->GetTimerTick();
	m_fltStepIODuration = m_lpSim->TimerDiff_m(lStepStartTick, lEndStartTick); 
}

void RbFirmataDynamixelServo::ShutdownIO()
{
	ShutdownMotor();
}

void RbFirmataDynamixelServo::StepSimulation()
{
	if(!m_lpSim->InSimulation())
	{
		RbFirmataPart::StepSimulation();
		RbDynamixelServo::StepSimulation();
	}
}

void RbFirmataDynamixelServo::ResetSimulation()
{
	RbFirmataPart::ResetSimulation();
	m_fltReadParamTime = 0;
}

void RbFirmataDynamixelServo::Load(StdUtils::CStdXml &oXml)
{
	RbFirmataPart::Load(oXml);
	RbDynamixelServo::Load(oXml);
}

void RbFirmataDynamixelServo::MicroSleep(unsigned int iTime)
{
	if(m_lpSim)
	m_lpSim->MicroSleep(iTime);
}

Simulator *RbFirmataDynamixelServo::GetSimulator()
{
	return m_lpSim;
}

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

