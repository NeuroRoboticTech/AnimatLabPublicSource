// StdPostFixEval.h: interface for the CStdPostFixEval class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_STDPOSTFIXEVAL_H__588F66E1_A393_11D4_ADA4_00E098064C82__INCLUDED_)
#define AFX_STDPOSTFIXEVAL_H__588F66E1_A393_11D4_ADA4_00E098064C82__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class STD_UTILS_PORT CStdPostFixEval  
{
protected:
	string m_strEquation;
	CStdArray<string> m_aryPostFix;
	CStdPtrArray<CStdVariable> m_aryVariables;
	CStdStack<double> m_aryStack;
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

#endif // !defined(AFX_STDPOSTFIXEVAL_H__588F66E1_A393_11D4_ADA4_00E098064C82__INCLUDED_)
