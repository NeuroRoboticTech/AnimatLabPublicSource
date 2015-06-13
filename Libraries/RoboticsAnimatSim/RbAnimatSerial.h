// RbAnimatSerial.h: interface for the RbAnimatSerial class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

#define MAX_ANIMAT_BUFFER 1024

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{

class ROBOTICS_PORT RbAnimatSerial : public AnimatSim::Robotics::RemoteControl
{
protected:
	union id_tag {
	 unsigned char bval[2];
	 unsigned short ival;
	} m_id;
	
	union value_tag {
	 unsigned char bval[4];
	 float fval;
	} m_value;

	///The serial port this Xbee communicates on.
	std::string m_strPort;

	///The baud rate of communications for this XBee
	int m_iBaudRate;

	ofSerial m_Port;

	// internal variables used for reading messages
	unsigned char m_vals[MAX_ANIMAT_BUFFER];  // temporary values, moved after we confirm checksum
    int m_index;              // -1 = waiting for new packet
    int m_checksum;
	int m_iMessageID;
	int m_iPacketSize;

    unsigned char status; 
	virtual void StepIO();
	virtual bool OpenIO();
	virtual void CloseIO();

	virtual void ResetData();
	virtual void WaitForThreadNotifyReady();
	virtual void ReadData();

public:
	RbAnimatSerial();
	virtual ~RbAnimatSerial();

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

