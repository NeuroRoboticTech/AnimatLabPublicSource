// RbClassFactory.cpp: implementation of the RbClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "PyClassFactory.h"
#include "ScriptProcessorPy.h"

#ifdef _WINDOWS
	extern "C" __declspec(dllexport) IStdClassFactory* __cdecl GetStdClassFactory() 
#else
	extern "C" IStdClassFactory* GetStdClassFactory() 
#endif
{
	IStdClassFactory *lpFactory = new AnimatSimPy::PyClassFactory;
	return lpFactory;
}


namespace AnimatSimPy
{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

PyClassFactory::PyClassFactory()
{

}

PyClassFactory::~PyClassFactory()
{

}

// ************* Script Processor Type Conversion functions ******************************

ScriptProcessor *PyClassFactory::CreateScriptProcessor(std::string strType, bool bThrowError)
{
	ScriptProcessor *lpScript=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "SCRIPTPROCESSORPY")
		lpScript = new ScriptProcessorPy();
	else
	{
		lpScript = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidPartType, Al_Err_strInvalidPartType, "PartType", strType);
	}
	
	return lpScript;
}
catch(CStdErrorInfo oError)
{
	if(lpScript) delete lpScript;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpScript) delete lpScript;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Script Processor Type Conversion functions ******************************

// ************* IStdClassFactory functions ******************************

CStdSerialize *PyClassFactory::CreateObject(std::string strClassType, std::string strObjectType, bool bThrowError)
{
	CStdSerialize *lpObject=NULL;

	strClassType = Std_ToUpper(Std_Trim(strClassType));

	if(strClassType == "SCRIPTPROCESSOR")
		lpObject = CreateScriptProcessor(strObjectType, bThrowError);
	else
	{
		lpObject = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Std_Err_lInvalidClassType, Std_Err_strInvalidClassType, "ClassType", strClassType);
	}

	return lpObject;
}
// ************* IStdClassFactory functions ******************************



}			//RoboticsAnimatSim
