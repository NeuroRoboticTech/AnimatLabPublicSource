/**
\file	StdColor.h

\brief	Declares the standard color class.
**/

#pragma once

/**
\namespace	StdUtils

\brief	Namespace for the standard utility objects.
**/
namespace StdUtils
{
/**
\brief	Standard color class. 

\author	dcofer
\date	5/3/2011
**/
class STD_UTILS_PORT CStdColor
{
protected:
	/// The red component of the color.
	float m_fltR;

	/// The blue component of the color.
	float m_fltB;

	/// The green component of the color.
	float m_fltG;

	/// The alpha component of the color.
	float m_fltA;

	///some colors are 0-255, others are 0-1. This specifies the max range for these colors
	float m_fltMaxRange;

public:

	/**
	\brief	Default constructor.

	\author	dcofer
	\date	5/3/2011
	**/
	CStdColor::CStdColor();

	/**
	\brief	Constructor with maximum range.

	\author	dcofer
	\date	5/3/2011

	\param	fltMaxRange	The maximum range used for color values. 
	**/
	CStdColor::CStdColor(float fltMaxRange);

	/**
	\brief	Constructor.

	\author	dcofer
	\date	5/3/2011

	\param	valr	   	The red value. 
	\param	valg	   	The green value. 
	\param	valb	   	The blue value. 
	\param	vala	   	The alpha value. 
	\param	fltMaxRange	The maximum range for the color values. 
	**/
	CStdColor::CStdColor(float valr, float valg, float valb, float vala, float fltMaxRange);

	void r(float fltR, BOOL bThrowError = TRUE);
	float r();

	void g(float fltG, BOOL bThrowError = TRUE);
	float g();

	void b(float fltB, BOOL bThrowError = TRUE);
	float b();

	void a(float fltA, BOOL bThrowError = TRUE);
	float a();

	void Set(float R, float G, float B, float A);
	bool operator==(const CStdColor &oPoint);
	bool operator!=(const CStdColor &oPoint);
	void operator=(const CStdColor &oPoint);
	void operator+=(const CStdColor &oPoint);
	void operator-=(const CStdColor &oPoint);
	CStdColor operator+(const CStdColor &oPoint);
	CStdColor operator-(const CStdColor &oPoint);
	void operator+=(const float fltVal);
	void operator-=(const float fltVal);
	void operator*=(const float fltVal);
	void operator/=(const float fltVal);

	CStdColor operator+(const float fltVal);
	CStdColor operator-(const float fltVal);
	CStdColor operator*(const float fltVal);
	CStdColor operator/(const float fltVal);
	double Magnitude();
	void Normalize();
	void ClearNearZero(float fltTolerance = 1e-5f);
	float operator[](const int iIndex);
	void Load(CStdXml &oXml, string strParamName, BOOL bThrowError = TRUE);
	void Load(string strXml, string strParamName, BOOL bThrowError = TRUE);

};

}				//StdUtils
