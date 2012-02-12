/**
\file IonChannelSigmoid.cpp

\brief	Implements the ion channel sigmoid class.
**/

#include "StdAfx.h"
#include "IonChannelSigmoid.h"


namespace IntegrateFireSim
{
	namespace Gains
	{

/**
\brief	Default constructor.

\author	dcofer
\date	3/31/2011
**/
IonChannelSigmoid::IonChannelSigmoid()
{
	m_fltA = 0;
	m_fltB = 0;
	m_fltC = 0;
	m_fltD = 0;
	m_fltE = 0;
	m_fltF = 0;
	m_fltG = 0;
	m_fltH = 1;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/31/2011
**/
IonChannelSigmoid::~IonChannelSigmoid()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of IonChannelSigmoid\r\n", "", -1, FALSE, TRUE);}
}

float IonChannelSigmoid::CalculateGain(float fltInput)
{
	if(InLimits(fltInput))
		return (m_fltA + (m_fltB/(m_fltH + exp(m_fltC*(fltInput+m_fltD)) + m_fltE*exp(m_fltF*(fltInput+m_fltG)) )));
	else
		return CalculateLimitOutput(fltInput);
}

BOOL IonChannelSigmoid::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(Gain::SetData(strDataType, strValue, false))
		return true;

	if(strDataType == "A")
	{
		m_fltA = atof(strValue.c_str());
		return true;
	}

	if(strDataType == "B")
	{
		m_fltB = atof(strValue.c_str());
		return true;
	}

	if(strDataType == "C")
	{
		m_fltC = atof(strValue.c_str());
		return true;
	}

	if(strDataType == "D")
	{
		m_fltD = atof(strValue.c_str());
		return true;
	}

	if(strDataType == "E")
	{
		m_fltE = atof(strValue.c_str());
		return true;
	}

	if(strDataType == "F")
	{
		m_fltF = atof(strValue.c_str());
		return true;
	}

	if(strDataType == "G")
	{
		m_fltG = atof(strValue.c_str());
		return true;
	}

	if(strDataType == "H")
	{
		m_fltH = atof(strValue.c_str());
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void IonChannelSigmoid::Load(CStdXml &oXml)
{
	Gain::Load(oXml);

	oXml.IntoElem();  //Into Adapter Element

	m_fltA = oXml.GetChildFloat("A");
	m_fltB = oXml.GetChildFloat("B");
	m_fltC = oXml.GetChildFloat("C");
	m_fltD = oXml.GetChildFloat("D");
	m_fltE = oXml.GetChildFloat("E");
	m_fltF = oXml.GetChildFloat("F");
	m_fltG = oXml.GetChildFloat("G");
	m_fltH = oXml.GetChildFloat("H");

	oXml.OutOfElem(); //OutOf Adapter Element
}

	}			//Gains
}			//AnimatSim
