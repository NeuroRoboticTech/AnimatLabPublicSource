// HiClassFactory.cpp: implementation of the HiClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "HiClassFactory.h"

#include "HiSpike2.h"
#include "HiC884Controller.h"
#include "HiM110Actuator.h"

#ifdef _WINDOWS
	extern "C" __declspec(dllexport) IStdClassFactory* __cdecl GetStdClassFactory() 
#else
	extern "C" IStdClassFactory* GetStdClassFactory() 
#endif
{
	IStdClassFactory *lpFactory = new HiClassFactory;
	return lpFactory;
}

namespace HybridInterfaceSim
{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

HiClassFactory::HiClassFactory()
{

}

HiClassFactory::~HiClassFactory()
{

}


// ************* Robot IO Control Conversion functions ******************************

RobotIOControl *HiClassFactory::CreateRobotIOControl(std::string strType, bool bThrowError)
{
	RobotIOControl *lpControl=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "SPIKE2")
	{
		lpControl = new HiSpike2;
	}
	else if(strType == "C884CONTROLLER")
	{
		lpControl = new HiC884Controller;
	}
	else
	{
		lpControl = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidRobotIOControlType, Al_Err_strInvalidRobotIOControlType, "RobotartIOControl", strType);
	}

	return lpControl;
}
catch(CStdErrorInfo oError)
{
	if(lpControl) delete lpControl;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpControl) delete lpControl;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


// ************* Robot IO Control Conversion functions ******************************


// ************* Robot Part Interface Conversion functions ******************************

RobotPartInterface *HiClassFactory::CreateRobotPartInterface(std::string strType, bool bThrowError)
{
	RobotPartInterface *lpInterface=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "M110ACTUATOR")
	{
		lpInterface = new HiM110Actuator;
	}
	else
	{
		lpInterface = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidRobotPartInterfaceType, Al_Err_strInvalidRobotPartInterfaceType, "RobotartInterface", strType);
	}

	return lpInterface;
}
catch(CStdErrorInfo oError)
{
	if(lpInterface) delete lpInterface;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpInterface) delete lpInterface;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


// ************* Robot Part Interface Type Conversion functions ******************************



// ************* IStdClassFactory functions ******************************

CStdSerialize *HiClassFactory::CreateObject(std::string strClassType, std::string strObjectType, bool bThrowError)
{
	CStdSerialize *lpObject=NULL;

	strClassType = Std_ToUpper(Std_Trim(strClassType));

	if(strClassType == "ROBOTIOCONTROL")
		lpObject = CreateRobotIOControl(strObjectType, bThrowError);
	else if(strClassType == "ROBOTPARTINTERFACE")
		lpObject = CreateRobotPartInterface(strObjectType, bThrowError);
	else
	{
		lpObject = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Std_Err_lInvalidClassType, Std_Err_strInvalidClassType, "ClassType", strClassType);
	}

	return lpObject;
}
// ************* IStdClassFactory functions ******************************


}			//HybridInterfaceSim
