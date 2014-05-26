// RbDynamixelUSBServo.h: interface for the RbDynamixelUSBServo class.
//
//////////////////////////////////////////////////////////////////////

#pragma once


// Dynamixel Control table address
#define P_MODEL_NUMBER_L		0x00
#define P_MODEL_NUMBER_H		0x01
#define P_FIRMWARE_VERSION		0x02
#define P_ID					0x03
#define P_BAUD_RATE				0x04
#define P_RETURN_DELAY_TIME		0x05
#define P_CW_ANGLE_LIMIT_L		0x06
#define P_CW_ANGLE_LIMIT_H		0x07
#define P_CCW_ANGLE_LIMIT_L		0x08
#define P_CCW_ANGLE_LIMIT_H		0x09
#define P_INTERNAL_HIGHEST_TEMP	0x0B
#define P_LOWEST_LIMIT_VOLTAGE	0x0C
#define P_HIGHEST_LIMIT_VOLTAGE	0x0D
#define P_MAX_TORQUE_L			0x0E
#define P_MAX_TORQUE_H			0x0F
#define P_STATUS_RETURN_LEVEL	0x10
#define P_ALARM_LED				0x11
#define P_ALARM_SHUTDOWN		0x12
#define P_TORQUE_ENABLE			0x18
#define P_LED					0x19
#define P_CW_COMPLIANCE_MARGIN	0x1A
#define P_CCW_COMPLIANCE_MARGIN	0x1B
#define P_CW_COMPLIANCE_SLOPE	0x1C
#define P_CCW_COMPLIANCE_SLOPE	0x1D
#define P_GOAL_POSITION_L		0x1E
#define P_GOAL_POSITION_H		0x1F
#define P_MOVING_SPEED_L		0x20
#define P_MOVING_SPEED_H		0x21
#define P_TORQUE_LIMIT_L		0x22
#define P_TORQUE_LIMIT_H		0x23
#define P_PRESENT_POSITION_L	0x24
#define P_PRESENT_POSITION_H	0x25
#define P_PRESENT_SPEED_L		0x26
#define P_PRESENT_SPEED_H		0x27
#define P_PRESENT_LOAD_L		0x28
#define P_PRESENT_LOAD_H		0x29
#define P_PRESENT_VOLTAGE		0x2A
#define P_PRESENT_TEMPERATURE	0x2B
#define P_REGISTERED			0x2C
#define P_MOVING				0x2E
#define P_LOCK					0x2F
#define P_PUNCH_L				0x30
#define P_PUNCH_H				0x31


namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{
			namespace DynamixelUSB
			{

class ROBOTICS_PORT RbDynamixelUSBServo
{
protected:
	///ID used for communications with this servo
	int m_iServoID;

	///Minimum value that can be set for the poistion
	int m_iMinPos; 

	///Maximum value that can be set for the position
	int m_iMaxPos; 
	int m_iMaxAngle;

	float m_fltMaxAngle;
	float m_fltMinPos;
	float m_fltMaxPos;

	///This is the minimum angle in fixed point that can be used for this servo as specified in the simulation.
	int m_iMinSimPos;

	///This is the maximum angle in fixed point that can be used for this servo as specified in the simulation.
	int m_iMaxSimPos;

	///This is the minimum angle in radians that can be used for this servo as specified in the simulation.
	float m_fltMinSimPos;

	///This is the maximum angle in radians that can be used for this servo as specified in the simulation.
	float m_fltMaxSimPos;

	///The conversion factor to convert radians to FP position.
	float m_fltPosFPToRadSlope;
	float m_fltPosFPToRadIntercept;

	float m_fltPosRadToFPSlope;
	float m_fltPosRadToFPIntercept;

	///The center point value in fixed point numbers
	int m_iCenterPosFP;

	//The center point value in radians.
	float m_fltCenterPos;

	//Keeps track of the last servo goal position that we set
	int m_iLastGoalPos;

	//This is the goal position that you would like to use in the next time step.
	int m_iNextGoalPos;

	///Minimum value that can be set for the velocity
	int m_iMinVelocity; 

	///Maximum value that can be set for the velocity
	int m_iMaxVelocity; 

	///The conversion factor to convert rad/s to FP velocity.
	float m_fltConvertRadSToFP;

	///The conversion factor to convert FP velocity value to rad/s.
	float m_fltConvertFPToRadS;

	//Keeps track of the last servo goal velocity that we set
	int m_iLastGoalVelocity;

	//This is the goal velocity that you would like to use in the next time step.
	int m_iNextGoalVelocity;

	///Minimum value that can be set for the load
	int m_iMinLoad; 

	///Maximum value that can be set for the load
	int m_iMaxLoad; 

	//Used to conver the load fixed point value back to a percentage of load
	float m_fltConvertFPToLoad;

	//The current position that was last read in for this servo.
	int m_iPresentPos;

	//The current position that was last read in for this servo.
	float m_fltPresentPos;

	//The current velocity that was last read in for this servo.
	int m_iPresentVelocity;

	//The current velocity that was last read in for this servo.
	float m_fltPresentVelocity;

	//The current load that was last read in for this servo.
	int m_iLoad;

	//The current load that was last read in for this servo.
	float m_fltLoad;

	//The current voltage that was last read in for this servo.
	int m_iVoltage;

	//The current voltage that was last read in for this servo.
	float m_fltVoltage;

	//The current temperature that was last read in for this servo.
	int m_iTemperature;
	
	//The current temperature that was last read in for this servo.
	float m_fltTemperature;

public:
	RbDynamixelUSBServo();
	virtual ~RbDynamixelUSBServo();

	virtual float ConvertPosFPToRad(int iPos);
	virtual int ConvertPosRadToFP(float fltPos);

	virtual float ConvertFPVelocity(int iVel);
	virtual float ConvertFPLoad(int iLoad);

	virtual void ServoID(int iID);
	virtual int ServoID();

	virtual void SetGoalVelocity_FP(int iVelocity);
	virtual void SetNextGoalVelocity_FP(int iVelocity);
	virtual int GetGoalVelocity_FP();

	virtual void SetMaximumVelocity();
	virtual void SetNextMaximumVelocity();

	virtual void SetGoalVelocity(float fltVelocity);
	virtual void SetNextGoalVelocity(float fltVelocity);
	virtual float GetGoalVelocity();

	virtual void SetGoalPosition_FP(int iPos);
	virtual void SetNextGoalPosition_FP(int iPos);
	virtual int GetGoalPosition_FP();

	virtual void SetGoalPosition(float fltPos);
	virtual void SetNextGoalPosition(float fltPos);
	virtual float GetGoalPosition();

	virtual int LastGoalPosition_FP();

	virtual int GetActualPosition_FP();
	virtual float GetActualPosition();

	virtual int GetActualVelocity_FP();
	virtual float GetActualVelocity();

	virtual int LastGoalVelocity_FP();

	virtual int GetActualLoad_FP();
	virtual float GetActualLoad();

	virtual int GetActualVoltage_FP();
	virtual float GetActualVoltage();

	virtual float GetActualTemperatureFahrenheit();
	virtual float GetActualTemperatureCelcius();

	virtual float GetPresentPosition() {return m_fltPresentPos;}
	virtual float GetPresentVelocity() {return m_fltPresentVelocity;}
	virtual float GetPresentLoad() {return m_fltLoad;}
	virtual float GetPresentVoltage() {return m_fltVoltage;}
	virtual float GetPresentTemperature() {return m_fltTemperature;}

	virtual bool GetIsMoving();
	virtual void GetIsLEDOn(bool &bIsBlueOn, bool &bIsGreenOn, bool &bIsRedOn);
	virtual bool GetIsAlarmShutdown();

	virtual int GetModelNumber();
	virtual int GetIDNumber();
	virtual int GetFirmwareVersion();

	virtual void InitMotorData();
	virtual void ShutdownMotor();

	virtual bool dxl_read_block( int id, int address, int length, std::vector<int> &aryData);
	virtual void ReadAllParams();
	virtual void ReadKeyParams();

	virtual std::string GetErrorCode();
	virtual std::string GetCommStatus(int CommStatus);

	virtual void SetReturnDelayTime(int iVal);
	virtual int GetReturnDelayTime();

	virtual void SetCWAngleLimit_FP(int iVal);
	virtual void SetCWAngleLimit(float fltLimit);
	virtual int GetCWAngleLimit_FP();
	virtual float GetCWAngleLimit();

	virtual void SetCCWAngleLimit_FP(int iVal);
	virtual void SetCCWAngleLimit(float fltLimit);
	virtual int GetCCWAngleLimit_FP();
	virtual float GetCCWAngleLimit();

	virtual int GetMinSimPos_FP();
	virtual float GetMinSimPos();
	virtual void SetMinSimPos(float fltVal);

	virtual int GetMaxSimPos_FP();
	virtual float GetMaxSimPos();
	virtual void SetMaxSimPos(float fltVal);	
	
	virtual void MicroSleep(unsigned int iTime) = 0;
};

			}	//DynamixelUSB
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

