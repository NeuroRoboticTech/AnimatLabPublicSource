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
	m_lStartServoUpdateTick= 0;
	m_iModelNum = -1;
	m_iFirmwareVersion = -1;
	m_iReturnDelayTime = -1;
	m_iCCWAngleLimit = -1;
	m_iCWAngleLimit = -1;
	m_iTorqueLimit = -1;
	m_iIsMoving = -1;
	m_iLED = -1;
	m_iAlarm = -1;
	m_iLastGetRegisterID = 0;
	m_iLastGetRegisterValue = 0;
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

int RbFirmataDynamixelServo::ReadIsMoving(int iServoID)
{
	if(m_lpFirmata)
	{
		//If we have made the call then m_iIsMoving = -2, so do not make it again till we get a response.
		m_lpFirmata->sendDynamixelGetRegister(m_iServoID, P_MOVING, 1);
		bool bRet = m_lpFirmata->waitForSysExMessage(SYSEX_DYNAMIXEL_GET_REGISTER, 2);
	}

	return m_iIsMoving;
}

int RbFirmataDynamixelServo::ReadLED(int iServoID)
{
	if(m_lpFirmata)
	{
		m_lpFirmata->sendDynamixelGetRegister(m_iServoID, P_LED, 1);
		bool bRet = m_lpFirmata->waitForSysExMessage(SYSEX_DYNAMIXEL_GET_REGISTER, 2);
	}

	return m_iLED;
}

