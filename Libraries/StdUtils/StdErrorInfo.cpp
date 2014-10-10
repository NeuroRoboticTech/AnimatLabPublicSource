/**
\file	StdErrorInfo.cpp

\brief	Implements the standard error information class.
**/

#include "StdAfx.h"

namespace StdUtils
{

/**
\brief	Default constructor.

\author	dcofer
\date	5/3/2011
**/
CStdErrorInfo::CStdErrorInfo()
{
	m_lError = 0;
	m_lSourceLine = 0;
}

/**
\brief	Constructor.

\author	dcofer
\date	5/3/2011

\param	lError		 	The error number. 
\param	strError	 	The error message. 
\param	strSourceFile	The source filename. 
\param	lSourceLine  	Source line number. 
**/
CStdErrorInfo::CStdErrorInfo(long lError, std::string strError, std::string strSourceFile, long lSourceLine)
{
	m_lError = lError;
	m_strError = strError;
	m_strSourceFile = strSourceFile;
	m_lSourceLine = lSourceLine;
}

/**
\brief	Destructor.

\author	dcofer
\date	5/3/2011
**/
CStdErrorInfo::~CStdErrorInfo() throw()
{

try
{
	m_aryCallChain.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of CStdErrorInfo\n", "", -1, false, true);}
}

/**
\brief	Logs the error to the loggin mechanism.

\author	dcofer
\date	5/3/2011

\return	.
**/
std::string CStdErrorInfo::Log()
{
	std::string strText;

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

std::string CStdErrorInfo::StackTrace()
{
	std::string strOut = "Source file: " + m_strSourceFile + "\r\nSource Line: " + STR(m_lSourceLine) + "\r\nStack Trace: \r\n";
	int iCount = m_aryCallChain.GetSize();
	for(int iIdx=0; iIdx<iCount; iIdx++)
		strOut += m_aryCallChain[iIdx];

	return strOut;
}

}				//StdUtils
