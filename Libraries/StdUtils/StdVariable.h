// StdVariable.h: interface for the CStdVariable class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_STDVARIABLE_H__588F66E2_A393_11D4_ADA4_00E098064C82__INCLUDED_)
#define AFX_STDVARIABLE_H__588F66E2_A393_11D4_ADA4_00E098064C82__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class STD_UTILS_PORT CStdVariable  
{
public:
	string m_strVariable;
	double m_dblValue;

	CStdVariable();
	virtual ~CStdVariable();

};

#endif // !defined(AFX_STDVARIABLE_H__588F66E2_A393_11D4_ADA4_00E098064C82__INCLUDED_)
