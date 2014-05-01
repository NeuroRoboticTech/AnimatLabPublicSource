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

	///Set to true once the Arduino is setup correctly.
	bool m_bSetupArduino;

	///Flags the thread processing loop to exit.
	bool m_bStopArduino;

	boost::thread m_ArduinoThread;

	boost::signals2::connection m_EInitializedConnection;
	boost::signals2::connection m_EDigitalPinChanged;
	boost::signals2::connection m_EAnalogPinChanged;

	boost::interprocess::interprocess_mutex m_WaitForArduinoMutex;
	boost::interprocess::interprocess_condition  m_WaitForArduinoCond;

	virtual void ProcessArduino();
	virtual void digitalPinChanged(const int & pinNum);
	virtual void analogPinChanged(const int & pinNum);
	virtual void setupArduino(const int & version);

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

	virtual void Initialize();
	virtual void Load(StdUtils::CStdXml &oXml);
};

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

