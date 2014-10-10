// TestClassFactory.cpp: implementation of the TestClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "TestClassFactory.h"
#include "TestObject.h"


//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

TestClassFactory::TestClassFactory()
{

}

TestClassFactory::~TestClassFactory()
{

}


// ************* IStdClassFactory functions ******************************

CStdSerialize *TestClassFactory::CreateObject(std::string strClassType, std::string strObjectType, bool bThrowError)
{
	CStdSerialize *lpObject=NULL;

	strClassType = Std_ToUpper(Std_Trim(strClassType));
	strObjectType = Std_ToUpper(Std_Trim(strObjectType));

	if(strClassType == "TEST" && strObjectType == "TESTOBJECT")
	{
		lpObject = new TestObject;
		return lpObject;
	}

	lpObject = NULL;
	if(bThrowError)
		THROW_PARAM_ERROR(Std_Err_lInvalidClassType, Std_Err_strInvalidClassType, "ClassType", strClassType);

	return lpObject;
}
// ************* IStdClassFactory functions ******************************

#ifdef _WINDOWS
	extern "C" __declspec(dllexport) IStdClassFactory* __cdecl GetStdClassFactory() 
#else
	extern "C" IStdClassFactory *GetStdClassFactory() 
#endif
{
	IStdClassFactory *lpFactory = new TestClassFactory;
	return lpFactory;
}
