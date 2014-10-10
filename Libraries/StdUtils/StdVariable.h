/**
\file	StdVariable.h

\brief	Declares the standard variable class.
**/

#pragma once

namespace StdUtils
{
/**
\brief	A variable that is used within the CStdPostFixEval class. 

\author	dcofer
\date	5/4/2011
**/
class STD_UTILS_PORT CStdVariable  
{
public:
	/// The variable name.
	std::string m_strVariable;

	/// The variable value.
	double m_dblValue;

	CStdVariable();
	virtual ~CStdVariable();

};

}				//StdUtils
