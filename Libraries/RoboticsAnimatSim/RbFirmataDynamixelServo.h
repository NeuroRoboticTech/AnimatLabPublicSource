// RbFirmataDynamixelServo.h: interface for the RbFirmataDynamixelServo class.
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
			namespace Firmata
			{

class ROBOTICS_PORT RbFirmataDynamixelServo : public RbDynamixelServo, public RbFirmataPart
{
protected:
	///The tick when we start a servo update
	unsigned long long m_lStartServoUpdateTick;

	///Keeps track of the servos model number. This is updated once at the start of the sim and then used throughout
	int m_iModelNum;

	///Keeps track of the servos firmware version. This is updated once at the start of the sim and then used throughout
	int m_iFirmwareVersion;

	///Keeps track of the servos return delay time.
	int m_iReturnDelayTime;

	///Keeps track of the counter-clockwise limit.
	int m_iCCWAngleLimit;

	///Keeps track of the clockwise limit.
	int m_iCWAngleLimit;

	///Keeps track of the torque limit.
	int m_iTorqueLimit;

	///Keeps track of whether this servo is moving
	int m_iIsMoving;

	///Keeps track of LED Status
	int m_iLED;

	///Keeps track of alarm Status
	int m_iAlarm;

	//Stores the last reg id returned by a get register callback
	unsigned int m_iLastGetRegisterID;

	//Stores the last value returned by a get register callback
	unsigned int m_iLastGetRegisterValue;

	virtual void WriteGoalPosition(int iServoID, int iPos);
	virtual int ReadGoalPosition(int iServoID)  {return m_iLastGoalPos;};
	virtual int ReadPresentPosition(int iServoID)  {return m_iPresentPos;};

	virtual void WriteMovingSpeed(int iServoID, int iVelocity);
	virtual int ReadMovingSpeed(int iServoID)  {return m_iLastGoalVelocity;};
	virtual int ReadPresentSpeed(int iServoID)  {return m_iPresentVelocity;};

	virtual int ReadPresentLoad(int iServoID)  {return m_iLoad;};
	virtual int ReadPresentVoltage(int iServoID)  {return m_iVoltage;};
	virtual int ReadPresentTemperature(int iServoID)  {return m_iTemperature;};
	virtual int ReadIsMoving(int iServoID);
	virtual int ReadLED(int iServoID);
	virtual int ReadAlarmShutdown(int iServoID);
	virtual int ReadModelNumber(int iServoID)   {return m_iModelNum;};
	virtual int ReadID(int iServoID)  {return m_iServoID;};
	virtual int ReadFirmwareVersion(int iServoID)  {return m_iFirmwareVersion;};

	virtual void WriteReturnDelayTime(int iServoID, int iVal);
	virtual int ReadReturnDelayTime(int iServoID)  {return m_iReturnDelayTime;};

	virtual void WriteCCWAngleLimit(int iServoID, int iVal);
	virtual int ReadCCWAngleLimit(int iServoID)  {return m_iCCWAngleLimit;};

	virtual void WriteCWAngleLimit(int iServoID, int iVal);
	virtual int ReadCWAngleLimit(int iServoID)  {return m_iCWAngleLimit;};

	virtual void WriteTorqueLimit(int iServoID, int iVal);
	virtual int ReadTorqueLimit(int iServoID)  {return m_iTorqueLimit;};

	boost::signals2::connection m_EDynamixelReceived;
	boost::signals2::connection m_EDynamixelTransmitError;
	boost::signals2::connection m_EDynamixelGetRegister;

	virtual void DynamixelRecieved(const int &iServoID);
	virtual void DynamixelTransmitError(const int &iCmd, const int &iServoID); 
	virtual void DynamixelGetRegister(const unsigned char &iServoID, const unsigned char &iReg, const unsigned int &iValue);

	virtual void AddMotorUpdate(int iPos, int iSpeed);
	virtual void UpdateMotorData();
	virtual void UpdateAllMotorData();
	virtual void UpdateKeyMotorData();
	
	virtual void SetRegister(unsigned char reg, unsigned char length, unsigned int value);
	virtual int GetRegister(unsigned char reg, unsigned char length);

	virtual void Move(float fltPos, float fltVel);
	
	virtual void ConfigureServo();

public:
	RbFirmataDynamixelServo();
	virtual ~RbFirmataDynamixelServo();
	
	virtual bool IsMotorControl() {return true;};

	virtual void IOComponentID(int iID);

	virtual float QuantizeServoPosition(float fltPos);
	virtual float QuantizeServoVelocity(float fltVel);
	
	virtual void Stop();

	virtual void InitMotorData();
	virtual void ShutdownMotor();
	virtual void WaitForMoveToFinish();

#pragma region DataAccesMethods

	virtual float *GetDataPointer(const std::string &strDataType);
	virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
	virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

    virtual void Initialize();
    virtual void StepSimulation();
	virtual void ResetSimulation();
	virtual void Load(StdUtils::CStdXml &oXml);
	virtual void MicroSleep(unsigned int iTime);    
	virtual Simulator *GetSimulator();
	virtual void SetupIO();
	virtual void StepIO(int iPartIdx);
	virtual void ShutdownIO();

};

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

