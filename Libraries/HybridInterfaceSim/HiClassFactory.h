// HiClassFactory.h: interface for the HiClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#pragma once


namespace HybridInterfaceSim
{

	class HYBRID_PORT HiClassFactory : public IStdClassFactory  
	{
	public:
		HiClassFactory();
		virtual ~HiClassFactory();

		virtual RobotIOControl *CreateRobotIOControl(std::string strType, bool bThrowError = true);
		virtual RobotPartInterface *CreateRobotPartInterface(std::string strType, bool bThrowError = true);

		virtual CStdSerialize *CreateObject(std::string strClassType, std::string strObjectType, bool bThrowError = true);
	};

}			//HybridInterfaceSim
