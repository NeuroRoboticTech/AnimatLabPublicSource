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

public:

	RbLANWirelessInterface();
	virtual ~RbLANWirelessInterface();

#pragma region DataAccesMethods

	virtual float *GetDataPointer(const std::string &strDataType);
	virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
	virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

	virtual void Initialize();
	virtual void StepSimulation();
	virtual void Load(StdUtils::CStdXml &oXml);
};

		}		//MotorControlSystems
	}			// Robotics
}				//RoboticsAnimatSim

