/**
\file	PolynomialGain.cpp

\brief	Implements the polynomial gain class. 
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Gain.h"
#include "PolynomialGain.h"


namespace AnimatSim
{
	namespace Gains
	{
/**
\brief	Default constructor. 

\author	dcofer
\date	3/16/2011
**/
PolynomialGain::PolynomialGain()
{
	m_fltA = 0;
	m_fltB = 0;
	m_fltC = 0;
	m_fltD = 0;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/16/2011
**/
PolynomialGain::~PolynomialGain()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of PolynomialGain\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Gets A parameter of polynomial eqation: Out = A*In^3 + B*In^2 + C*In + D

\author	dcofer
\date	3/16/2011

\return	A param. 
**/
float PolynomialGain::A() {return m_fltA;}

/**
\brief	Sets A parameter of polynomial eqation: Out = A*In^3 + B*In^2 + C*In + D

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void PolynomialGain::A(float fltVal) {m_fltA = fltVal;}

/**
\brief	Gets B parameter of polynomial eqation: Out = A*In^3 + B*In^2 + C*In + D

\author	dcofer
\date	3/16/2011

\return	B param. 
**/
float PolynomialGain::B() {return m_fltB;}

/**
\brief	Sets B parameter of polynomial eqation: Out = A*In^3 + B*In^2 + C*In + D

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void PolynomialGain::B(float fltVal) {m_fltB = fltVal;}

/**
\brief	Gets C parameter of polynomial eqation: Out = A*In^3 + B*In^2 + C*In + D

\author	dcofer
\date	3/16/2011

\return	C param. 
**/
float PolynomialGain::C() {return m_fltC;}

/**
\brief	Sets C parameter of polynomial eqation: Out = A*In^3 + B*In^2 + C*In + D

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void PolynomialGain::C(float fltVal) {m_fltC = fltVal;}

/**
\brief	Sets D parameter of polynomial eqation: Out = A*In^3 + B*In^2 + C*In + D

\author	dcofer
\date	3/16/2011

\return	D param. 
**/
float PolynomialGain::D() {return m_fltD;}

/**
\brief	Sets D parameter of polynomial eqation: Out = A*In^3 + B*In^2 + C*In + D

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void PolynomialGain::D(float fltVal) {m_fltD = fltVal;}

float PolynomialGain::CalculateGain(float fltInput)
{
	//Gain = A*x^3 + B*x^2 + C*x + D
	if(InLimits(fltInput))
		return ((m_fltA*fltInput*fltInput*fltInput) + (m_fltB*fltInput*fltInput) + (m_fltC*fltInput) + m_fltD);
	else
		return CalculateLimitOutput(fltInput);
}	

BOOL PolynomialGain::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(Gain::SetData(strDataType, strValue, false))
		return true;

	if(strDataType == "A")
	{
		A(atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "B")
	{
		B(atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "C")
	{
		C(atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "D")
	{
		D(atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void PolynomialGain::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	Gain::QueryProperties(aryNames, aryTypes);

	aryNames.Add("A");
	aryTypes.Add("Float");

	aryNames.Add("B");
	aryTypes.Add("Float");

	aryNames.Add("C");
	aryTypes.Add("Float");

	aryNames.Add("D");
	aryTypes.Add("Float");
}

void PolynomialGain::Load(CStdXml &oXml)
{
	Gain::Load(oXml);

	oXml.IntoElem();  //Into Adapter Element

	A(oXml.GetChildFloat("A"));
	B(oXml.GetChildFloat("B"));
	C(oXml.GetChildFloat("C"));
	D(oXml.GetChildFloat("D"));

	oXml.OutOfElem(); //OutOf Adapter Element
}

	}			//Gains
}			//AnimatSim