int RbFirmataDynamixelServo::ReadAlarmShutdown(int iServoID)
{
	if(m_lpFirmata)
	{
		m_lpFirmata->sendDynamixelGetRegister(m_iServoID, P_ALARM_SHUTDOWN, 1);
		bool bRet = m_lpFirmata->waitForSysExMessage(SYSEX_DYNAMIXEL_GET_REGISTER, 2);
	}

	return m_iAlarm;
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

void RbFirmataDynamixelServo::WriteCWComplianceMargin(int iServoID, int iVal)
{
	if(m_lpFirmata)
	{
		m_lpFirmata->sendDynamixelSetRegister(m_iServoID, P_CW_COMPLIANCE_MARGIN, 1, iVal);
		m_iCWComplianceMargin = iVal;
	}
}

void RbFirmataDynamixelServo::WriteCCWComplianceMargin(int iServoID, int iVal)
{
	if(m_lpFirmata)
	{
		m_lpFirmata->sendDynamixelSetRegister(m_iServoID, P_CCW_COMPLIANCE_MARGIN, 1, iVal);
		m_iCCWComplianceMargin = iVal;
	}
}

void RbFirmataDynamixelServo::WriteCWComplianceSlope(int iServoID, int iVal)
{
	if(m_lpFirmata)
	{
		m_lpFirmata->sendDynamixelSetRegister(m_iServoID, P_CW_COMPLIANCE_SLOPE, 1, iVal);
		m_iCWComplianceSlope = iVal;
	}
}

void RbFirmataDynamixelServo::WriteCCWComplianceSlope(int iServoID, int iVal)
{
	if(m_lpFirmata)
	{
		m_lpFirmata->sendDynamixelSetRegister(m_iServoID, P_CCW_COMPLIANCE_SLOPE, 1, iVal);
		m_iCCWComplianceSlope = iVal;
	}
}

void RbFirmataDynamixelServo::WriteMaxTorque(int iServoID, int iVal)
{
	if(m_lpFirmata)
	{
		m_lpFirmata->sendDynamixelSetRegister(m_iServoID, P_MAX_TORQUE_L, 2, iVal);
		m_iMaxTorque = iVal;
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

void RbFirmataDynamixelServo::WriteGoalPosition(int iServoID, int iPos) 
{
	if(m_lpFirmata)
	{
		m_lpFirmata->sendDynamixelSetRegister(m_iServoID, P_GOAL_POSITION_L, 2, iPos);
		m_iLastGoalPos = iPos;
	}
}

void RbFirmataDynamixelServo::WriteMovingSpeed(int iServoID, int iVelocity)
{
	if(m_lpFirmata)
	{
		m_lpFirmata->sendDynamixelSetRegister(m_iServoID, P_MOVING_SPEED_L, 2, iVelocity);
		m_iLastGoalVelocity = iVelocity;
	}
}
	
void RbFirmataDynamixelServo::SetRegister(unsigned char reg, unsigned char length, unsigned int value)
{
	m_lpFirmata->sendDynamixelSetRegister(m_iServoID, reg, length, value);
}

int RbFirmataDynamixelServo::GetRegister(unsigned char reg, unsigned char length)
{
	m_lpFirmata->sendDynamixelGetRegister(m_iServoID, reg, length);
	bool bRet = m_lpFirmata->waitForSysExMessage(SYSEX_DYNAMIXEL_GET_REGISTER, 2);
	if(bRet && m_iLastGetRegisterID == reg)
		return m_iLastGetRegisterValue;
	else
		return -1;
}

void RbFirmataDynamixelServo::Move(float fltPos, float fltVel)
{
	if(m_lpFirmata)
	{
		int iPos = ConvertPosFloatToFP(fltPos);
		int iVel = 0;
		
		if(fltVel >= 0)
			iVel = ConvertFloatVelocity(fltVel);

		m_lpFirmata->sendDynamixelMove(m_iServoID, iPos, iVel);
	}
}

void RbFirmataDynamixelServo::ConfigureServo()
{
	if(m_lpFirmata)
	{
		int iCWLimit = m_iMinSimPos;
		int iCCWLimit = m_iMaxSimPos;
		int iRetDelay = 1;

		m_lpFirmata->sendDynamixelConfigureServo(m_iServoID, iCWLimit, iCCWLimit, m_iMaxTorque, iRetDelay, 
											    m_iCWComplianceMargin, m_iCCWComplianceMargin, 
												m_iCWComplianceSlope, m_iCCWComplianceSlope);
	}
}

void RbFirmataDynamixelServo::ShutdownMotor()
{
	RbDynamixelServo::ShutdownMotor();

	//Make sure we detach the servo from the arbotix.
	if(m_lpFirmata && m_bQueryMotorData)
		m_lpFirmata->sendDynamixelServoDetach(m_iServoID);
}

void RbFirmataDynamixelServo::InitMotorData()
{
	if(m_lpFirmata)
	{
		//Hook up the dynamixel events to notify this servo object.
		//m_EDynamixelReceived = m_lpFirmata->EDynamixelKeyReceived.connect(boost::bind(&RbFirmataDynamixelServo::DynamixelRecieved, this, _1));
		m_EDynamixelTransmitError = m_lpFirmata->EDynamixelTransmitError.connect(boost::bind(&RbFirmataDynamixelServo::DynamixelTransmitError, this, _1, _2));
		m_EDynamixelGetRegister = m_lpFirmata->EDynamixelGetRegister.connect(boost::bind(&RbFirmataDynamixelServo::DynamixelGetRegister, this, _1, _2, _3));

		//If we are querying data from this motor then inform firmata it
		//needs to send back constant updates.
		if(m_bQueryMotorData)
			m_lpFirmata->sendDynamixelServoAttach(m_iServoID);

		RbDynamixelServo::InitMotorData();

		m_lStartServoUpdateTick = m_lpSim->GetTimerTick();
	}
}

void RbFirmataDynamixelServo::WaitForMoveToFinish() 
{
	//Reset the value for a new wait sequence
	m_iIsMoving = -1;

	if(m_lpFirmata)
	{
		m_lpFirmata->sendDynamixelStopped(m_iServoID);
		m_lpFirmata->waitForSysExMessage(SYSEX_DYNAMIXEL_STOPPED, 10);
	}
};

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
	if(m_lpFirmata && m_lpSim)
	{
		m_iPresentPos = m_lpFirmata->_dynamixelServos[m_iServoID]._actualPosition;
		m_iPresentVelocity = m_lpFirmata->_dynamixelServos[m_iServoID]._actualSpeed;
		
		m_fltPresentPos = ConvertPosFPToFloat(m_iPresentPos);
		m_fltPresentVelocity = ConvertFPVelocity(m_iPresentVelocity);

		m_lpFirmata->_dynamixelServos[m_iServoID]._keyChanged = false;

		//Calculate time between updates.
		unsigned long long lEndTick = m_lpSim->GetTimerTick();
		m_fltReadParamTime = m_lpSim->TimerDiff_m(m_lStartServoUpdateTick, lEndTick); 
		if(m_fltReadParamTime <= 0) m_fltReadParamTime = 0;
		m_lStartServoUpdateTick = lEndTick;
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


void RbFirmataDynamixelServo::DynamixelRecieved(const int &iServoID) 
{
	if(iServoID == 1 && m_iServoID == 1)
	{
		//std::cout << "Servo: " << iServoID << ", Pos: " <<  m_iPresentPos << ", Vel: " <<  m_iPresentVelocity << "\r\n";
		//std::cout << "Servo: " << iServoID << ", Pos: " <<  m_lpFirmata->_dynamixelServos[m_iServoID]._actualPosition << ", Vel: " <<  m_lpFirmata->_dynamixelServos[m_iServoID]._actualSpeed << "\r\n";
	}
}

void RbFirmataDynamixelServo::DynamixelTransmitError(const int &iCmd, const int &iServoID) 
{
	if(iServoID == m_iServoID)
	{
		switch(iCmd)
		{
		case SYSEX_DYNAMIXEL_STOP:
			Stop();
			break;
		case SYSEX_DYNAMIXEL_CONFIGURE_SERVO:
			ConfigureServo();
			break;
		}

		std::cout << "Transmit error Cmd: " << iCmd << ", servo: " << iServoID << "\r\n";
	}
}

void RbFirmataDynamixelServo::DynamixelGetRegister(const unsigned char &iServoID, const unsigned char &iReg, const unsigned int &iValue)
{
	if(iServoID == m_iServoID)
	{
		m_iLastGetRegisterID = iReg;
		m_iLastGetRegisterValue = iValue;

		//std::cout << "Get Register Servo: " << iServoID << ", Reg: " << iReg << ", Value: " << iValue << "\r\n";

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
		case P_MOVING:
			m_iIsMoving = iValue;
			break;
		case P_LED:
			m_iLED = iValue;
			break;
		case P_ALARM_SHUTDOWN:
			m_iAlarm = iValue;
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

///For the Firmata dynamixel we do not need to try and set the current position. It is more
///efficient to use the stop command built into the ArbotixFirmata sketch.
void RbFirmataDynamixelServo::Stop()
{
	if(!m_lpSim->InSimulation() && m_lpFirmata)
	{
		m_lpFirmata->sendDynamixelStop(m_iServoID);

		//Set the next and goal pos to the current pos so we do not
		//attempt to resend a servo command.
		m_iNextGoalPos = m_iPresentPos;
		m_iLastGoalPos = m_iPresentPos;

		//Do a similar thing with the velocity values so it thinks it is at 0 velocity.
		m_iNextGoalVelocity = -1;
		m_iLastGoalVelocity = 1;
	}
	else
		RbDynamixelServo::Stop();
}

void RbFirmataDynamixelServo::Initialize()
{
	RbFirmataPart::Initialize();

	m_lpMotorJoint = dynamic_cast<MotorizedJoint *>(m_lpPart);

	GetLimitValues();

	RecalculateParams();
}

//We need to get some key data when we first do setup so that info is available 
//if queried
void RbFirmataDynamixelServo::SetupIO()
{
	if(!m_lpSim->InSimulation() && m_lpFirmata)
	{
		SetMinSimPos(m_fltLowLimit);
		SetMaxSimPos(m_fltHiLimit);
		InitMotorData();

		//Set the next goal positions to the current ones.
		m_iNextGoalPos = m_iLastGoalPos;

		m_iNextGoalVelocity = m_iLastGoalVelocity;
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
	RbFirmataPart::StepSimulation();
	RbDynamixelServo::StepSimulation();
}

void RbFirmataDynamixelServo::ResetSimulation()
{
	RbFirmataPart::ResetSimulation();
	RbDynamixelServo::ResetSimulation();
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

