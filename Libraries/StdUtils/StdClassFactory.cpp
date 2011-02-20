// StdClassFactory.cpp: implementation of the IStdClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

IStdClassFactory::IStdClassFactory()
{
}

IStdClassFactory::~IStdClassFactory()
{}

IStdClassFactory *IStdClassFactory::LoadModule(string strModuleName)
{
	TRACE_DEBUG("Loading Module: " + strModuleName);

	if(Std_IsBlank(strModuleName))
		THROW_ERROR(Std_Err_lModuleNameIsBlank, Std_Err_strModuleNameIsBlank);

	HMODULE hMod = NULL;

#ifdef _WIN32_WCE
	wchar_t *sMessage = Std_ConvertFromANSI(strModuleName);

	if(sMessage)
	{
		OutputDebugString(sMessage);
		hMod = LoadLibrary(sMessage);
		delete sMessage;
		sMessage = NULL;
	}
#else
	hMod = LoadLibrary(strModuleName.c_str());
#endif
	
	if(!hMod)
		THROW_PARAM_ERROR(Std_Err_lModuleNotLoaded, Std_Err_strModuleNotLoaded, "Module", strModuleName);

	GetClassFactory lpFactoryFunc = NULL;

	TRACE_DEBUG("  Gettting the classfactory pointer.");

#ifdef _WIN32_WCE
	lpFactoryFunc = (GetClassFactory) GetProcAddress(hMod, _T("GetStdClassFactory")); 
#else
    lpFactoryFunc = (GetClassFactory) GetProcAddress(hMod, "GetStdClassFactory"); 
#endif

	if(!lpFactoryFunc)
		THROW_PARAM_ERROR(Std_Err_lModuleProcNotLoaded, Std_Err_strModuleProcNotLoaded, "Module", strModuleName);

	TRACE_DEBUG("Finished Loading Module: " + strModuleName);
	return lpFactoryFunc();
}







