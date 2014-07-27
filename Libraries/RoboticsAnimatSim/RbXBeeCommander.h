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

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{

class ROBOTICS_PORT RbXBeeCommander : public AnimatSim::Robotics::RemoteControl
{
protected:
	std::string m_strPort;
	int m_iBaudRate;

    // joystick values are -125 to 125
    signed char m_iWalkV;      // vertical stick movement = forward speed
    signed char m_iWalkH;      // horizontal stick movement = sideways or angular speed
    signed char m_iLookV;      // vertical stick movement = tilt    
    signed char m_iLookH;      // horizontal stick movement = pan (when we run out of pan, turn body?)
    // 0-1023, use in extended mode    
    int m_iPan;
    int m_iTilt;
    
    // buttons are 0 or 1 (PRESSED), and bitmapped
    unsigned char m_iButtons;  // 
    unsigned char m_iExt;      // Extended function set

	ofSerial m_Port;

    // internal variables used for reading messages
    unsigned char vals[7];  // temporary values, moved after we confirm checksum
    int index;              // -1 = waiting for new packet
    int checksum;
    unsigned char status; 

	virtual void ProcessIO();
	virtual void StepIO();
	virtual void ExitIOThread();

public:
	RbXBeeCommander();
	virtual ~RbXBeeCommander();

	virtual void Port(std::string strPort);
	virtual std::string Port();

	virtual void BaudRate(int iRate);
	virtual int BaudRate();

#pragma region DataAccesMethods

	virtual float *GetDataPointer(const std::string &strDataType);
	virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
	virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

	virtual void Initialize();
	virtual void Load(StdUtils::CStdXml &oXml);
};

		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

