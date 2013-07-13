/**
\file	StdClassFactory.cpp

\brief	Implements the standard class factory class.
**/

#include "StdAfx.h"


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

#ifdef _WINDOWS
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
#else
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

	string strModRenamed = "lib" + Std_Replace(strModuleName, ".dll", ".so");
	//string strModRenamed = "/home/me/Projects/AnimatLabPublicSource/unit_test_bin/libStdClassFactoryTester.so";
	
	void *hMod = NULL;

	hMod = dlopen(strModRenamed.c_str(), RTLD_LAZY);
	
	if(!hMod)
		THROW_PARAM_ERROR(Std_Err_lModuleNotLoaded, Std_Err_strModuleNotLoaded, "Module", strModRenamed + ", Last Error: " + dlerror());

	GetClassFactory lpFactoryFunc = NULL;

	TRACE_DEBUG("  Gettting the classfactory pointer.");

	lpFactoryFunc = (GetClassFactory) dlsym(hMod, "GetStdClassFactory");

	if(!lpFactoryFunc || dlerror() != NULL)
		THROW_PARAM_ERROR(Std_Err_lModuleProcNotLoaded, Std_Err_strModuleProcNotLoaded, "Module", strModRenamed + ", Last Error: " + dlerror());

	TRACE_DEBUG("Finished Loading Module: " + strModRenamed);
	IStdClassFactory *lpFact = lpFactoryFunc();
	
	TRACE_DEBUG("Returning class factory: " + strModRenamed);
	return lpFact;
}

#endif


}				//StdUtils





