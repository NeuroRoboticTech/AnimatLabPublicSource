// HiSpike2Input.h: interface for the HiSpike2Input class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace HybridInterfaceSim
{
	namespace Robotics
	{

class HYBRID_PORT HiSpike2Input : public AnimatSim::Robotics::RobotIOControl
{
protected:
	int m_iPortNumber;

	virtual void ProcessIO();
	virtual void ExitIOThread();

public:
	HiSpike2Input();
	virtual ~HiSpike2Input();

	virtual void PortNumber(int iPort);
	virtual int PortNumber();

#pragma region DataAccesMethods

	virtual float *GetDataPointer(const std::string &strDataType);
	virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
	virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

	virtual void Initialize();
	virtual void Load(StdUtils::CStdXml &oXml);
};

	}			// Robotics
}				//HybridInterfaceSim

