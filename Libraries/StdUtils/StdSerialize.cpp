// StdSerialize.cpp: implementation of the CStdSerialize class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CStdSerialize::CStdSerialize()
{

}

CStdSerialize::~CStdSerialize()
{

}

CStdSerialize *CStdSerialize::Clone()
{return NULL;}

void CStdSerialize::Trace(ostream &oOs)
{}

void CStdSerialize::Load(CStdXml &oXml)
{}

void CStdSerialize::Save(CStdXml &oXml)
{}


ostream STD_UTILS_PORT &operator<<(ostream& oOs, CStdSerialize *lpObj)
{
	lpObj->Trace(oOs);
	return oOs;
}

ostream STD_UTILS_PORT &operator<<(ostream& oOs, CStdIPoint oPoint)
{
	oOs << "(" << oPoint.x << ", " << oPoint.y << ", " << oPoint.z << ")";
	return oOs;
}

ostream STD_UTILS_PORT &operator<<(ostream& oOs, CStdLPoint oPoint)
{
	oOs << "(" << oPoint.x << ", " << oPoint.y << ", " << oPoint.z << ")";
	return oOs;
}

ostream STD_UTILS_PORT &operator<<(ostream& oOs, CStdFPoint oPoint)
{
	oOs << "(" << oPoint.x << ", " << oPoint.y << ", " << oPoint.z << ")";
	return oOs;
}

ostream STD_UTILS_PORT &operator<<(ostream& oOs, CStdDPoint oPoint)
{
	oOs << "(" << oPoint.x << ", " << oPoint.y << ", " << oPoint.z << ")";
	return oOs;
}


//Save To Xml
void STD_UTILS_PORT Std_SavePoint(CStdXml &oXml, string strName, CStdIPoint &oPoint)
{
	oXml.AddChildElement(strName);
	oXml.IntoChildElement(strName);
	oXml.SetAttrib("x", oPoint.x);
	oXml.SetAttrib("y", oPoint.y);
	oXml.SetAttrib("z", oPoint.z);
	oXml.OutOfElem();
}

void STD_UTILS_PORT Std_SavePoint(CStdXml &oXml, string strName, CStdLPoint &oPoint)
{
	oXml.AddChildElement(strName);
	oXml.IntoChildElement(strName);
	oXml.SetAttrib("x", oPoint.x);
	oXml.SetAttrib("y", oPoint.y);
	oXml.SetAttrib("z", oPoint.z);
	oXml.OutOfElem();
}

void STD_UTILS_PORT Std_SavePoint(CStdXml &oXml, string strName, CStdFPoint &oPoint)
{
	oXml.AddChildElement(strName);
	oXml.IntoChildElement(strName);
	oXml.SetAttrib("x", oPoint.x);
	oXml.SetAttrib("y", oPoint.y);
	oXml.SetAttrib("z", oPoint.z);
	oXml.OutOfElem();
}

void STD_UTILS_PORT Std_SavePoint(CStdXml &oXml, string strName, CStdDPoint &oPoint)
{
	oXml.AddChildElement(strName);
	oXml.IntoChildElement(strName);
	oXml.SetAttrib("x", oPoint.x);
	oXml.SetAttrib("y", oPoint.y);
	oXml.SetAttrib("z", oPoint.z);
	oXml.OutOfElem();
}


//Load From Xml
bool STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, string strName, CStdIPoint &oPoint, bool bThrowError)
{
	if(oXml.FindChildElement(strName, bThrowError))
	{
		oXml.IntoElem();
		oPoint.x = oXml.GetAttribInt("x");
		oPoint.y = oXml.GetAttribInt("y");
		oPoint.z = oXml.GetAttribInt("z", 0);
		oXml.OutOfElem();
		return true;
	}

	return false;
}

