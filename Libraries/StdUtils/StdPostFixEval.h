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
	string m_strEquation;

	/// internal array used to store the equation parts.
	CStdArray<string> m_aryPostFix;

	/// Array of variables
	CStdPtrArray<CStdVariable> m_aryVariables;

	/// Stack of equation parts.
	CStdStack<double> m_aryStack;

	/// The double solution value
	double m_dblSolution;

	void SavePostFixInArray(string &strEqu);
	void GetParams(double &dblLeft, double &dblRight, int iNumParams);
	CStdVariable *FindVariable(string strVarName);
	void FillInVariables(CStdArray<string> &aryPostFix);

public:
	CStdPostFixEval();
	virtual ~CStdPostFixEval();

	void AddVariable(string strVarName);
	void SetVariable(string strVarName, double dblVal);

	double Solution();

	void Equation(string strVal);
	string Equation();

	double Solve();
};

}				//StdUtils
