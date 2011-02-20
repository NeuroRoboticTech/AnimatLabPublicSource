// BellGain.cpp: implementation of the BellGain class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"
#include "Gain.h"
#include "BellGain.h"

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

BellGain::BellGain()
{
	m_fltA = 0;
	m_fltB = 0;
	m_fltC = 0;
	m_fltD = 0;
}


/*! \brief 
   Destroys the structure object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the structure object..	 
*/

BellGain::~BellGain()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of BellGain\r\n", "", -1, FALSE, TRUE);}
}

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

void BellGain::Load(CStdXml &oXml)
{
	Gain::Load(oXml);

	oXml.IntoElem();  //Into Adapter Element

	m_fltA = oXml.GetChildFloat("A");
	m_fltB = oXml.GetChildFloat("B");
	m_fltC = oXml.GetChildFloat("C");
	m_fltD = oXml.GetChildFloat("D");

	oXml.OutOfElem(); //OutOf Adapter Element
}


	}			//Gains
}			//AnimatSim
