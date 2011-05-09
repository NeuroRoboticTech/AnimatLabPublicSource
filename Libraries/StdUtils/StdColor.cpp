/**
\file	StdColor.cpp

\brief	Implements the standard color class.
**/

#include "StdAfx.h"

namespace StdUtils
{


CStdColor::CStdColor()
{
	m_fltR = 0;
	m_fltB = 0;
	m_fltG = 0;
	m_fltA = 0;
	m_fltMaxRange = 1;
}


CStdColor::CStdColor(float fltMaxRange)
{
	m_fltR = 0;
	m_fltB = 0;
	m_fltG = 0;
	m_fltA = 0;
	m_fltMaxRange = fltMaxRange;
}

CStdColor::CStdColor(float valr, float valg, float valb, float vala, float fltMaxRange)
{
	m_fltMaxRange = fltMaxRange;

	r(valr);
	g(valg);
	b(valb);
	a(vala);
}

/**
\brief	Sets the red value of the color

\author	dcofer
\date	5/3/2011

\param	fltR	   	The new red value. 
\param	bThrowError	true to throw error if there is a problem. 
**/
void CStdColor::r(float fltR, BOOL bThrowError)
{
	if(Std_InValidRange((float) 0, (float) 1, fltR, bThrowError, "R"))
		m_fltR = fltR;
}

/**
\brief	Gets the red value of the color

\author	dcofer
\date	5/3/2011

\return	color value.
**/
float CStdColor::r() {return m_fltR;}

/**
\brief	Sets the green value of the color

\author	dcofer
\date	5/3/2011

\param	fltG	   	The new green value. 
\param	bThrowError	true to throw error if there is a problem.
**/
void CStdColor::g(float fltG, BOOL bThrowError)
{
	if(Std_InValidRange((float) 0, (float) 1, fltG, bThrowError, "G"))
		m_fltG = fltG;
}

/**
\brief	Gets the green value of the color

\author	dcofer
\date	5/3/2011

\return	color value.
**/
float CStdColor::g() {return m_fltG;}

/**
\brief	Sets the blue value of the color

\author	dcofer
\date	5/3/2011

\param	fltB	   	The new blue value. 
\param	bThrowError	true to throw error if there is a problem.
**/
void CStdColor::b(float fltB, BOOL bThrowError)
{
	if(Std_InValidRange((float) 0, (float) 1, fltB, bThrowError, "B"))
		m_fltB = fltB;
}

/**
\brief	Gets the blue value of the color

\author	dcofer
\date	5/3/2011

\return	color value.
**/
float CStdColor::b() {return m_fltB;}

/**
\brief	Sets the alpha value of the color

\author	dcofer
\date	5/3/2011

\param	fltA	   	The new alpha value. 
\param	bThrowError	true to throw error if there is a problem.
**/
void CStdColor::a(float fltA, BOOL bThrowError)
{
	if(Std_InValidRange((float) 0, (float) 1, fltA, bThrowError, "A"))
		m_fltA = fltA;
}

/**
\brief	Gets the alpha value of the color

\author	dcofer
\date	5/3/2011

\return	color value.
**/
float CStdColor::a() {return m_fltA;}

/**
\brief	Sets the color values.

\author	dcofer
\date	5/3/2011

\param	R	   	The red value. 
\param	G	   	The green value. 
\param	B	   	The blue value. 
\param	A	   	The alpha value. 
**/
void CStdColor::Set(float R, float G, float B, float A)
{
	r(R);
	g(G);
	b(B);
	a(A);
}

/**
\brief	== casting operator.

\author	dcofer
\date	5/3/2011

\param	oPoint	The point. 

\return	The result of the operation.
**/
bool CStdColor::operator==(const CStdColor &oPoint)
{
	if( (m_fltR == oPoint.m_fltR) && (m_fltB == oPoint.m_fltB) && (m_fltG == oPoint.m_fltG) && (m_fltA == oPoint.m_fltA) )
		return true;
	return false;
};

/**
\brief	!= casting operator.

\author	dcofer
\date	5/3/2011

\param	oPoint	The point. 

\return	The result of the operation.
**/
bool CStdColor::operator!=(const CStdColor &oPoint)
{
	if( (m_fltR == oPoint.m_fltR) && (m_fltB == oPoint.m_fltB) && (m_fltG == oPoint.m_fltG) && (m_fltA == oPoint.m_fltA) )
		return false;
	return true;
};

/**
\brief	= casting operator.

\author	dcofer
\date	5/3/2011

\param	oPoint	The point. 
**/
void CStdColor::operator=(const CStdColor &oPoint)
{
	m_fltR=oPoint.m_fltR;
	m_fltG=oPoint.m_fltG;
	m_fltB=oPoint.m_fltB;
	m_fltA=oPoint.m_fltA;
};

/**
\brief	+= casting operator.

\author	dcofer
\date	5/3/2011

\param	oPoint	The point. 
**/
void CStdColor::operator+=(const CStdColor &oPoint)
{
	m_fltR+=oPoint.m_fltR;
	m_fltG+=oPoint.m_fltG;
	m_fltB+=oPoint.m_fltB;
	m_fltA+=oPoint.m_fltA;
};

/**
\brief	-= casting operator.

\author	dcofer
\date	5/3/2011

\param	oPoint	The point. 
**/
void CStdColor::operator-=(const CStdColor &oPoint)
{
	m_fltR-=oPoint.m_fltR;
	m_fltG-=oPoint.m_fltG;
	m_fltB-=oPoint.m_fltB;
	m_fltA-=oPoint.m_fltA;
};

/**
\brief	+ casting operator.

\author	dcofer
\date	5/3/2011

\param	oPoint	The point. 

\return	The result of the operation.
**/
CStdColor CStdColor::operator+(const CStdColor &oPoint)
{
	CStdColor oNewPoint(m_fltMaxRange);

	oNewPoint.m_fltR = m_fltR + oPoint.m_fltR;
	oNewPoint.m_fltG = m_fltG + oPoint.m_fltG;
	oNewPoint.m_fltB = m_fltB + oPoint.m_fltB;
	oNewPoint.m_fltA = m_fltA + oPoint.m_fltA;
	return oNewPoint;
};

/**
\brief	- casting operator.

\author	dcofer
\date	5/3/2011

\param	oPoint	The point. 

\return	The result of the operation.
**/
CStdColor CStdColor::operator-(const CStdColor &oPoint)
{
	CStdColor oNewPoint(m_fltMaxRange);

	oNewPoint.m_fltR = m_fltR - oPoint.m_fltR;
	oNewPoint.m_fltG = m_fltG - oPoint.m_fltG;
	oNewPoint.m_fltB = m_fltB - oPoint.m_fltB;
	oNewPoint.m_fltA = m_fltA - oPoint.m_fltA;
	return oNewPoint;
};

/**
\brief	+= casting operator.

\author	dcofer
\date	5/3/2011

\param	fltVal	The value. 
**/
void CStdColor::operator+=(const float fltVal)
{
	m_fltR+=fltVal;
	m_fltG+=fltVal;
	m_fltB+=fltVal;
	m_fltA+=fltVal;
};

/**
\brief	-= casting operator.

\author	dcofer
\date	5/3/2011

\param	fltVal	The value. 
**/
void CStdColor::operator-=(const float fltVal)
{
	m_fltR-=fltVal;
	m_fltG-=fltVal;
	m_fltB-=fltVal;
	m_fltA-=fltVal;
};

/**
\brief	*= casting operator.

\author	dcofer
\date	5/3/2011

\param	fltVal	The value. 
**/
void CStdColor::operator*=(const float fltVal)
{
	m_fltR*=fltVal;
	m_fltG*=fltVal;
	m_fltB*=fltVal;
	m_fltA*=fltVal;
};

/**
\brief	/= casting operator.

\author	dcofer
\date	5/3/2011

\param	fltVal	The value. 
**/
void CStdColor::operator/=(const float fltVal)
{
	if(fltVal)
	{
		m_fltR/=fltVal;
		m_fltG/=fltVal;
		m_fltB/=fltVal;
		m_fltA/=fltVal;
	}
	else
		THROW_ERROR(Std_Err_lDivByZero, Std_Err_strDivByZero);
};

/**
\brief	+ casting operator.

\author	dcofer
\date	5/3/2011

\param	fltVal	The value. 

\return	The result of the operation.
**/
CStdColor CStdColor::operator+(const float fltVal)
{
	CStdColor oNewPoint(m_fltMaxRange);

	oNewPoint.m_fltR = m_fltR + fltVal;
	oNewPoint.m_fltG = m_fltG + fltVal;
	oNewPoint.m_fltB = m_fltB + fltVal;
	oNewPoint.m_fltA = m_fltA + fltVal;
	return oNewPoint;
};

/**
\brief	- casting operator.

\author	dcofer
\date	5/3/2011

\param	fltVal	The value. 

\return	The result of the operation.
**/
CStdColor CStdColor::operator-(const float fltVal)
{
	CStdColor oNewPoint(m_fltMaxRange);

	oNewPoint.m_fltR = m_fltR - fltVal;
	oNewPoint.m_fltG = m_fltG - fltVal;
	oNewPoint.m_fltB = m_fltB - fltVal;
	oNewPoint.m_fltA = m_fltA - fltVal;
	return oNewPoint;
};

/**
\brief	* casting operator.

\author	dcofer
\date	5/3/2011

\param	fltVal	The value. 

\return	The result of the operation.
**/
CStdColor CStdColor::operator*(const float fltVal)
{
	CStdColor oNewPoint(m_fltMaxRange);

	oNewPoint.m_fltR = m_fltR * fltVal;
	oNewPoint.m_fltG = m_fltG * fltVal;
	oNewPoint.m_fltB = m_fltB * fltVal;
	oNewPoint.m_fltA = m_fltA * fltVal;
	return oNewPoint;
};

/**
\brief	/ casting operator.

\author	dcofer
\date	5/3/2011

\param	fltVal	The value. 

\return	The result of the operation.
**/
CStdColor CStdColor::operator/(const float fltVal)
{
	if(!fltVal)
		THROW_ERROR(Std_Err_lDivByZero, Std_Err_strDivByZero);

	CStdColor oNewPoint(m_fltMaxRange);

	oNewPoint.m_fltR = m_fltR / fltVal;
	oNewPoint.m_fltG = m_fltG / fltVal;
	oNewPoint.m_fltB = m_fltB / fltVal;
	oNewPoint.m_fltA = m_fltA / fltVal;
	return oNewPoint;
};

/**
\brief	Gets the magnitude of the color.

\author	dcofer
\date	5/3/2011

\return	Magnitude.
**/
double CStdColor::Magnitude()
{return sqrt( (m_fltR*m_fltR) + (m_fltB*m_fltB) + (m_fltG*m_fltG) + (m_fltA*m_fltA) );};

/**
\brief	Normalizes the color.

\author	dcofer
\date	5/3/2011
**/
void CStdColor::Normalize()
{
	double dblMag = Magnitude();

	if(dblMag > 0)
	{
		m_fltR/=dblMag;
		m_fltG/=dblMag;
		m_fltB/=dblMag;
		m_fltA/=dblMag;
	}
	else
	{
		m_fltR=1;
		m_fltG=0;
		m_fltB=0;
		m_fltA=1;
	}
};


/**
\brief	Clears the near zero described by fltTolerance.

\details This method checks each value to see if it is less than m_fltA give tolerance.
If it is then it just sets it to zero.

\author	dcofer
\date	5/3/2011

\param	fltTolerance	The tolerance. 
**/
void CStdColor::ClearNearZero(float fltTolerance)
{
	if(fabs(m_fltR) < fltTolerance)
		m_fltR = 0;
	if(fabs(m_fltG) < fltTolerance)
		m_fltG = 0;
	if(fabs(m_fltB) < fltTolerance)
		m_fltB = 0;
	if(fabs(m_fltA) < fltTolerance)
		m_fltA = 0;
}

/**
\brief	[] casting operator.

\author	dcofer
\date	5/3/2011

\param	iIndex	Zero-based index of the. 

\return	The result of the operation.
**/
float CStdColor::operator[](const int iIndex)
{
	switch(iIndex)
	{
	case 0:
		return m_fltR;
	case 1:
		return m_fltG;
	case 2:
		return m_fltB;
	case 3:
		return m_fltA;
	default:
		THROW_PARAM_ERROR(Std_Err_lInvalidIndex, Std_Err_strInvalidIndex, "Index", iIndex);
	}
	return 0;
};

/**
\brief	Loads the color.

\author	dcofer
\date	5/3/2011

\param [in,out]	oXml	The xml used to load data. 
\param	strParamName	Name of the string parameter. 
\param	bThrowError 	true to throw error if there is a problem. 
**/
void CStdColor::Load(CStdXml &oXml, string strParamName, BOOL bThrowError)
{
	if(oXml.FindChildElement(strParamName, bThrowError))
	{
		oXml.IntoChildElement(strParamName);
		r(oXml.GetAttribFloat("Red"), bThrowError);
		g(oXml.GetAttribFloat("Green"), bThrowError);
		b(oXml.GetAttribFloat("Blue"), bThrowError);
		a(oXml.GetAttribFloat("Alpha"), bThrowError);
		oXml.OutOfElem();
	}
	else if(bThrowError)
		THROW_TEXT_ERROR(Std_Err_lParamNotFound, Std_Err_strParamNotFound, strParamName);
	//If not found then use the default settings.
}

/**
\brief	Loads the color.

\author	dcofer
\date	5/3/2011

\param	strXml			The xml used to load data. 
\param	strParamName	Name of the string parameter. 
\param	bThrowError 	true to throw error if there is a problem. 
**/
void CStdColor::Load(string strXml, string strParamName, BOOL bThrowError)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement(strParamName);
	
	Load(oXml, strParamName, bThrowError);
}


}				//StdUtils