bool STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, string strName, CStdLPoint &oPoint, bool bThrowError)
{
	if(oXml.FindChildElement(strName, bThrowError))
	{
		oXml.IntoElem();
		oPoint.x = oXml.GetAttribLong("x");
		oPoint.y = oXml.GetAttribLong("y");
		oPoint.z = oXml.GetAttribLong("z", 0);
		oXml.OutOfElem();
		return true;
	}

	return false;
}

bool STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, string strName, CStdFPoint &oPoint, bool bThrowError)
{
	if(oXml.FindChildElement(strName, bThrowError))
	{
		oXml.IntoElem();
		oPoint.x = oXml.GetAttribFloat("x");
		oPoint.y = oXml.GetAttribFloat("y");
		oPoint.z = oXml.GetAttribFloat("z", 0);
		oXml.OutOfElem();
		return true;
	}

	return false;
}

bool STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, string strName, CStdDPoint &oPoint, bool bThrowError)
{
	if(oXml.FindChildElement(strName, bThrowError))
	{
		oXml.IntoElem();
		oPoint.x = oXml.GetAttribDouble("x");
		oPoint.y = oXml.GetAttribDouble("y");
		oPoint.z = oXml.GetAttribDouble("z", 0);
		oXml.OutOfElem();
		return true;
	}

	return false;
}

void STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, int iIndex, CStdIPoint &oPoint)
{
	oXml.FindChildByIndex(iIndex);
	oXml.IntoElem();
	oPoint.x = oXml.GetAttribInt("x");
	oPoint.y = oXml.GetAttribInt("y");
	oPoint.z = oXml.GetAttribInt("z", 0);
	oXml.OutOfElem();
}

void STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, int iIndex, CStdLPoint &oPoint)
{
	oXml.FindChildByIndex(iIndex);
	oXml.IntoElem();
	oPoint.x = oXml.GetAttribLong("x");
	oPoint.y = oXml.GetAttribLong("y");
	oPoint.z = oXml.GetAttribLong("z", 0);
	oXml.OutOfElem();
}

void STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, int iIndex, CStdFPoint &oPoint)
{
	oXml.FindChildByIndex(iIndex);
	oXml.IntoElem();
	oPoint.x = oXml.GetAttribFloat("x");
	oPoint.y = oXml.GetAttribFloat("y");
	oPoint.z = oXml.GetAttribFloat("z", 0);
	oXml.OutOfElem();
}

void STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, int iIndex, CStdDPoint &oPoint)
{
	oXml.FindChildByIndex(iIndex);
	oXml.IntoElem();
	oPoint.x = oXml.GetAttribDouble("x");
	oPoint.y = oXml.GetAttribDouble("y");
	oPoint.z = oXml.GetAttribDouble("z", 0);
	oXml.OutOfElem();
}

double STD_UTILS_PORT Std_CalculateDistance(CStdIPoint &ptA, CStdIPoint &ptB)
{
	return (double) sqrt( (double) (pow((double)(ptA.x-ptB.x), 2) + pow((double)(ptA.y-ptB.y), 2) + pow((double)(ptA.z-ptB.z), 2)) );
}

double STD_UTILS_PORT Std_CalculateDistance(CStdLPoint &ptA, CStdLPoint &ptB)
{
	return (double) sqrt( (double) (pow((double)(ptA.x-ptB.x), 2) + pow((double)(ptA.y-ptB.y), 2) + pow((double)(ptA.z-ptB.z), 2)) );
}

double STD_UTILS_PORT Std_CalculateDistance(CStdFPoint &ptA, CStdFPoint &ptB)
{
	return sqrt( pow((ptA.x-ptB.x), 2) + pow((ptA.y-ptB.y), 2) + pow((ptA.z-ptB.z), 2) );
}

double STD_UTILS_PORT Std_CalculateDistance(CStdDPoint &ptA, CStdDPoint &ptB)
{
	return sqrt( pow((ptA.x-ptB.x), 2) + pow((ptA.y-ptB.y), 2) + pow((ptA.z-ptB.z), 2) );
}

