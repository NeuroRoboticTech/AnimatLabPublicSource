// ErrorInfo.cpp: implementation of the CErrorInfo class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CStdErrorInfo::CStdErrorInfo()
{
	m_lError = 0;
	m_lSourceLine = 0;
}

	
CStdErrorInfo::CStdErrorInfo(long lError, string strError, string strSourceFile, long lSourceLine)
{
	m_lError = lError;
	m_strError = strError;
	m_strSourceFile = strSourceFile;
	m_lSourceLine = lSourceLine;
}

CStdErrorInfo::~CStdErrorInfo()
{

try
{
	m_aryCallChain.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of CStdErrorInfo\n", "", -1, FALSE, TRUE);}
}


string CStdErrorInfo::Log()
{
	string strText;

	strText = "\r\nErrorNum: " + STR(m_lError) + "\r\n" + 
						"ErrorMsg: " + STR(m_strError) + "\r\n" +
						"Source: " + m_strSourceFile + "(" + STR(m_lSourceLine) + ")\r\n\r\n";

	int iCount = m_aryCallChain.GetSize();
	if(iCount) strText += "RelayPath\r\n";
	for(int iIndex=0; iIndex<iCount; iIndex++)
		strText += m_aryCallChain[iIndex] + "\r\n";
	
	strText += "\r\n\r\n";
	LOG_ERROR(strText);

	return strText;
}
