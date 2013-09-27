/**
\file	PolynomialGain.cpp

\brief	Implements the polynomial gain class. 
**/

#include "StdAfx.h"
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
{Std_TraceMsg(0, "Caught Error in desctructor of PolynomialGain\r\n", "", -1, false, true);}
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

bool PolynomialGain::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	if(Gain::SetData(strDataType, strValue, false))
		return true;

	if(strDataType == "A")
	{
		A((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "B")
	{
		B((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "C")
	{
		C((float) atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "D")
	{
		D((float) atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void PolynomialGain::QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes)
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
