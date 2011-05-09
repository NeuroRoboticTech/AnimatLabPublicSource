/**
\file	StdSerialize.cpp

\brief	Implements the standard serialize class.
**/

#include "stdafx.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

namespace StdUtils
{
/**
\brief	Default constructor.

\author	dcofer
\date	5/3/2011
**/
CStdSerialize::CStdSerialize()
{

}

/**
\brief	Destructor.

\author	dcofer
\date	5/3/2011
**/
CStdSerialize::~CStdSerialize()
{

}

/**
\brief	Makes a deep copy of this object.

\author	dcofer
\date	5/3/2011

\return	null if it fails, else a copy of this object.
**/
CStdSerialize *CStdSerialize::Clone()
{return NULL;}

/**
\brief	Traces this object to an output stream.

\author	dcofer
\date	5/3/2011

\param [in,out]	oOs	The output stream. 
**/
void CStdSerialize::Trace(ostream &oOs)
{}

/**
\brief	Loads this object from an xml packet.

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml	The xml to load. 
**/
void CStdSerialize::Load(CStdXml &oXml)
{}

/**
\brief	Saves this object to an xml packet..

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml	The xml to save. 
**/
void CStdSerialize::Save(CStdXml &oXml)
{}

/**
\brief	 writes to an output stream

\return	output stream
**/
ostream STD_UTILS_PORT &operator<<(ostream& oOs, CStdSerialize *lpObj)
{
	lpObj->Trace(oOs);
	return oOs;
}

/**
\brief	 writes to an output stream

\return	output stream
**/
ostream STD_UTILS_PORT &operator<<(ostream& oOs, CStdIPoint oPoint)
{
	oOs << "(" << oPoint.x << ", " << oPoint.y << ", " << oPoint.z << ")";
	return oOs;
}

/**
\brief	 writes to an output stream

\return	output stream
**/
ostream STD_UTILS_PORT &operator<<(ostream& oOs, CStdLPoint oPoint)
{
	oOs << "(" << oPoint.x << ", " << oPoint.y << ", " << oPoint.z << ")";
	return oOs;
}

/**
\brief	 writes to an output stream

\return	output stream
**/
ostream STD_UTILS_PORT &operator<<(ostream& oOs, CStdFPoint oPoint)
{
	oOs << "(" << oPoint.x << ", " << oPoint.y << ", " << oPoint.z << ")";
	return oOs;
}

/**
\brief	 writes to an output stream

\return	output stream
**/
ostream STD_UTILS_PORT &operator<<(ostream& oOs, CStdDPoint oPoint)
{
	oOs << "(" << oPoint.x << ", " << oPoint.y << ", " << oPoint.z << ")";
	return oOs;
}


//Save To Xml

/**
\brief	Standard save point.

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml  	The xml to save. 
\param	strName		  	Name of the xml packet. 
\param [in,out]	oPoint	The point to save. 

\return	.
**/
void STD_UTILS_PORT Std_SavePoint(CStdXml &oXml, string strName, CStdIPoint &oPoint)
{
	oXml.AddChildElement(strName);
	oXml.IntoChildElement(strName);
	oXml.SetAttrib("x", oPoint.x);
	oXml.SetAttrib("y", oPoint.y);
	oXml.SetAttrib("z", oPoint.z);
	oXml.OutOfElem();
}

/**
\brief	Standard save point.

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml  	The xml to save. 
\param	strName		  	Name of the xml packet. 
\param [in,out]	oPoint	The point to save. 

\return	.
**/
void STD_UTILS_PORT Std_SavePoint(CStdXml &oXml, string strName, CStdLPoint &oPoint)
{
	oXml.AddChildElement(strName);
	oXml.IntoChildElement(strName);
	oXml.SetAttrib("x", oPoint.x);
	oXml.SetAttrib("y", oPoint.y);
	oXml.SetAttrib("z", oPoint.z);
	oXml.OutOfElem();
}

/**
\brief	Standard save point.

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml  	The xml to save. 
\param	strName		  	Name of the xml packet. 
\param [in,out]	oPoint	The point to save. 

\return	.
**/
void STD_UTILS_PORT Std_SavePoint(CStdXml &oXml, string strName, CStdFPoint &oPoint)
{
	oXml.AddChildElement(strName);
	oXml.IntoChildElement(strName);
	oXml.SetAttrib("x", oPoint.x);
	oXml.SetAttrib("y", oPoint.y);
	oXml.SetAttrib("z", oPoint.z);
	oXml.OutOfElem();
}

/**
\brief	Standard save point.

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml  	The xml to save. 
\param	strName		  	Name of the xml packet. 
\param [in,out]	oPoint	The point to save. 

\return	.
**/
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

/**
\brief	Standard load point.

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml  	The xml to load. 
\param	strName		  	Name of the xml element. 
\param [in,out]	oPoint	The point to load. 
\param	bThrowError   	true to throw error if there is a problem. 

\return	.
**/
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

//Load From Xml

/**
\brief	Standard load point.

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml  	The xml to load. 
\param	strName		  	Name of the xml element. 
\param [in,out]	oPoint	The point to load. 
\param	bThrowError   	true to throw error if there is a problem. 

\return	.
**/
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

//Load From Xml

/**
\brief	Standard load point.

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml  	The xml to load. 
\param	strName		  	Name of the xml element. 
\param [in,out]	oPoint	The point to load. 
\param	bThrowError   	true to throw error if there is a problem. 

\return	.
**/
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

//Load From Xml

/**
\brief	Standard load point.

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml  	The xml to load. 
\param	strName		  	Name of the xml element. 
\param [in,out]	oPoint	The point to load. 
\param	bThrowError   	true to throw error if there is a problem. 

\return	.
**/
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

/**
\brief	Standard load point.

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml  	The xml to load. 
\param	iIndex		  	Zero-based index of the child element to load. 
\param [in,out]	oPoint	The point to load. 

\return	.
**/
void STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, int iIndex, CStdIPoint &oPoint)
{
	oXml.FindChildByIndex(iIndex);
	oXml.IntoElem();
	oPoint.x = oXml.GetAttribInt("x");
	oPoint.y = oXml.GetAttribInt("y");
	oPoint.z = oXml.GetAttribInt("z", 0);
	oXml.OutOfElem();
}

