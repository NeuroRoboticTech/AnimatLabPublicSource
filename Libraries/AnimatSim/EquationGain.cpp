/**
\file	EquationGain.cpp

\brief	Implements the equation gain class. 
**/

#include "stdafx.h"
#include "IBodyPartCallback.h"
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
{Std_TraceMsg(0, "Caught Error in desctructor of EquationGain\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Gets the post-fix gain equation. 

\author	dcofer
\date	3/16/2011

\return	Gain equation string. 
**/
string EquationGain::GainEquation() {return m_strGainEquation;}

/**
\brief	Sets the gain equation. 

\author	dcofer
\date	3/16/2011

\param	strEquation	The post fix gain equation string. 
**/
void EquationGain::GainEquation(string strEquation)
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

void EquationGain::Load(CStdXml &oXml)
{
	Gain::Load(oXml);

	oXml.IntoElem();  //Into Adapter Element

	GainEquation(oXml.GetChildString("Equation"));

	oXml.OutOfElem(); //OutOf Adapter Element
}

	}			//Gains
}			//AnimatSim
