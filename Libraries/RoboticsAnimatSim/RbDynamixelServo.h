// RbDynamixelServo.h: interface for the RbDynamixelServo class.
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

class ROBOTICS_PORT RbDynamixelMotorUpdateData
{
public:
	int m_iID;
	int m_iGoalPos;
	int m_iGoalVelocity;

	RbDynamixelMotorUpdateData()
	{
		m_iID = 0;
		m_iGoalPos = 0;
		m_iGoalVelocity = 0;
	}

	RbDynamixelMotorUpdateData(int iID, int iGoalPos, int iGoalVelocity)
	{
		m_iID = iID;
		m_iGoalPos = iGoalPos;
		m_iGoalVelocity = iGoalVelocity;
	}

};

class ROBOTICS_PORT RbDynamixelServo
{
protected:
	///ID used for communications with this servo
	int m_iServoID;

	///Minimum value that can be set for the position in fixed point number.
	int m_iMinPosFP; 

	///Maximum value that can be set for the position in fixed point number.
	int m_iMaxPosFP; 

	///Maximum angle the servo can move in fixed point number. m_iMaxPosFP - m_iMinPosFP
	int m_iTotalAngle;

	///Maximum angle the servo can move in floating point number. m_iMaxPosFP - m_iMinPosFP
	float m_fltTotalAngle;

	///Minimum value that can be set for the position in floating point number.
	float m_fltMinAngle;

	///Maximum value that can be set for the position in floating point number.
	float m_fltMaxAngle;

	///This is the minimum angle in fixed point that can be used for this servo as specified in the simulation.
	int m_iMinSimPos;

	///This is the maximum angle in fixed point that can be used for this servo as specified in the simulation.
	int m_iMaxSimPos;

	///This is the minimum angle in radians that can be used for this servo as specified in the simulation.
	float m_fltMinSimPos;

	///This is the maximum angle in radians that can be used for this servo as specified in the simulation.
	float m_fltMaxSimPos;

	///The conversion factor to convert FP position to floating point position.
	float m_fltPosFPToFloatSlope;

	///The conversion factor to convert FP position to floating point position..
	float m_fltPosFPToFloatIntercept;

	///The conversion factor to convert radians to FP position.
	float m_fltPosFloatToFPSlope;

	///The conversion factor to convert radians to FP position.
	float m_fltPosFloatToFPIntercept;

	//true if a hinge joint, false otherwise.
	bool m_bIsHinge;

	///The total translation range over which a prismatic joint can move. This does not apply to hinge joints.
	float m_fltTranslationRange;

	///Conversion factor for when we are doing prismatic joints. Converts rotations to linear movements
	float m_fltFPToFloatTranslation;

	///Conversion factor for when we are doing prismatic joints. Converts rotations to linear movements
	float m_fltFloatToFPTranslation;

	///The center point value in fixed point numbers
	int m_iCenterPosFP;

	///The center point value in radians.
	float m_fltCenterPos;

	///Keeps track of the last servo goal position that we set
	int m_iLastGoalPos;

	///This is the goal position that you would like to use in the next time step.
	int m_iNextGoalPos;

	///Minimum value that can be set for the velocity
	int m_iMinVelocityFP; 

	///Maximum value that can be set for the velocity
	int m_iMaxVelocityFP; 

	///Stores the maximum rot/min for this motor. The max rotations (rad) per fixed point unit.
	float m_fltRPMPerFPUnit;

	///Stores the maximum rad/sec for this motor
	float m_fltMaxPosSec; 

	///The conversion factor to convert rad/s to FP velocity.
	float m_fltConvertPosSToFP;

	///The conversion factor to convert FP velocity value to rad/s.
	float m_fltConvertFPToPosS;

	///Keeps track of the last servo goal velocity that we set
	int m_iLastGoalVelocity;

	///This is the goal velocity that you would like to use in the next time step.
	int m_iNextGoalVelocity;

	///Minimum value that can be set for the load
	int m_iMinLoadFP; 

	///Maximum value that can be set for the load
	int m_iMaxLoadFP; 

	///Used to conver the load fixed point value back to a percentage of load
	float m_fltConvertFPToLoad;

	///The current position that was last read in for this servo.
	int m_iPresentPos;

	///The current position that was last read in for this servo.
	float m_fltPresentPos;

	///The current velocity that was last read in for this servo.
	int m_iPresentVelocity;