/**
\brief	Standard load point.

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml  	The xml to load. 
\param	iIndex		  	Zero-based index of the child element to load. 
\param [in,out]	oPoint	The point to load. 

\return	.
**/
void STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, int iIndex, CStdLPoint &oPoint)
{
	oXml.FindChildByIndex(iIndex);
	oXml.IntoElem();
	oPoint.x = oXml.GetAttribLong("x");
	oPoint.y = oXml.GetAttribLong("y");
	oPoint.z = oXml.GetAttribLong("z", 0);
	oXml.OutOfElem();
}

/**
\brief	Standard load point.

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml  	The xml to load. 
\param	iIndex		  	Zero-based index of the child element to load. 
\param [in,out]	oPoint	The point to load. 

\return	.
**/
void STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, int iIndex, CStdFPoint &oPoint)
{
	oXml.FindChildByIndex(iIndex);
	oXml.IntoElem();
	oPoint.x = oXml.GetAttribFloat("x");
	oPoint.y = oXml.GetAttribFloat("y");
	oPoint.z = oXml.GetAttribFloat("z", 0);
	oXml.OutOfElem();
}

/**
\brief	Standard load point.

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml  	The xml to load. 
\param	iIndex		  	Zero-based index of the child element to load. 
\param [in,out]	oPoint	The point to load. 

\return	.
**/
void STD_UTILS_PORT Std_LoadPoint(CStdXml &oXml, int iIndex, CStdDPoint &oPoint)
{
	oXml.FindChildByIndex(iIndex);
	oXml.IntoElem();
	oPoint.x = oXml.GetAttribDouble("x");
	oPoint.y = oXml.GetAttribDouble("y");
	oPoint.z = oXml.GetAttribDouble("z", 0);
	oXml.OutOfElem();
}

/**
\brief	Calculates the distance between two points.

\author	dcofer
\date	5/3/2011

\param [in,out]	ptA	The point a. 
\param [in,out]	ptB	The point b. 

\return	.
**/
double STD_UTILS_PORT Std_CalculateDistance(CStdIPoint &ptA, CStdIPoint &ptB)
{
	return (double) sqrt( (double) (pow((double)(ptA.x-ptB.x), 2) + pow((double)(ptA.y-ptB.y), 2) + pow((double)(ptA.z-ptB.z), 2)) );
}

/**
\brief	Calculates the distance between two points.

\author	dcofer
\date	5/3/2011

\param [in,out]	ptA	The point a. 
\param [in,out]	ptB	The point b. 

\return	.
**/
double STD_UTILS_PORT Std_CalculateDistance(CStdLPoint &ptA, CStdLPoint &ptB)
{
	return (double) sqrt( (double) (pow((double)(ptA.x-ptB.x), 2) + pow((double)(ptA.y-ptB.y), 2) + pow((double)(ptA.z-ptB.z), 2)) );
}

/**
\brief	Calculates the distance between two points.

\author	dcofer
\date	5/3/2011

\param [in,out]	ptA	The point a. 
\param [in,out]	ptB	The point b. 

\return	.
**/
double STD_UTILS_PORT Std_CalculateDistance(CStdFPoint &ptA, CStdFPoint &ptB)
{
	return sqrt( pow((ptA.x-ptB.x), 2) + pow((ptA.y-ptB.y), 2) + pow((ptA.z-ptB.z), 2) );
}

/**
\brief	Calculates the distance between two points.

\author	dcofer
\date	5/3/2011

\param [in,out]	ptA	The point a. 
\param [in,out]	ptB	The point b. 

\return	.
**/
double STD_UTILS_PORT Std_CalculateDistance(CStdDPoint &ptA, CStdDPoint &ptB)
{
	return sqrt( pow((ptA.x-ptB.x), 2) + pow((ptA.y-ptB.y), 2) + pow((ptA.z-ptB.z), 2) );
}

}				//StdUtils
