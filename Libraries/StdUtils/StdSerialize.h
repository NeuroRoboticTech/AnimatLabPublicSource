/**
\file	StdSerialize.h

\brief	Declares the standard serialize class.
**/

#pragma once

namespace StdUtils
{
/**
\brief	Standard serialize class. 

\details This is a very low level base class. All objects with this system are derived from this base so they 
have the ability to load, clone, save and trace.

\author	dcofer
\date	5/3/2011
**/
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
double STD_UTILS_PORT Std_CalculateDistance(float fltX1, float fltY1, float fltZ1, float fltX2, float fltY2, float fltZ2);

}				//StdUtils
