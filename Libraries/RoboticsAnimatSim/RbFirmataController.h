// RbFirmataController.h: interface for the RbFirmataController class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{
			namespace Firmata
			{

class ROBOTICS_PORT RbFirmataController : public AnimatSim::Robotics::RobotIOControl, public ofArduino
{
protected:
	///Com port for the connection to the Arduino
	std::string m_strComPort;

	//Baud rate for the connection to the Arduino
	int m_iBaudRate;

	///The time between when synchronous motor commands are sent.
	float m_fltMotorSendTime;

	///Tick to measure the the timing for sending motor commands
	unsigned long long m_lMotorSendStart;

	boost::signals2::connection m_EInitializedConnection;
	boost::signals2::connection m_EDigitalPinChanged;
	boost::signals2::connection m_EAnalogPinChanged;

	virtual void digitalPinChanged(const int & pinNum);
	virtual void analogPinChanged(const int & pinNum);
	virtual void setupArduino(const int & version);
	virtual void ProcessIO();
	virtual bool OpenIO();
	virtual void CloseIO();

public:
	RbFirmataController();
	virtual ~RbFirmataController();

	virtual void ComPort(std::string strPort);
	virtual std::string ComPort();

	virtual void BaudRate(int iRate);
	virtual int BaudRate();

#pragma region DataAccesMethods

	virtual float *GetDataPointer(const std::string &strDataType);
	virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
	virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

	virtual void Load(StdUtils::CStdXml &oXml);
};

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

