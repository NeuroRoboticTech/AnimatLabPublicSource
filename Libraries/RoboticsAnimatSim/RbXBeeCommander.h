// RbDynamixelUSB.h: interface for the RbDynamixelUSB class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

/* bitmasks for buttons array */
#define BUT_R1      0x01
#define BUT_R2      0x02
#define BUT_R3      0x04
#define BUT_L4      0x08
#define BUT_L5      0x10
#define BUT_L6      0x20
#define BUT_RT      0x40
#define BUT_LT      0x80

#define BUT_ID_WALKV	0
#define BUT_ID_WALKH	1
#define BUT_ID_LOOKV	2
#define BUT_ID_LOOKH	3
#define BUT_ID_PAN		4
#define BUT_ID_TILT		5
#define BUT_ID_R1		6
#define BUT_ID_R2		7
#define BUT_ID_R3		8
#define BUT_ID_L4		9
#define BUT_ID_L5		10
#define BUT_ID_L6		11
#define BUT_ID_RT		12
#define BUT_ID_LT		13
#define BUT_ID_TOTAL	14

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{

class ROBOTICS_PORT RbXBeeCommanderButtonData
{
public:
    // joystick values are -125 to 125

	///The array index of this button
	int m_iButtonID;
	float m_fltStart;
    float m_fltValue;      // vertical stick movement = forward speed
	float m_fltStop;
	float m_fltPrev;
	int m_iCount;
	bool m_bStarted;
	int m_iStartDir;

	///This keeps track of whether the sim has already stepped over a change or not.
	///The first time start or stop is set and sim is called we do not want to reset it to zero
	///right then so that the rest of the app can see it. We do want to reset it the next time though.
	int m_iSimStepped;

	///The number of simulation time slices to keep a start/stop signal active.
	int m_iChangeSimStepCount;

	RbXBeeCommanderButtonData()
	{
		m_iButtonID = -1;
		m_iChangeSimStepCount = 5;
		ClearData();
	}

	void ClearData()
	{
		m_fltStart = 0;
		m_fltValue = 0;      
		m_fltStop = 0;
		m_fltPrev = 0;
		m_iCount = 0;
		m_bStarted = 0;
		m_iSimStepped = 0;
		m_iStartDir = 1;
	}

	void CheckStartedStopped();
	void ClearStartStops();
};

class ROBOTICS_PORT RbXBeeCommander : public AnimatSim::Robotics::RemoteControl
{
protected:
	///The serial port this Xbee communicates on.
	std::string m_strPort;

	///The baud rate of communications for this XBee
	int m_iBaudRate;
	
	///The number of simulation time slices to keep a start/stop signal active.
	int m_iChangeSimStepCount;

	///Array of button information to keep track of incoming data.
	RbXBeeCommanderButtonData m_ButtonData[BUT_ID_TOTAL];

	/// buttons are 0 or 1 (PRESSED), and bitmapped
    unsigned char m_iButtons;  // 
    unsigned char m_iExt;      // Extended function set

	ofSerial m_Port;

    // internal variables used for reading messages
    unsigned char vals[7];  // temporary values, moved after we confirm checksum
    int index;              // -1 = waiting for new packet
    int checksum;
    unsigned char status; 

	virtual void StepIO();
	virtual bool OpenIO();
	virtual void CloseIO();

	virtual void ResetData();
	virtual void CheckStartedStopped();

	virtual void ClearStartStops();
	virtual void WaitForThreadNotifyReady();

public:
	RbXBeeCommander();
	virtual ~RbXBeeCommander();

	virtual void Port(std::string strPort);
	virtual std::string Port();

	virtual void BaudRate(int iRate);
	virtual int BaudRate();

	virtual void ChangeSimStepCount(int iRate);
	virtual int ChangeSimStepCount();

#pragma region DataAccesMethods

	virtual float *GetDataPointer(const std::string &strDataType);
	virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
	virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

	virtual void SimStarting();

	virtual void Initialize();
	virtual void ResetSimulation();
	virtual void StepSimulation();
	virtual void Load(StdUtils::CStdXml &oXml);
};

		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

