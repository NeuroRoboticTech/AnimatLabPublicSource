/**
\file	CStdPID.cpp

\brief	Implements the proportional-integral-derivative controller class.
**/

#include "StdAfx.h"
#include "StdPID.h"

namespace StdUtils
{

#define DEGTORAD    ((double)3.1415926535/180.0)

CStdPID::CStdPID(void)
{
    FullReset();
}

CStdPID::CStdPID(float fltSetpoint, float fltGain, float fltIntegralAct, float fltDerivativeAct, 
    bool bComplexError, bool bAntiResetWindup, bool bRampLimit, 
    float fltRangeMax, float fltRangeMin, float fltARWBound, float fltRampGradient)
{
    FullReset();

    Setpoint(fltSetpoint);
    Gain(fltGain);
    IntegralAct(fltIntegralAct);
    DerivativeAct(fltDerivativeAct);
    ComplexError(bComplexError);
    AntiResetWindup(bAntiResetWindup);
    RampLimit(bRampLimit);
    RangeMax(fltRangeMax);
    RangeMin(fltRangeMin);
    ARWBound(fltARWBound);
    RampGradient(fltRampGradient);
}


CStdPID::~CStdPID(void)
{
}


void CStdPID::FullReset()
{
    m_fltError = 0;
    m_fltSetpoint = 0;
    m_bComplexError = true;
    m_bAntiResetWindup = true;
    m_bRampLimit = true;
    m_fltErrorChange = 0;

    m_fltGain = 0;
    m_fltIntegralAct = 0;

    m_fltProportional = 0;
    m_fltIntegral = 0;
    m_fltOldIntegral = 0;
    m_fltDerivative = 0;
    m_fltDerivativeAct = 0;

    m_fltOutput = 0;
    m_fltOldOutput = 0;

    m_fltRangeMax = 0;
    m_fltRangeMin = 0;
    m_fltRange = 0;
    m_fltARWBound = 0;
    m_fltRampGradient = 0;

    //Initialize with 3 spots
    m_aryOldErrors.Clear();
    m_aryOldErrors.Add(0);
    m_aryOldErrors.Add(0);
    m_aryOldErrors.Add(0);
}

void CStdPID::ResetVars()
{
    m_fltError = 0;
    m_fltSetpoint = 0;
    m_fltErrorChange = 0;

    m_fltProportional = 0;
    m_fltIntegral = 0;
    m_fltOldIntegral = 0;
    m_fltDerivative = 0;

    m_fltOutput = 0;
    m_fltOldOutput = 0;

    //Initialize with 3 spots
    m_aryOldErrors.Clear();
    m_aryOldErrors.Add(0);
    m_aryOldErrors.Add(0);
    m_aryOldErrors.Add(0);
}

float CStdPID::Error() {return m_fltError;}

float CStdPID::ErrorChange() {return m_fltErrorChange;}
        
void CStdPID::Setpoint(float fltVal) {m_fltSetpoint = fltVal;}

float CStdPID::Setpoint() {return m_fltSetpoint;}
        
void CStdPID::Gain(float fltVal) 
{
    if(fltVal >= 0)
        m_fltGain = fltVal;
}

float CStdPID::Gain() {return m_fltGain;}

void CStdPID::IntegralAct(float fltVal) 
{
    if(fltVal >= 0)
        m_fltIntegralAct = fltVal;
}

float CStdPID::IntegralAct() {return m_fltIntegralAct;}

void CStdPID::DerivativeAct(float fltVal) 
{
    if(fltVal >= 0)
        m_fltDerivativeAct = fltVal;
}

float CStdPID::DerivativeAct() {return m_fltDerivativeAct;}

float CStdPID::Proportional() {return m_fltProportional;}

float CStdPID::Integral() {return m_fltIntegral;}

float CStdPID::OldIntegral() {return m_fltOldIntegral;}

float CStdPID::Derivative() {return m_fltDerivative;}

float CStdPID::Output() {return m_fltOutput;}

float CStdPID::OldOutput() {return m_fltOldOutput;}

void CStdPID::ComplexError(bool bVal) {m_bComplexError = bVal;}

bool CStdPID::ComplexError() {return m_bComplexError;}

void CStdPID::AntiResetWindup(bool bVal) {m_bAntiResetWindup = bVal;}

bool CStdPID::AntiResetWindup() {return m_bAntiResetWindup;}

void CStdPID::RampLimit(bool bVal) {m_bRampLimit = bVal;}

bool CStdPID::RampLimit() {return m_bRampLimit;}

void CStdPID::RangeMax(float fltVal) 
{
    if(fltVal >= m_fltRangeMin)
    {
        m_fltRangeMax = fltVal;
        m_fltRange = m_fltRangeMax - m_fltRangeMin;
    }
}

float CStdPID::RangeMax() {return m_fltRangeMax;}

void CStdPID::RangeMin(float fltVal) 
{
    if(fltVal <= m_fltRangeMax)
    {
        m_fltRangeMin = fltVal;
        m_fltRange = m_fltRangeMax - m_fltRangeMin;
    }
}

float CStdPID::RangeMin() {return m_fltRangeMin;}

float CStdPID::Range() {return m_fltRange;}

/**
 \brief Sets the anti-reset boundary. This is specified as a percentage of the 
 entire output range. A limit of zero percent would make output ARW impossible 
 and a limit of 50 percent or greater would allow maximum ARW. 

 \author    David Cofer
 \date  1/10/2014

 \param fltVal  The new value.
 */
void CStdPID::ARWBound(float fltVal) 
{
    if(fltVal >= 0 && fltVal <= 1)
        m_fltARWBound = fltVal;
}

float CStdPID::ARWBound() {return m_fltARWBound;}

/**
 \brief Ramp gradient.

 \author    David Cofer
 \date  1/10/2014

 \param fltVal  The new degree value.
 */
void CStdPID::RampGradient(float fltVal) 
{
    if(fltVal >= 0 && fltVal <= 90)
        m_fltRampGradient = fltVal;
}

float CStdPID::RampGradient() {return m_fltRampGradient;}


// Calculate PID
float CStdPID::Calculate(float fltDt, float fltInput)
{
	float fltError1;
	float fltError2;
	float fltError3;
	float fltChange;
	float fltMaxChange;

	// Error
	m_fltError = m_fltSetpoint-fltInput;

	// Use simple error change or more stable average error change
	if(!m_bComplexError)
	{
		fltError1 = m_aryOldErrors.GetAt(2);
		m_fltErrorChange = m_fltError-fltError1;
	}
	else
	{
		fltError1 = m_aryOldErrors.GetAt(2);
		fltError2 = m_aryOldErrors.GetAt(1);
		fltError3 = m_aryOldErrors.GetAt(0);
		m_fltErrorChange = (m_fltError+(3*fltError1)-(3*fltError2)-fltError3)/6;
	}

	// Proportional
	m_fltProportional = m_fltGain * m_fltError;

	// Integral
	m_fltIntegral = m_fltIntegral + (m_fltGain * (m_fltIntegralAct*fltDt) * m_fltError);

	// Derivative
	m_fltDerivative = m_fltGain * (m_fltDerivativeAct/fltDt) * m_fltErrorChange;

	// Calculate Output
	m_fltOutput = (m_fltProportional + m_fltIntegral + m_fltDerivative);

	// Perform Anti-reset windup?
	if(m_bAntiResetWindup)
	{
		if(m_fltOutput >= m_fltRangeMax-(m_fltARWBound*m_fltRange))
		{
			m_fltOutput = m_fltRangeMax-(m_fltARWBound*m_fltRange);
			m_fltIntegral = m_fltOldIntegral;
		}
		else if(m_fltOutput <= m_fltRangeMin+(m_fltARWBound*m_fltRange))
		{
			m_fltOutput = m_fltRangeMin+(m_fltARWBound*m_fltRange);
			m_fltIntegral = m_fltOldIntegral;
		}
	}

	// Perform Ramp limiting?
	if(m_bRampLimit)
	{
		fltMaxChange = (float)tan((((double)m_fltRampGradient)*DEGTORAD)/fltDt);
		fltChange = m_fltOutput - m_fltOldOutput;
		if(fltChange > fltMaxChange)
		{
			m_fltOutput = m_fltOldOutput + fltMaxChange;

			// limit integral?
			fltChange = (float)fabs(m_fltIntegral - m_fltOldIntegral);
			if(fltChange > fltMaxChange)
			{
				m_fltIntegral = m_fltOldIntegral + fltMaxChange;
			}
		}
		else if(fltChange < (0-fltMaxChange))
		{
			m_fltOutput = m_fltOldOutput - fltMaxChange;

			// limit integral?
			fltChange = (float)fabs(m_fltIntegral - m_fltOldIntegral);
			if(fltChange > fltMaxChange)
			{
				m_fltIntegral = m_fltOldIntegral - fltMaxChange;
			}
		}
	}

    m_aryOldErrors.AddEnd(m_fltError);

	m_fltOldIntegral = m_fltIntegral;

	m_fltOldOutput = m_fltOutput;

    return m_fltOutput;
}

//void CStdPID::Load(CStdXml &oXml)
//{
//	AnimatBase::Load(oXml);
//
//	oXml.IntoElem();  //Into Joint Element
//
//    Setpoint(oXml.GetChildFloat("Setpoint", m_fltSetpoint));
//    Gain(oXml.GetChildFloat("Gain", m_fltGain));
//    IntegralAct(oXml.GetChildFloat("IntegralAct", m_fltIntegralAct));
//    DerivativeAct(oXml.GetChildFloat("DerivativeAct", m_fltDerivativeAct));
//    ComplexError(oXml.GetChildBool("ComplexError", m_bComplexError));
//    AntiResetWindup(oXml.GetChildBool("ARW", m_bAntiResetWindup));
//    RampLimit(oXml.GetChildBool("RampLimit", m_bRampLimit));
//    RangeMax(oXml.GetChildFloat("RangeMax", m_fltRangeMax));
//    RangeMin(oXml.GetChildFloat("RangeMin", m_fltRangeMin));
//    ARWBound(oXml.GetChildFloat("ARWBound", m_fltARWBound));
//    RampGradient(oXml.GetChildFloat("RampGradient", m_fltRampGradient));
//
//	oXml.OutOfElem(); //OutOf Joint Element
//}

}				//StdUtils