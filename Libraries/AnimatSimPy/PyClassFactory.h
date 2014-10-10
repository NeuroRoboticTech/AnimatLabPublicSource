// RbClassFactory.h: interface for the RbClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#pragma once


namespace AnimatSimPy
{

	class ANIMATSIMPY_PORT PyClassFactory : public IStdClassFactory  
	{
	public:
		PyClassFactory();
		virtual ~PyClassFactory();

		virtual ScriptProcessor *CreateScriptProcessor(std::string strType, bool bThrowError = true);
		virtual CStdSerialize *CreateObject(std::string strClassType, std::string strObjectType, bool bThrowError = true);
	};

}			//RoboticsAnimatSim
