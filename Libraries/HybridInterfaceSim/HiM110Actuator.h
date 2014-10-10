// HiM110Actuator.h: interface for the HiM110Actuator class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace HybridInterfaceSim
{
	namespace Robotics
	{

class HYBRID_PORT HiM110Actuator : public AnimatSim::Robotics::RobotPartInterface
{
protected:
	HiC884Controller *m_lpParentC884;

public:
	HiM110Actuator();
	virtual ~HiM110Actuator();

	virtual bool IsMotorControl() {return true;};

#pragma region DataAccesMethods

	virtual float *GetDataPointer(const std::string &strDataType);
	virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
	virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion
	
	virtual void SetupIO();
	virtual void StepIO(int iPartIdx);
	virtual void ShutdownIO();

	virtual void Initialize();
    virtual void StepSimulation();
	virtual void ResetSimulation();
	virtual void Load(StdUtils::CStdXml &oXml);
};

	}			// Robotics
}				//HybridInterfaceSim

