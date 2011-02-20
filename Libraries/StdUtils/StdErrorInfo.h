// ErrorInfo.h: interface for the CErrorInfo class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ERRORINFO_H__E227ABE1_471E_11D4_BD86_00A0CC2405DA__INCLUDED_)
#define AFX_ERRORINFO_H__E227ABE1_471E_11D4_BD86_00A0CC2405DA__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class STD_UTILS_PORT CStdErrorInfo : public std::exception 
{
public:
	long m_lError;
	string m_strError;
	long m_lSourceLine;
	string m_strSourceFile;

	CStdArray<string> m_aryCallChain;

	CStdErrorInfo();
	CStdErrorInfo(long lError, string strError, string strSourceFile, long lSourceLine);
	virtual ~CStdErrorInfo();

	virtual string Log();
	virtual const char* what() const throw() {return m_strError.c_str();};
};

#endif // !defined(AFX_ERRORINFO_H__E227ABE1_471E_11D4_BD86_00A0CC2405DA__INCLUDED_)
