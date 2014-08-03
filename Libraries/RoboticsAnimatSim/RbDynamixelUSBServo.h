// RbDynamixelUSBServo.h: interface for the RbDynamixelUSBServo class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

#include "RbDynamixelServo.h"

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{
			namespace DynamixelUSB
			{

class ROBOTICS_PORT RbDynamixelUSBServo : public RbDynamixelServo
{
protected:
	virtual void WriteGoalPosition(int iServoID, int iPos) {dxl_write_word(iServoID, P_GOAL_POSITION_L, iPos );};
	virtual int ReadGoalPosition(int iServoID)  {return dxl_read_word(iServoID, P_GOAL_POSITION_L);};
	virtual int ReadPresentPosition(int iServoID)  {return dxl_read_word(iServoID, P_PRESENT_POSITION_L);};

	virtual void WriteMovingSpeed(int iServoID, int iVelocity) {dxl_write_word(iServoID, P_MOVING_SPEED_L, iVelocity);};
	virtual int ReadMovingSpeed(int iServoID)  {return dxl_read_word(iServoID, P_MOVING_SPEED_L);};
	virtual int ReadPresentSpeed(int iServoID)  {return dxl_read_word(iServoID, P_PRESENT_SPEED_L);};

	virtual int ReadPresentLoad(int iServoID)  {return dxl_read_word(iServoID, P_PRESENT_LOAD_L);};
	virtual int ReadPresentVoltage(int iServoID)  {return dxl_read_byte(iServoID, P_PRESENT_VOLTAGE);};
	virtual int ReadPresentTemperature(int iServoID)  {return dxl_read_byte(iServoID, P_PRESENT_TEMPERATURE);};
	virtual int ReadIsMoving(int iServoID)  {return dxl_read_byte(iServoID, P_MOVING);};
	virtual int ReadLED(int iServoID)  {return dxl_read_byte(iServoID, P_LED);};
	virtual int ReadAlarmShutdown(int iServoID)  {return dxl_read_byte(iServoID, P_ALARM_SHUTDOWN);};
	virtual int ReadModelNumber(int iServoID)   {return dxl_read_word(iServoID, P_MODEL_NUMBER_L);};
	virtual int ReadID(int iServoID)  {return dxl_read_byte(iServoID, P_ID);};
	virtual int ReadFirmwareVersion(int iServoID)  {return dxl_read_byte(iServoID, P_FIRMWARE_VERSION);};

	virtual void WriteReturnDelayTime(int iServoID, int iVal) {dxl_write_byte(iServoID, P_RETURN_DELAY_TIME, iVal);};
	virtual int ReadReturnDelayTime(int iServoID)  {return dxl_read_byte(iServoID, P_RETURN_DELAY_TIME);};

	virtual void WriteCCWAngleLimit(int iServoID, int iVal) {dxl_write_word(iServoID, P_CCW_ANGLE_LIMIT_L, iVal);};
	virtual int ReadCCWAngleLimit(int iServoID)  {return dxl_read_word(iServoID, P_CCW_ANGLE_LIMIT_L);};

	virtual void WriteCWAngleLimit(int iServoID, int iVal) {dxl_write_word(iServoID, P_CW_ANGLE_LIMIT_L, iVal);};
	virtual int ReadCWAngleLimit(int iServoID)  {return dxl_read_word(iServoID, P_CW_ANGLE_LIMIT_L);};

	virtual void WriteTorqueLimit(int iServoID, int iVal) {dxl_write_word(iServoID, P_MAX_TORQUE_L, iVal);};
	virtual int ReadTorqueLimit(int iServoID)  {return dxl_read_word(iServoID, P_MAX_TORQUE_L);};

public:
	RbDynamixelUSBServo();
	virtual ~RbDynamixelUSBServo();
	
	virtual bool dxl_read_block( int id, int address, int length, std::vector<int> &aryData);
	virtual void ReadAllParams();
	virtual void ReadKeyParams();

	virtual std::string GetErrorCode();
	virtual std::string GetCommStatus(int CommStatus);
};

			}	//DynamixelUSB
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

