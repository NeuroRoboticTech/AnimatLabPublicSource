// HiSpike2.h: interface for the HiSpike2 class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace HybridInterfaceSim
{
	namespace Robotics
	{

class HYBRID_PORT HiSpike2 : public AnimatSim::Robotics::RemoteControl
{
protected:
	int m_iPortNumber;
	int m_iCounter;
	int m_iInternalData;
	float m_fltData;

	virtual bool OpenIO();
	virtual void CloseIO();

public:
	HiSpike2();
	virtual ~HiSpike2();

	virtual void PortNumber(int iPort);
	virtual int PortNumber();

#pragma region DataAccesMethods

	virtual float *GetDataPointer(const std::string &strDataType);
	virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
	virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

	virtual void StepIO();

	virtual void Initialize();
	virtual void ResetSimulation();
	virtual void Load(StdUtils::CStdXml &oXml);
};

	}			// Robotics
}				//HybridInterfaceSim

