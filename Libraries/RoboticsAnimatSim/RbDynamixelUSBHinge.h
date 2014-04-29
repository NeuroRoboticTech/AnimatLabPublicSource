// RbDynamixelUSBHinge.h: interface for the RbDynamixelUSBHinge class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{
			namespace DynamixelUSB
			{

class ROBOTICS_PORT RbDynamixelUSBHinge : public AnimatSim::Robotics::RobotPartInterface, public RbDynamixelUSBServo
{
protected:
    RbHinge *m_lpHinge;

public:
	RbDynamixelUSBHinge();
	virtual ~RbDynamixelUSBHinge();

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

			}	//DynamixelUSB
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

