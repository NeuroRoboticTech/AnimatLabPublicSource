// PolynomialGain.cpp: implementation of the PolynomialGain class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Gain.h"
#include "PolynomialGain.h"

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

PolynomialGain::PolynomialGain()
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

PolynomialGain::~PolynomialGain()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of PolynomialGain\r\n", "", -1, FALSE, TRUE);}
}

float PolynomialGain::CalculateGain(float fltInput)
{
	//Gain = A*x^3 + B*x^2 + C*x + D
	if(InLimits(fltInput))
		return ((m_fltA*fltInput*fltInput*fltInput) + (m_fltB*fltInput*fltInput) + (m_fltC*fltInput) + m_fltD);
	else
		return CalculateLimitOutput(fltInput);
}	

void PolynomialGain::Load(CStdXml &oXml)
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
