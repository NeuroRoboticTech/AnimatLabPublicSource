/**
\file	StdPostFixEval.h

\brief	Declares the standard post fix evaluation class.
**/

#pragma once

namespace StdUtils
{
/**
\brief	Standard post fix evaluation class.

\details This evaluates a post-fix equation.

\author	dcofer
\date	5/3/2011
**/
class STD_UTILS_PORT CStdPostFixEval  
{
protected:
	/// The post-fix equation
	std::string m_strEquation;

	/// internal array used to store the equation parts.
	CStdArray<std::string> m_aryPostFix;

	/// Array of variables
	CStdPtrArray<CStdVariable> m_aryVariables;

	/// Stack of equation parts.
	CStdStack<double> m_aryStack;

	/// The double solution value
	double m_dblSolution;

	void SavePostFixInArray(std::string &strEqu);
	void GetParams(double &dblLeft, double &dblRight, int iNumParams);
	CStdVariable *FindVariable(std::string strVarName);
	void FillInVariables(CStdArray<std::string> &aryPostFix);

public:
	CStdPostFixEval();
	virtual ~CStdPostFixEval();

	void AddVariable(std::string strVarName);
	void SetVariable(std::string strVarName, double dblVal);

	double Solution();

	void Equation(std::string strVal);
	std::string Equation();

	double Solve();
};

}				//StdUtils
