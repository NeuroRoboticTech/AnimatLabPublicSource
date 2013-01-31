/**
\file	LengthTensionGain.cpp

\brief	Implements an inverted quadratic gain class used to calculate length-tension relationship for muscle. 
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Gain.h"
#include "LengthTensionGain.h"

namespace AnimatSim
{
	namespace Gains
	{

/**
\brief	Default constructor. 

\author	dcofer
\date	3/16/2011
**/
LengthTensionGain::LengthTensionGain()
{
	m_fltRestingLength = 0;
	m_fltTLwidth = 0;
	m_fltTLc = 0;
	m_fltSeRestLength = 0;
	m_fltPeLengthPercentage = 90;
	m_fltMinPeLengthPercentage = 5;
	m_fltMinPeLength = 0;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/16/2011
**/
LengthTensionGain::~LengthTensionGain()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of LengthTensionGain\r\n", "", -1, FALSE, TRUE);}
}


float LengthTensionGain::RestingLength() {return m_fltRestingLength;}

void LengthTensionGain::RestingLength(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "RestingLength");
	m_fltRestingLength = fltVal;
	m_fltSeRestLength = m_fltRestingLength - (m_fltRestingLength * m_fltPeLengthPercentage);
	m_fltMinPeLength = m_fltRestingLength * m_fltMinPeLengthPercentage;
}

float LengthTensionGain::TLwidth() {return m_fltTLwidth;}

void LengthTensionGain::TLwidth(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "TLwidth");
	m_fltTLwidth = fltVal;
	m_fltTLc = pow(m_fltTLwidth, 2);
}

float LengthTensionGain::TLc() {return m_fltTLc;}

float LengthTensionGain::CalculateGain(float fltInput)
{
	float fltLceNorm = fltInput - m_fltRestingLength;
	float fltTl = (-(pow(fltLceNorm, 2)/m_fltTLc)  + 1);
	if(fltTl<0) fltTl = 0;
	return fltTl;
}
/**
\brief	Gets the pe length percentage.

\author	dcofer
\date	5/20/2011

\return	percentage.
**/
float LengthTensionGain::PeLengthPercentage() {return m_fltPeLengthPercentage;}

/**
\brief	Sets the pe length percentage.

\author	dcofer
\date	5/20/2011

\param	fltVal	The new value.
**/
void LengthTensionGain::PeLengthPercentage(float fltVal)
{
	//We need to scale it because this comes in as 0-100, but we need it in 0-1.
	fltVal = fltVal/100;

	Std_InValidRange((float) 0, (float) 1, fltVal, TRUE, "PeLengthPercentage");
	m_fltPeLengthPercentage = fltVal;
	m_fltSeRestLength = m_fltRestingLength - (m_fltRestingLength * m_fltPeLengthPercentage);
}

/**
\brief	Gets the minimum pe length percentage.

\author	dcofer
\date	5/20/2011

\return	percentage.
**/
float LengthTensionGain::MinPeLengthPercentage() {return m_fltMinPeLengthPercentage;}

/**
\brief	Sets the minimum pe length percentage.

\author	dcofer
\date	5/20/2011

\param	fltVal	The new value.
**/
void LengthTensionGain::MinPeLengthPercentage(float fltVal)
{
	//We need to scale it because this comes in as 0-100, but we need it in 0-1.
	fltVal = fltVal/100;

	Std_InValidRange((float) 0, m_fltPeLengthPercentage, fltVal, TRUE, "MinPeLengthPercentage");
	m_fltMinPeLengthPercentage = fltVal;
	m_fltMinPeLength = m_fltRestingLength * m_fltMinPeLengthPercentage;
}

/**
\brief	Gets the se rest length.

\author	dcofer
\date	5/20/2011

\return	length.
**/
float LengthTensionGain::SeRestLength() {return m_fltSeRestLength;}

/**
\brief	Gets the minimum pe length.

\author	dcofer
\date	5/20/2011

\return	length.
**/
float LengthTensionGain::MinPeLength() {return m_fltMinPeLength;}

BOOL LengthTensionGain::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	if(Gain::SetData(strDataType, strValue, false))
		return true;

	if(strDataType == "RESTINGLENGTH")
	{
		RestingLength(atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "LWIDTH")
	{
		TLwidth(atof(strValue.c_str()));
		return true;
	}

	if(strDataType == "PELENGTHPERCENTAGE")
	{
		PeLengthPercentage(atof(strValue.c_str()));
		return true;
	}
	
	if(strDataType == "MINPELENGTHPERCENTAGE")
	{
		MinPeLengthPercentage(atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void LengthTensionGain::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	Gain::QueryProperties(aryNames, aryTypes);

	aryNames.Add("RestingLength");
	aryTypes.Add("Float");

	aryNames.Add("Lwidth");
	aryTypes.Add("Float");

	aryNames.Add("PeLengthPercentage");
	aryTypes.Add("Float");

	aryNames.Add("MinPeLengthPercentage");
	aryTypes.Add("Float");
}

void LengthTensionGain::Load(CStdXml &oXml)
{
	Gain::Load(oXml);

	oXml.IntoElem();  //Into Adapter Element

	RestingLength(oXml.GetChildFloat("RestingLength"));
	TLwidth(oXml.GetChildFloat("Lwidth"));
	PeLengthPercentage(oXml.GetChildFloat("PeLength"));
	MinPeLengthPercentage(oXml.GetChildFloat("MinPeLength"));

	oXml.OutOfElem(); //OutOf Adapter Element
}

	}			//Gains
}			//AnimatSim
