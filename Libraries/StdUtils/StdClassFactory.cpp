/**
\file	StdClassFactory.cpp

\brief	Implements the standard class factory class.
**/

#include "stdafx.h"


namespace StdUtils
{

/**
\brief	Default constructor.

\author	dcofer
\date	5/3/2011
**/
IStdClassFactory::IStdClassFactory()
{
}

/**
\brief	Destructor.

\author	dcofer
\date	5/3/2011
**/
IStdClassFactory::~IStdClassFactory()
{}

/**
\brief	Loads a DLL module by name and attempts to call the GetStdClassFactory method to get a pointer to the class factory.

\author	dcofer
\date	5/3/2011

\param	strModuleName	Name of the DLL module. 

\return	Pointer to the loaded module, exception if not found.
**/
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
    {
        int iError = GetLastError();
		THROW_PARAM_ERROR(Std_Err_lModuleNotLoaded, Std_Err_strModuleNotLoaded, "Module", strModuleName + ", Last Error: " + STR(iError));
    }

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


}				//StdUtils





