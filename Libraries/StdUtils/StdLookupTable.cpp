/**
\file	StdLookupTable.cpp

\brief	Implements the standard lookup table class.
**/

#include "stdafx.h"


namespace StdUtils
{

/**
\brief	Default constructor.

\author	dcofer
\date	5/3/2011
**/
CStdLookupTable::CStdLookupTable()
{
	m_iTableSize = 0;
	m_dblDelta = 0;
	m_dblStartX = 0;
	m_dblEndX = 0;
	m_aryM = NULL;
	m_aryB = NULL;

	m_bUseLowLimitValue = TRUE;
	m_bUseHighLimitValue = TRUE;
	m_dblLowLimitValue = 0;
	m_dblHighLimitValue = 0;
}

/**
\brief	Destructor.

\author	dcofer
\date	5/3/2011
**/
CStdLookupTable::~CStdLookupTable()
{
	try
	{
		Clear();
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of CStdLookupTable\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Clears this lookup table.

\author	dcofer
\date	5/3/2011
**/
void CStdLookupTable::Clear()
{
		if(m_aryM)
			delete [] m_aryM;

		if(m_aryB)
			delete [] m_aryB;

		m_aryInitialPoints.RemoveAll();
}

/**
\brief	Adds a point to the initial points.

\author	dcofer
\date	5/3/2011

\param	dblX	The double x coordinate. 
\param	dblY	The double y coordinate. 
**/
void CStdLookupTable::AddPoint(double dblX, double dblY)
{
	CStdDPoint oPoint(dblX, dblY, 0);
	m_aryInitialPoints.Add(oPoint);
}

/**
\brief	Compare data points.

\author	dcofer
\date	5/3/2011

\param	oPoint1	The first o point. 
\param	oPoint2	The second o point. 

\return	true if point1.x less than point2.x, false else.
**/
BOOL CompareDataPoints(CStdDPoint oPoint1, CStdDPoint oPoint2)
{
	if(oPoint1.x<oPoint2.x)
		return TRUE;
	else
		return FALSE;
}

/**
\brief	Initializes this lookup table.

\author	dcofer
\date	5/3/2011
**/
void CStdLookupTable::Initialize()
{
	if(m_aryInitialPoints.GetSize() <= 1)
		THROW_ERROR(Std_Err_lNotEnoughPointsInLookupTable, Std_Err_strNotEnoughPointsInLookupTable);

#if defined(STD_TRACING_ON)
	TracePointArray(m_aryInitialPoints, "Unsorted LookupTable Points");
#endif

	//First sort the initial points.
	stable_sort(m_aryInitialPoints.begin(), m_aryInitialPoints.end(), CompareDataPoints);

#if defined(STD_TRACING_ON)
	TracePointArray(m_aryInitialPoints, "Sorted LookupTable Points");
#endif

	//Now make sure that there are not two points with the same x value, and find the minimum delta
	double dblMin = -1, dblDelta;
	int iCount = m_aryInitialPoints.GetSize(), iIndex;
	CStdDPoint oFirst, oSecond;
	for(iIndex=1; iIndex<iCount; iIndex++)
	{
		oFirst = m_aryInitialPoints[iIndex-1];
		oSecond  = m_aryInitialPoints[iIndex];

		if(oFirst.x == oSecond.x)
			THROW_PARAM_ERROR(Std_Err_lOverlappingLookupTablePoints, Std_Err_strOverlappingLookupTablePoints, "X", oFirst.x);

		dblDelta = oSecond.x - oFirst.x;
		if(dblMin == -1 || dblDelta < dblMin) 
			dblMin = dblDelta;
	}

	//Create the tables
	m_dblDelta = dblMin;
	m_dblStartX = m_aryInitialPoints[0].x;
	m_dblEndX = m_aryInitialPoints[m_aryInitialPoints.GetSize()-1].x;
	m_iTableSize = (int) ((m_dblEndX-m_dblStartX)/m_dblDelta);

	if(m_aryM) 
		delete [] m_aryM;
	if(m_aryB) 
		delete [] m_aryB;

	m_aryM = new double[m_iTableSize];
	m_aryB = new double[m_iTableSize];

	//Now fill the tables with the line equations for each segment.
	double dblX = m_dblStartX;
	int iInitPointIndex=1, iInitPointTotal = m_aryInitialPoints.GetSize();
	oFirst = m_aryInitialPoints[0];
	oSecond  = m_aryInitialPoints[1];
	for(iIndex=1; iIndex<m_iTableSize; iIndex++)
	{
		m_aryM[iIndex] = (oSecond.y - oFirst.y) / (oSecond.x - oFirst.x);
		m_aryB[iIndex] = oSecond.y - m_aryM[iIndex]*oSecond.x;

		dblX += m_dblDelta;

		if( (dblX > oSecond.x) || (dblX < oSecond.x && fabs(oSecond.x-dblX) < 0.0001) )
		{
			if(iInitPointIndex<iInitPointTotal)
			{
				iInitPointIndex++;
				oFirst = m_aryInitialPoints[iInitPointIndex-1];
				oSecond  = m_aryInitialPoints[iInitPointIndex];
			}
		}
	}

	m_aryM[0] = m_aryM[1];
	m_aryB[0] = m_aryB[1];
}

/**
\brief	Evaluates a x value to get the calculated y value.

\author	dcofer
\date	5/3/2011

\param	dblX	The double x coordinate. 

\return	.
**/
double CStdLookupTable::Evaluate(double dblX)
{
	int iIndex;

	if(dblX < m_dblStartX)
	{
		if(m_bUseLowLimitValue)
			return m_dblLowLimitValue;

		iIndex = 0;
	}
	else if(dblX > m_dblEndX)
	{
		if(m_bUseLowLimitValue)
			return m_dblLowLimitValue;

		iIndex = 0;
	}
	else
	{
		iIndex = (int) ((dblX - m_dblStartX)/ m_dblDelta);
		if(iIndex >= m_iTableSize)
			iIndex = m_iTableSize-1;
	}

	double dblVal = m_aryM[iIndex]*dblX + m_aryB[iIndex];
	return dblVal;
}

/**
\brief	Loads the lookup table.

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml	The xml to load. 
\param	strParamName	Name of the xml parameter. 
\param	bThrowError 	true to throw error if there is a problem. 
**/
void CStdLookupTable::Load(CStdXml &oXml, string strParamName, bool bThrowError)
{
	if(oXml.FindChildElement(strParamName, bThrowError))
	{
		m_aryInitialPoints.RemoveAll();

		oXml.IntoElem(); //Into LookupTable Element
		int iCount = oXml.NumberOfChildren();

		CStdDPoint oPoint;
		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			Std_LoadPoint(oXml, iIndex, oPoint);
			m_aryInitialPoints.Add(oPoint);
		}

		m_bUseLowLimitValue = (BOOL) oXml.GetChildBool("UseLowLimitValue", m_bUseLowLimitValue);
		m_bUseHighLimitValue = (BOOL) oXml.GetChildBool("UseHighLimitValue", m_bUseHighLimitValue);
		m_dblLowLimitValue = oXml.GetChildDouble("LowLimitValue", m_dblLowLimitValue);
		m_dblHighLimitValue = oXml.GetChildDouble("HighLimitValue", m_dblHighLimitValue);

		oXml.OutOfElem(); //OutOf LookupTable Element
	}
}

/**
\brief	Saves this lookup table..

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml	The xml to save. 
\param	strParamName	Name of the xml parameter. 
**/
void CStdLookupTable::Save(CStdXml &oXml, string strParamName)
{
}

}				//StdUtils
