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
	m_iModelNum = -1;
	m_iFirmwareVersion = -1;
	m_iReturnDelayTime = -1;
	m_iCCWAngleLimit = -1;
	m_iCWAngleLimit = -1;
	m_iTorqueLimit = -1;
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

void RbFirmataDynamixelServo::WriteReturnDelayTime(int iServoID, int iVal)
{
	if(m_lpFirmata)
	{
		m_lpFirmata->sendDynamixelSetRegister(m_iServoID, P_RETURN_DELAY_TIME, 1, iVal);
		m_iReturnDelayTime = iVal;
	}
}

void RbFirmataDynamixelServo::WriteCCWAngleLimit(int iServoID, int iVal)
{
	if(m_lpFirmata)
	{
		m_lpFirmata->sendDynamixelSetRegister(m_iServoID, P_CCW_ANGLE_LIMIT_L, 2, iVal);
		m_iCCWAngleLimit = iVal;
	}
}

void RbFirmataDynamixelServo::WriteCWAngleLimit(int iServoID, int iVal)
{
	if(m_lpFirmata)
	{
		m_lpFirmata->sendDynamixelSetRegister(m_iServoID, P_CW_ANGLE_LIMIT_L, 2, iVal);
		m_iCWAngleLimit = iVal;
	}
}

void RbFirmataDynamixelServo::WriteTorqueLimit(int iServoID, int iVal)
{
	if(m_lpFirmata)
	{
		m_lpFirmata->sendDynamixelSetRegister(m_iServoID, P_MAX_TORQUE_L, 2, iVal);
		m_iTorqueLimit = iVal;
	}
}

void RbFirmataDynamixelServo::InitMotorData()
{
	if(m_lpFirmata)
	{
		//Hook up the dynamixel events to notify this servo object.
		//m_EDynamixelReceived = m_lpFirmata->EDynamixelReceived.connect(boost::bind(&RbFirmataDynamixelServo::DynamixelRecieved, this, _1));
		m_EDynamixelTransmitError = m_lpFirmata->EDynamixelTransmitError.connect(boost::bind(&RbFirmataDynamixelServo::DynamixelTransmitError, this, _1, _2));
		m_EDynamixelGetRegister = m_lpFirmata->EDynamixelGetRegister.connect(boost::bind(&RbFirmataDynamixelServo::DynamixelGetRegister, this, _1, _2, _3));

		m_lpFirmata->sendDynamixelGetRegister(m_iServoID, P_MODEL_NUMBER_L, 2);
		m_lpFirmata->waitForSysExMessage(SYSEX_DYNAMIXEL_GET_REGISTER);
		m_lpFirmata->sendDynamixelGetRegister(m_iServoID, P_FIRMWARE_VERSION, 1);
		m_lpFirmata->waitForSysExMessage(SYSEX_DYNAMIXEL_GET_REGISTER);
		m_lpFirmata->sendDynamixelGetRegister(m_iServoID, P_RETURN_DELAY_TIME, 1);
		m_lpFirmata->waitForSysExMessage(SYSEX_DYNAMIXEL_GET_REGISTER);
		m_lpFirmata->sendDynamixelGetRegister(m_iServoID, P_CCW_ANGLE_LIMIT_L, 2);
		m_lpFirmata->waitForSysExMessage(SYSEX_DYNAMIXEL_GET_REGISTER);
		m_lpFirmata->sendDynamixelGetRegister(m_iServoID, P_CW_ANGLE_LIMIT_L, 2);
		m_lpFirmata->waitForSysExMessage(SYSEX_DYNAMIXEL_GET_REGISTER);
		m_lpFirmata->sendDynamixelGetRegister(m_iServoID, P_MAX_TORQUE_L, 2);
		m_lpFirmata->waitForSysExMessage(SYSEX_DYNAMIXEL_GET_REGISTER);

		//If we are querying data from this motor then inform firmata it
		//needs to send back constant updates.
		if(m_bQueryMotorData)
			m_lpFirmata->sendDynamixelServoAttach(m_iServoID);

		RbDynamixelServo::InitMotorData();
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
	if(m_lpFirmata)
	{
		m_iPresentPos = m_lpFirmata->_dynamixelServos[m_iServoID]._actualPosition;
		m_iPresentVelocity = m_lpFirmata->_dynamixelServos[m_iServoID]._actualSpeed;
		
		m_fltPresentPos = ConvertPosFPToRad(m_iPresentPos);
		m_fltPresentVelocity = ConvertFPVelocity(m_iPresentVelocity);

		m_lpFirmata->_dynamixelServos[m_iServoID]._keyChanged = false;
	}
}

void RbFirmataDynamixelServo::UpdateAllMotorData()
{
	UpdateKeyMotorData();

	if(m_lpFirmata)
	{
		m_iLoad = m_lpFirmata->_dynamixelServos[m_iServoID]._load;
		m_iVoltage = m_lpFirmata->_dynamixelServos[m_iServoID]._voltage;
		m_iTemperature = m_lpFirmata->_dynamixelServos[m_iServoID]._temperature;

		m_fltLoad = ConvertFPLoad(m_iLoad);
		m_fltVoltage = m_iVoltage/100.0;
		m_fltTemperature = (float) m_iTemperature;

		m_lpFirmata->_dynamixelServos[m_iServoID]._allChanged = false;
	}
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

void RbFirmataDynamixelServo::DynamixelGetRegister(const unsigned char &iServoID, const unsigned char &iReg, const unsigned int &iValue)
{
	if(iServoID == m_iServoID)
	{
		std::cout << "Get Register Servo: " << iServoID << ", Reg: " << iReg << ", Value: " << iValue << "\r\n";

		switch(iReg)
		{
		case P_MODEL_NUMBER_L:
			m_iModelNum = iValue;
			break;
		case P_FIRMWARE_VERSION:
			m_iFirmwareVersion = iValue;
			break;
		case P_RETURN_DELAY_TIME:
			m_iReturnDelayTime = iValue;
			break;
		case P_CW_ANGLE_LIMIT_L:
			m_iCWAngleLimit = iValue;
			break;
		case P_CCW_ANGLE_LIMIT_L:
			m_iCCWAngleLimit = iValue;
			break;
		case P_MAX_TORQUE_L:
			m_iTorqueLimit = iValue;
			break;
		}

	}
}

void RbFirmataDynamixelServo::AddMotorUpdate(int iPos, int iSpeed)
{
	if(!m_lpSim->InSimulation() && m_lpFirmata)
		m_lpFirmata->sendDynamixelSynchMoveAdd(m_iServoID, iPos, iSpeed);

	m_fltIOValue = iSpeed;
}

//We need to get some key data when we first do setup so that info is available 
//if queried
void RbFirmataDynamixelServo::SetupIO()
{
	if(!m_lpSim->InSimulation() && m_lpFirmata)
	{
		m_lpFirmata->sendDynamixelGetRegister(m_iServoID, P_CW_ANGLE_LIMIT_L, 2);
		MicroSleep(100);

	}
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

