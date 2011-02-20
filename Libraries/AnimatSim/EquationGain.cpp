// EquationGain.cpp: implementation of the EquationGain class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Gain.h"
#include "EquationGain.h"

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
namespace AnimatSim
{
	namespace Gains
	{

/*! \brief 
   Constructs an structure object..
   		
	 \return
	 No return value.

   \remarks
	 The constructor for a structure. 
*/

EquationGain::EquationGain()
{
	m_lpEval = NULL;
}


/*! \brief 
   Destroys the structure object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the structure object..	 
*/

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

	m_strGainEquation = oXml.GetChildString("Equation");

	oXml.OutOfElem(); //OutOf Adapter Element

	//Initialize the postfix evaluator.
	if(m_lpEval) 
	{delete m_lpEval; m_lpEval = NULL;}

	m_lpEval = new CStdPostFixEval;

	m_lpEval->AddVariable("x");
	m_lpEval->Equation(m_strGainEquation);
}

	}			//Gains
}			//AnimatSim
