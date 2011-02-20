#pragma once

class STD_UTILS_PORT CStdColor
{
protected:
	float m_fltR;
	float m_fltB;
	float m_fltG;
	float m_fltA;

	//some colors are 0-255, others are 0-1. This specifies the max range for these colors
	float m_fltMaxRange;

public:
	CStdColor::CStdColor();
	CStdColor::CStdColor(float fltMaxRange);
	CStdColor::CStdColor(float valx, float valy, float valz, float vala, float fltMaxRange);

	void r(float fltR, BOOL bThrowError = TRUE)
	{
		if(Std_InValidRange((float) 0, (float) 1, fltR, bThrowError, "R"))
			m_fltR = fltR;
	};
	float r() {return m_fltR;};

	void g(float fltG, BOOL bThrowError = TRUE)
	{
		if(Std_InValidRange((float) 0, (float) 1, fltG, bThrowError, "G"))
			m_fltG = fltG;
	};
	float g() {return m_fltG;};

	void b(float fltB, BOOL bThrowError = TRUE)
	{
		if(Std_InValidRange((float) 0, (float) 1, fltB, bThrowError, "B"))
			m_fltB = fltB;
	};
	float b() {return m_fltB;};

	void a(float fltA, BOOL bThrowError = TRUE)
	{
		if(Std_InValidRange((float) 0, (float) 1, fltA, bThrowError, "A"))
			m_fltA = fltA;
	};
	float a() {return m_fltA;};

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