	///The current velocity that was last read in for this servo.
	float m_fltPresentVelocity;

	///The current load that was last read in for this servo.
	int m_iLoad;

	///The current load that was last read in for this servo.
	float m_fltLoad;

	///The current voltage that was last read in for this servo.
	int m_iVoltage;

	///The current voltage that was last read in for this servo.
	float m_fltVoltage;

	///The current temperature that was last read in for this servo.
	int m_iTemperature;
	
	///The current temperature that was last read in for this servo.
	float m_fltTemperature;

	///The time taken to read the params of this motor for the current step
	float m_fltReadParamTime;
	
	///Keeps track of whether we want to pull the data back from this motor or not. 
	///If we are only setting values in it then there is no reason to do an update for it.
	bool m_bQueryMotorData;
	
	///Hi constraint limit value
	float m_fltHiLimit;

	///Low constraint limit value
	float m_fltLowLimit;

	///Used to report back the IO position of the servo at each step
	float m_fltIOPos;

	///Used to report back the IO velocity of the servo at each step
	float m_fltIOVelocity;

	///Used to keep track of whether we need to set the stop position when doing
	///velocity control and the velocity is set to 0
	bool m_bNeedSetVelStopPos;

	///Keeps track of whether we have already set velocity to stop when doing velocity control
	bool m_bVelStopPosSet;

	///If true then we should reset to the start position at the beginning of the simulation.
	bool m_bResetToStartPos;

	///Pointer to an associated motorized joint.
    MotorizedJoint *m_lpMotorJoint;

	///Used to set the CW compliance margin of the servo
    int m_iCWComplianceMargin;

	///Used to set the CCW compliance margin of the servo
	int m_iCCWComplianceMargin;

	///Used to set the CW compliance slope of the servo
    int m_iCWComplianceSlope;

	///Used to set the CCW compliance slope of the servo
    int m_iCCWComplianceSlope;

	///Used to set the maximum torque of the servo
    int m_iMaxTorque;

	virtual void RecalculateParams();

	virtual void WriteGoalPosition(int iServoID, int iPos) = 0;
	virtual int ReadGoalPosition(int iServoID) = 0;
	virtual int ReadPresentPosition(int iServoID) = 0;

	virtual void WriteMovingSpeed(int iServoID, int iVelocity) = 0;
	virtual int ReadMovingSpeed(int iServoID) = 0;
	virtual int ReadPresentSpeed(int iServoID) = 0;

	virtual int ReadPresentLoad(int iServoID) = 0;
	virtual int ReadPresentVoltage(int iServoID) = 0;
	virtual int ReadPresentTemperature(int iServoID) = 0;
	virtual int ReadIsMoving(int iServoID) = 0;
	virtual int ReadLED(int iServoID) = 0;
	virtual int ReadAlarmShutdown(int iServoID) = 0;
	virtual int ReadModelNumber(int iServoID) = 0;
	virtual int ReadID(int iServoID) = 0;
	virtual int ReadFirmwareVersion(int iServoID) = 0;

	virtual void WriteCWComplianceMargin(int iServoID, int iVal) = 0;
	virtual int ReadCWComplianceMargin(int iServoID) = 0;

	virtual void WriteCCWComplianceMargin(int iServoID, int iVal) = 0;
	virtual int ReadCCWComplianceMargin(int iServoID) = 0;

	virtual void WriteCWComplianceSlope(int iServoID, int iVal) = 0;
	virtual int ReadCWComplianceSlope(int iServoID) = 0;

	virtual void WriteCCWComplianceSlope(int iServoID, int iVal) = 0;
	virtual int ReadCCWComplianceSlope(int iServoID) = 0;

	virtual void WriteMaxTorque(int iServoID, int iVal) = 0;
	virtual int ReadMaxTorque(int iServoID) = 0;

	virtual void WriteReturnDelayTime(int iServoID, int iVal) = 0;
	virtual int ReadReturnDelayTime(int iServoID) = 0;

	virtual void WriteCCWAngleLimit(int iServoID, int iVal) = 0;
	virtual int ReadCCWAngleLimit(int iServoID) = 0;

	virtual void WriteCWAngleLimit(int iServoID, int iVal) = 0;
	virtual int ReadCWAngleLimit(int iServoID) = 0;

	virtual void WriteTorqueLimit(int iServoID, int iVal) = 0;
	virtual int ReadTorqueLimit(int iServoID) = 0;

