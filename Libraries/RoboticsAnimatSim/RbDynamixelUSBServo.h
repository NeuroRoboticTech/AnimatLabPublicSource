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

class ROBOTICS_PORT RbDynamixelUSBServo  : public AnimatSim::Robotics::RobotPartInterface, public RbDynamixelServo
{
protected:
	RbDynamixelUSB *m_lpParentUSB;

	///The number update cycles till we update all params. Typically we only want to update
	///position and speed of the servo every time. We will only update the other params every now 
	///and then. This tells how often to do this.
	int m_iUpdateAllParamsCount;

	///Keeps track of the number of update loops that have occurred since we last updated all params.
	int m_iUpdateIdx;

	///Keeps track of when to update this motor data from the real motor. If it is is -1 then it will
	///update it every time. If it is set to some other positive value then it will only update it
	///when that cycle comes around. This is for updating the motors in a round robin fashion.
	int m_iUpdateQueueIndex;

	virtual void AddMotorUpdate(int iPos, int iSpeed);

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
	
	virtual bool IsMotorControl() {return true;};

	virtual void IOComponentID(int iID);

	virtual void UpdateAllParamsCount(int iVal);
	virtual int UpdateAllParamsCount();

	virtual void UpdateQueueIndex(int iVal);
	virtual int UpdateQueueIndex();

	virtual float QuantizeServoPosition(float fltPos);
	virtual float QuantizeServoVelocity(float fltVel);

	virtual bool IncludeInPartsCycle();

	virtual bool dxl_read_block( int id, int address, int length, std::vector<int> &aryData);
	virtual void ReadAllParams();
	virtual void ReadKeyParams();

	virtual std::string GetErrorCode();
	virtual std::string GetCommStatus(int CommStatus);

#pragma region DataAccesMethods

	virtual float *GetDataPointer(const std::string &strDataType);
	virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
	virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion
	
	virtual void SetupIO();
	virtual void StepIO(int iPartIdx);
	virtual void ShutdownIO();

	virtual void Initialize();
    virtual void StepSimulation();
	virtual void ResetSimulation();
	virtual void Load(StdUtils::CStdXml &oXml);
	virtual void MicroSleep(unsigned int iTime);    
	virtual Simulator *GetSimulator();
};

			}	//DynamixelUSB
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

