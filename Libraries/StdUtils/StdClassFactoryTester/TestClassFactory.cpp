// TestClassFactory.cpp: implementation of the TestClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
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

CStdSerialize *TestClassFactory::CreateObject(string strClassType, string strObjectType, BOOL bThrowError)
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



extern "C" __declspec(dllexport) IStdClassFactory* __cdecl GetStdClassFactory() 
{
	IStdClassFactory *lpFactory = new TestClassFactory;
	return lpFactory;
}
