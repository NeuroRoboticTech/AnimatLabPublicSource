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

class ROBOTICS_PORT RbXBeeCommander : public AnimatSim::Robotics::RemoteControl
{
protected:
	///The serial port this Xbee communicates on.
	std::string m_strPort;

	///The baud rate of communications for this XBee
	int m_iBaudRate;

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
	virtual void WaitForThreadNotifyReady();

	virtual void CreateDataIDMap();

public:
	RbXBeeCommander();
	virtual ~RbXBeeCommander();

	virtual void Port(std::string strPort);
	virtual std::string Port();

	virtual void BaudRate(int iRate);
	virtual int BaudRate();

#pragma region DataAccesMethods

	virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
	virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

	virtual void SimStarting();

	virtual void Initialize();
	virtual void Load(StdUtils::CStdXml &oXml);
};

		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

