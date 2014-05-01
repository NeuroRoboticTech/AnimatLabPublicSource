// RbFirmataAnalogOutput.h: interface for the RbFirmataAnalogOutput class.
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

class ROBOTICS_PORT RbFirmataAnalogOutput : public AnimatSim::Robotics::RobotPartInterface
{
protected:
    RbHinge *m_lpHinge;

public:
	RbFirmataAnalogOutput();
	virtual ~RbFirmataAnalogOutput();

	virtual void ServoID(int iID);

#pragma region DataAccesMethods

	virtual float *GetDataPointer(const std::string &strDataType);
	virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
	virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

	virtual void Initialize();
    virtual void StepSimulation();
	virtual void Load(StdUtils::CStdXml &oXml);
};

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

