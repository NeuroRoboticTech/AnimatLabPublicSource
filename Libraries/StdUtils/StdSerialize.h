// StdSerialize.h: interface for the CStdXmlSerialize class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_STDSERIALIZE_H__E3CB9FC1_892C_446F_9DE6_F2575DE1EE37__INCLUDED_)
#define AFX_STDSERIALIZE_H__E3CB9FC1_892C_446F_9DE6_F2575DE1EE37__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class STD_UTILS_PORT CStdSerialize  
{
public:
	CStdSerialize();
	virtual ~CStdSerialize();
	
	virtual CStdSerialize *Clone();
	virtual void Trace(ostream &oOs);
	virtual void Load(CStdXml &oXml);
	virtual void Save(CStdXml &oXml);
};

ostream STD_UTILS_PORT &operator<<(ostream& oOs, CStdSerialize *lpObj);
ostream STD_UTILS_PORT &operator<<(ostream& oOs, CStdIPoint oPoint);
ostream STD_UTILS_PORT &operator<<(ostream& oOs, CStdLPoint oPoint);
ostream STD_UTILS_PORT &operator<<(ostream& oOs, CStdFPoint oPoint);
ostream STD_UTILS_PORT &operator<<(ostream& oOs, CStdDPoint oPoint);

void STD_UTILS_PORT Std_SavePoint(CStdXml &oXml, string strName, CStdIPoint &oPoint);
void STD_UTILS_PORT Std_SavePoint(CStdXml &oXml, string strName, CStdLPoint &oPoint);
void STD_UTILS_PORT Std_SavePoint(CStdXml &oXml, string strName, CStdFPoint &oPoint);
void STD_UTILS_PORT Std_SavePoint(CStdXml &oXml, string strName, CStdDPoint &oPoint);

bool STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, string strName, CStdIPoint &oPoint, bool bThrowError = true);
bool STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, string strName, CStdLPoint &oPoint, bool bThrowError = true);
bool STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, string strName, CStdFPoint &oPoint, bool bThrowError = true);
bool STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, string strName, CStdDPoint &oPoint, bool bThrowError = true);

void STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, int iIndex, CStdIPoint &oPoint);
void STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, int iIndex, CStdLPoint &oPoint);
void STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, int iIndex, CStdFPoint &oPoint);
void STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, int iIndex, CStdDPoint &oPoint);

double STD_UTILS_PORT Std_CalculateDistance(CStdIPoint &ptA, CStdIPoint &ptB);
double STD_UTILS_PORT Std_CalculateDistance(CStdLPoint &ptA, CStdLPoint &ptB);
double STD_UTILS_PORT Std_CalculateDistance(CStdFPoint &ptA, CStdFPoint &ptB);
double STD_UTILS_PORT Std_CalculateDistance(CStdDPoint &ptA, CStdDPoint &ptB);

#endif // !defined(AFX_STDSERIALIZE_H__E3CB9FC1_892C_446F_9DE6_F2575DE1EE37__INCLUDED_)
