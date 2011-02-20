// StdClassFactory.h: interface for the IStdClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_CLASSFACTORY_H__E227ABE1_471E_11D4_BD86_00A0CC2405DA__INCLUDED_)
#define AFX_CLASSFACTORY_H__E227ABE1_471E_11D4_BD86_00A0CC2405DA__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class STD_UTILS_PORT IStdClassFactory 
{
public:
	IStdClassFactory();
	virtual ~IStdClassFactory();

	virtual CStdSerialize *CreateObject(string strClassType, string strObjectType, BOOL bThrowError = TRUE) = 0;

	static IStdClassFactory *LoadModule(string strModuleName);
};

typedef IStdClassFactory *(*GetClassFactory)(void); 

#endif // !defined(AFX_CLASSFACTORY_H__E227ABE1_471E_11D4_BD86_00A0CC2405DA__INCLUDED_)
