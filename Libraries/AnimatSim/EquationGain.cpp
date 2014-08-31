/**
\file	EquationGain.cpp

\brief	Implements the equation gain class. 
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Gain.h"
#include "EquationGain.h"


namespace AnimatSim
{
	namespace Gains
	{

/**
\brief	Default constructor. 

\author	dcofer
\date	3/16/2011
**/
EquationGain::EquationGain()
{
	m_lpEval = NULL;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/16/2011
**/
EquationGain::~EquationGain()
{

try
{
	if(m_lpEval)
		delete m_lpEval;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of EquationGain\r\n", "", -1, false, true);}
}

/**
\brief	Gets the post-fix gain equation. 

\author	dcofer
\date	3/16/2011

\return	Gain equation string. 
**/
std::string EquationGain::GainEquation() {return m_strGainEquation;}

/**
\brief	Sets the gain equation. 

\author	dcofer
\date	3/16/2011

\param	strEquation	The post fix gain equation string. 
**/
void EquationGain::GainEquation(std::string strEquation)
{
	CStdPostFixEval *lpEval = new CStdPostFixEval;

	try
	{
		lpEval->AddVariable("x");
		lpEval->Equation(strEquation);
	}
	catch(CStdErrorInfo oError)
	{
		if(lpEval) delete lpEval;
		RELAY_ERROR(oError);
	}
	catch(...)
	{
		if(lpEval) delete lpEval;
		THROW_PARAM_ERROR(Std_Err_lSettingEquation, Std_Err_strSettingEquation, "Equation", strEquation);
	}

	//Initialize the postfix evaluator.
	if(m_lpEval) 
		{delete m_lpEval; m_lpEval = NULL;}
	m_lpEval = lpEval;
}

void EquationGain::Copy(CStdSerialize *lpSource)
{
	Gain::Copy(lpSource);

	EquationGain *lpOrig = dynamic_cast<EquationGain *>(lpSource);

	m_strGainEquation = lpOrig->m_strGainEquation;

	if(m_lpEval)
	{
		delete m_lpEval;
		m_lpEval = NULL;
	}

	//TODO Need to clone this.
	//if(lpOrig->m_lpEval)
	//	m_lpEval = lpOrig->m_l
}

CStdSerialize *EquationGain::Clone()
{
	CStdSerialize *lpClone = new EquationGain();
	lpClone->Copy(this);
	return lpClone;
}

float EquationGain::CalculateGain(float fltInput)
{
	if(InLimits(fltInput))
	{
		m_lpEval->SetVariable("x", fltInput);
		return m_lpEval->Solve();
	}
	else
		return CalculateLimitOutput(fltInput);
}

bool EquationGain::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	if(Gain::SetData(strDataType, strValue, false))
		return true;

	if(strDataType == "EQUATION")
	{
		GainEquation(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void EquationGain::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	Gain::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Equation", AnimatPropertyType::String, AnimatPropertyDirection::Set));
}

void EquationGain::Load(CStdXml &oXml)
{
	Gain::Load(oXml);

	oXml.IntoElem();  //Into Adapter Element

	GainEquation(oXml.GetChildString("Equation"));

	oXml.OutOfElem(); //OutOf Adapter Element
}

	}			//Gains
}			//AnimatSim
