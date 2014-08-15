// HiC884Controller.h: interface for the HiC884Controller class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace HybridInterfaceSim
{
	namespace Robotics
	{

class HYBRID_PORT HiC884Controller : public AnimatSim::Robotics::RobotIOControl
{
protected:
	int m_iPortNumber;

	virtual bool OpenIO();
	virtual void CloseIO();

public:
	HiC884Controller();
	virtual ~HiC884Controller();

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

