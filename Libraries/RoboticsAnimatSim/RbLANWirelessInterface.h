// RbLANWirelessInterface.h: interface for the RbLANWirelessInterface class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotInterfaces
		{

class ROBOTICS_PORT RbLANWirelessInterface : public AnimatSim::Robotics::RobotInterface
{
protected:
	int m_iPortNumber;
	int m_iBaudRate;

public:

	RbLANWirelessInterface();
	virtual ~RbLANWirelessInterface();

	virtual void PortNumber(int iPort);
	virtual int PortNumber();

	virtual void BaudRate(int iRate);
	virtual int BaudRate();

#pragma region DataAccesMethods

	virtual float *GetDataPointer(const std::string &strDataType);
	virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
	virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);

#pragma endregion

	virtual void Initialize();
	virtual void StepSimulation();
	virtual void Load(StdUtils::CStdXml &oXml);
};

		}		//MotorControlSystems
	}			// Robotics
}				//RoboticsAnimatSim