	virtual void AddMotorUpdate(int iPos, int iSpeed) = 0;
	virtual void SetMotorPosVel();

	virtual float *GetDataPointer(const std::string &strDataType);
	virtual bool SetData(const std::string &strDataType, const std::string &strValue);
	virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
	virtual void StepSimulation();
	virtual void Load(StdUtils::CStdXml &oXml);

	virtual void GetLimitValues();

	virtual void ResetSimulation();
	
	virtual void SetRegister(unsigned char reg, unsigned char length, unsigned int value) = 0;
	virtual int GetRegister(unsigned char reg, unsigned char length) = 0;

	virtual void ConfigureServo();

public:
	RbDynamixelServo();
	virtual ~RbDynamixelServo();
	
	virtual void QueryMotorData(bool bVal);
	virtual bool QueryMotorData();

	virtual void MinPosFP(int iVal);
	virtual int MinPosFP();

	virtual void MaxPosFP(int iVal);
	virtual int MaxPosFP();

	virtual void MinAngle(float fltVal);
	virtual float MinAngle();

	virtual void MaxAngle(float fltVal);
	virtual float MaxAngle();

	virtual void MinVelocityFP(int iVal);
	virtual int MinVelocityFP();

	virtual void MaxVelocityFP(int iVal);
	virtual int MaxVelocityFP();

	virtual void RPMPerFPUnit(float fltVal);
	virtual float RPMPerFPUnit();

	virtual void MinLoadFP(int iVal);
	virtual int MinLoadFP();

	virtual void MaxLoadFP(int iVal);
	virtual int MaxLoadFP();
	
	virtual void CWComplianceMargin(int iVal);
	virtual int CWComplianceMargin();
	
	virtual void CCWComplianceMargin(int iVal);
	virtual int CCWComplianceMargin();

	virtual void CWComplianceSlope(int iVal);
	virtual int CWComplianceSlope();

	virtual void CCWComplianceSlope(int iVal);
	virtual int CCWComplianceSlope();

	virtual void MaxTorque(int iVal);
	virtual int MaxTorque();

	virtual float ConvertPosFPToFloat(int iPos);
	virtual int ConvertPosFloatToFP(float fltPos);

	virtual float TranslationRange();
	virtual void TranslationRange(float fltVal);

	virtual float ConvertFPVelocity(int iVel);
	virtual int ConvertFloatVelocity(float fltVelocity);

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

	virtual void Stop();

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

	virtual void SetReturnDelayTime_FP(int iVal);
	virtual int GetReturnDelayTime_FP();

	virtual void SetCWAngleLimit_FP(int iVal);
	virtual void SetCWAngleLimit(float fltLimit);
	virtual int GetCWAngleLimit_FP();
	virtual float GetCWAngleLimit();

	virtual void SetCCWAngleLimit_FP(int iVal);
	virtual void SetCCWAngleLimit(float fltLimit);
	virtual int GetCCWAngleLimit_FP();
	virtual float GetCCWAngleLimit();

	virtual void SetCWComplianceMargin_FP(int iVal);
	virtual int GetCWComplianceMargin_FP();

	virtual void SetCCWComplianceMargin_FP(int iVal);
	virtual int GetCCWComplianceMargin_FP();

	virtual void SetCWComplianceSlope_FP(int iVal);
	virtual int GetCWComplianceSlope_FP();

	virtual void SetCCWComplianceSlope_FP(int iVal);
	virtual int GetCCWComplianceSlope_FP();

	virtual void SetMaxTorque_FP(int iVal);
	virtual int GetMaxTorque_FP();

	virtual int GetMinSimPos_FP();
	virtual float GetMinSimPos();
	virtual void SetMinSimPos(float fltVal);

	virtual int GetMaxSimPos_FP();
	virtual float GetMaxSimPos();
	virtual void SetMaxSimPos(float fltVal);	

	virtual void SetTorqueLimit_FP(int iVal);
	virtual int GetTorqueLimit_FP();
	
	virtual bool ResetToStartPos();
	virtual void ResetToStartPos(bool bVal);

	virtual float QuantizeServoPosition(float fltPos);
	virtual float QuantizeServoVelocity(float fltVel);

	virtual void MicroSleep(unsigned int iTime) = 0;
	virtual Simulator *GetSimulator() = 0;

	virtual void Move(float fltPos, float fltVel);
	virtual void WaitForMoveToFinish();
};

		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

