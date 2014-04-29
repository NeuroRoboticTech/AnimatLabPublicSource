/**
\file	BellGain.cpp

\brief	Implements the bell gain class. 
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"
#include "Gain.h"
#include "BellGain.h"


namespace AnimatSim
{
	namespace Gains
	{
/**
\brief	Default constructor. 

\author	dcofer
\date	3/16/2011
**/
BellGain::BellGain()
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
BellGain::~BellGain()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of BellGain\r\n", "", -1, false, true);}
}

/**
\brief	Gets A parameter of bell eqation: Out = B*e^(-C*(In-A)^2)+D

\author	dcofer
\date	3/16/2011

\return	A param. 
**/
float BellGain::A() {return m_fltA;}

/**
\brief	Sets A parameter of bell eqation: Out = B*e^(-C*(In-A)^2)+D

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void BellGain::A(float fltVal) {m_fltA = fltVal;}

/**
\brief	Gets B parameter of bell eqation: Out = B*e^(-C*(In-A)^2)+D

\author	dcofer
\date	3/16/2011

\return	B param. 
**/
float BellGain::B() {return m_fltB;}

/**
\brief	Sets B parameter of bell eqation: Out = B*e^(-C*(In-A)^2)+D

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void BellGain::B(float fltVal) {m_fltB = fltVal;}

/**
\brief	Gets C parameter of bell eqation: Out = B*e^(-C*(In-A)^2)+D

\author	dcofer
\date	3/16/2011

\return	C param. 
**/
float BellGain::C() {return m_fltC;}

/**
\brief	Sets C parameter of bell eqation: Out = B*e^(-C*(In-A)^2)+D

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void BellGain::C(float fltVal) {m_fltC = fltVal;}

/**
\brief	Sets D parameter of bell eqation: Out = B*e^(-C*(In-A)^2)+D

\author	dcofer
\date	3/16/2011

\return	D param. 
**/
float BellGain::D() {return m_fltD;}

/**
\brief	Sets D parameter of bell eqation: Out = B*e^(-C*(In-A)^2)+D

\author	dcofer
\date	3/16/2011

\param	fltVal	The new value. 
**/
void BellGain::D(float fltVal) {m_fltD = fltVal;}

float BellGain::CalculateGain(float fltInput)
{
	float fltVal = 0;

	if(InLimits(fltInput))
	{
		if(m_fltB) 
		{
			fltVal = pow((float) (fltInput-m_fltA), (float) 2.0);
			fltVal = exp(-m_fltC * fltVal);
			fltVal = (m_fltB * fltVal) + m_fltD;	
		}
	}
	else
		fltVal = CalculateLimitOutput(fltInput);

	return fltVal;
}

bool BellGain::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
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

void BellGain::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Gain::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("A", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("B", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("C", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("D", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
}

void BellGain::Load(CStdXml &oXml)
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
