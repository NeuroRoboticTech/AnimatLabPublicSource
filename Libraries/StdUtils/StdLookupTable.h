// StdLookupTable.h: interface for the CStdLookupTable class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_STDLOOKUPTABLE_H__0FE6F490_A4DE_467F_B44A_93A4EBE63D47__INCLUDED_)
#define AFX_STDLOOKUPTABLE_H__0FE6F490_A4DE_467F_B44A_93A4EBE63D47__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class STD_UTILS_PORT CStdLookupTable 
{
protected:
	CStdArray< CStdDPoint > m_aryInitialPoints;

	int m_iTableSize;
	double m_dblDelta;
	double m_dblStartX;
	double m_dblEndX;
	double *m_aryM;
	double *m_aryB;

	BOOL m_bUseLowLimitValue;
	BOOL m_bUseHighLimitValue;
	double m_dblLowLimitValue;
	double m_dblHighLimitValue;

public:
	CStdLookupTable();
	virtual ~CStdLookupTable();

	int InitialPointCount() {return m_aryInitialPoints.GetSize();};
	CStdDPoint InitialPoint(int iIndex) {return m_aryInitialPoints[iIndex];};
	int TableSize() {return m_iTableSize;};
	double Delta() {return m_dblDelta;};
	double StartX() {return m_dblStartX;};
	double EndX() {return m_dblEndX;};

	BOOL UseLowLimitValue() {return m_bUseLowLimitValue;};
	void UseLowLimitValue(BOOL bVal) {m_bUseLowLimitValue = bVal;};

	BOOL UseHighLimitValue() {return m_bUseHighLimitValue;};
	void UseHighLimitValue(BOOL bVal) {m_bUseHighLimitValue = bVal;};

	double LowLimitValue() {return m_dblLowLimitValue;};
	void LowLimitValue(double dblVal) {m_dblLowLimitValue = dblVal;};

	double HighLimitValue() {return m_dblHighLimitValue;};
	void HighLimitValue(double dblVal) {m_dblHighLimitValue = dblVal;};

	void AddPoint(double dblX, double dblY);
	void Initialize();
	void Clear();
	double Evaluate(double dblX);

	virtual void Load(CStdXml &oXml, string strParamName, bool bThrowError = false);
	virtual void Save(CStdXml &oXml, string strParamName);
};

#endif // !defined(AFX_STDLOOKUPTABLE_H__0FE6F490_A4DE_467F_B44A_93A4EBE63D47__INCLUDED_)
