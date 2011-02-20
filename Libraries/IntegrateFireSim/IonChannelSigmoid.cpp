// IonChannelSigmoid.cpp: implementation of the IonChannelSigmoid class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "IonChannelSigmoid.h"

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
namespace IntegrateFireSim
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


/*! \brief 
   Destroys the structure object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the structure object..	 
*/

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
