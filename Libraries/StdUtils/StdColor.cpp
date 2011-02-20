#include "StdAfx.h"


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

CStdColor::CStdColor(float valx, float valy, float valz, float vala, float fltMaxRange)
{
	m_fltMaxRange = fltMaxRange;

	r(valx);
	g(valy);
	b(valz);
	a(vala);
}


void CStdColor::Set(float R, float G, float B, float A)
{
	r(R);
	g(G);
	b(B);
	a(A);
}

bool CStdColor::operator==(const CStdColor &oPoint)
{
	if( (m_fltR == oPoint.m_fltR) && (m_fltB == oPoint.m_fltB) && (m_fltG == oPoint.m_fltG) && (m_fltA == oPoint.m_fltA) )
		return true;
	return false;
};

bool CStdColor::operator!=(const CStdColor &oPoint)
{
	if( (m_fltR == oPoint.m_fltR) && (m_fltB == oPoint.m_fltB) && (m_fltG == oPoint.m_fltG) && (m_fltA == oPoint.m_fltA) )
		return false;
	return true;
};

void CStdColor::operator=(const CStdColor &oPoint)
{
	m_fltR=oPoint.m_fltR;
	m_fltG=oPoint.m_fltG;
	m_fltB=oPoint.m_fltB;
	m_fltA=oPoint.m_fltA;
};

void CStdColor::operator+=(const CStdColor &oPoint)
{
	m_fltR+=oPoint.m_fltR;
	m_fltG+=oPoint.m_fltG;
	m_fltB+=oPoint.m_fltB;
	m_fltA+=oPoint.m_fltA;
};

void CStdColor::operator-=(const CStdColor &oPoint)
{
	m_fltR-=oPoint.m_fltR;
	m_fltG-=oPoint.m_fltG;
	m_fltB-=oPoint.m_fltB;
	m_fltA-=oPoint.m_fltA;
};

CStdColor CStdColor::operator+(const CStdColor &oPoint)
{
	CStdColor oNewPoint(m_fltMaxRange);

	oNewPoint.m_fltR = m_fltR + oPoint.m_fltR;
	oNewPoint.m_fltG = m_fltG + oPoint.m_fltG;
	oNewPoint.m_fltB = m_fltB + oPoint.m_fltB;
	oNewPoint.m_fltA = m_fltA + oPoint.m_fltA;
	return oNewPoint;
};

CStdColor CStdColor::operator-(const CStdColor &oPoint)
{
	CStdColor oNewPoint(m_fltMaxRange);

	oNewPoint.m_fltR = m_fltR - oPoint.m_fltR;
	oNewPoint.m_fltG = m_fltG - oPoint.m_fltG;
	oNewPoint.m_fltB = m_fltB - oPoint.m_fltB;
	oNewPoint.m_fltA = m_fltA - oPoint.m_fltA;
	return oNewPoint;
};

void CStdColor::operator+=(const float fltVal)
{
	m_fltR+=fltVal;
	m_fltG+=fltVal;
	m_fltB+=fltVal;
	m_fltA+=fltVal;
};

void CStdColor::operator-=(const float fltVal)
{
	m_fltR-=fltVal;
	m_fltG-=fltVal;
	m_fltB-=fltVal;
	m_fltA-=fltVal;
};

void CStdColor::operator*=(const float fltVal)
{
	m_fltR*=fltVal;
	m_fltG*=fltVal;
	m_fltB*=fltVal;
	m_fltA*=fltVal;
};

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

CStdColor CStdColor::operator+(const float fltVal)
{
	CStdColor oNewPoint(m_fltMaxRange);

	oNewPoint.m_fltR = m_fltR + fltVal;
	oNewPoint.m_fltG = m_fltG + fltVal;
	oNewPoint.m_fltB = m_fltB + fltVal;
	oNewPoint.m_fltA = m_fltA + fltVal;
	return oNewPoint;
};

CStdColor CStdColor::operator-(const float fltVal)
{
	CStdColor oNewPoint(m_fltMaxRange);

	oNewPoint.m_fltR = m_fltR - fltVal;
	oNewPoint.m_fltG = m_fltG - fltVal;
	oNewPoint.m_fltB = m_fltB - fltVal;
	oNewPoint.m_fltA = m_fltA - fltVal;
	return oNewPoint;
};

CStdColor CStdColor::operator*(const float fltVal)
{
	CStdColor oNewPoint(m_fltMaxRange);

	oNewPoint.m_fltR = m_fltR * fltVal;
	oNewPoint.m_fltG = m_fltG * fltVal;
	oNewPoint.m_fltB = m_fltB * fltVal;
	oNewPoint.m_fltA = m_fltA * fltVal;
	return oNewPoint;
};

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

double CStdColor::Magnitude()
{return sqrt( (m_fltR*m_fltR) + (m_fltB*m_fltB) + (m_fltG*m_fltG) + (m_fltA*m_fltA) );};

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

//This method checks each value to see if it is less than m_fltA give tolerance.
//If it is then it just sets it to zero.
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


void CStdColor::Load(string strXml, string strParamName, BOOL bThrowError)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement(strParamName);
	
	Load(oXml, strParamName, bThrowError);
}