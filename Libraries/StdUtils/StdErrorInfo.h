/**
\file	StdErrorInfo.h

\brief	Declares the standard error information class.
**/

#pragma once

namespace StdUtils
{
/**
\brief	Information about the standard error. 

\details This is a standard exception type.

\author	dcofer
\date	5/3/2011
**/
class STD_UTILS_PORT CStdErrorInfo : public std::exception 
{
public:
	/// The error number
	long m_lError;

	/// The error message
	std::string m_strError;

	/// The source line code line where the error occurred.
	long m_lSourceLine;

	/// The source file name where the error occurred.
	std::string m_strSourceFile;

	/// The call chain of the errorr.
	CStdArray<std::string> m_aryCallChain;

	CStdErrorInfo();
	CStdErrorInfo(long lError, std::string strError, std::string strSourceFile, long lSourceLine);
	~CStdErrorInfo() throw();

	virtual std::string Log();

	/**
	\brief	Gets the error message.
	
	\author	dcofer
	\date	5/3/2011
	
	\return	std::string error message.
	**/
	virtual const char* what() const throw() {return m_strError.c_str();};

	/**
	\brief	Gets the stack trace.
	
	\author	dcofer
	\date	7/5/2011
	
	\return	stack trace std::string of the error.
	**/
	virtual std::string StackTrace();

};

}				//StdUtils


