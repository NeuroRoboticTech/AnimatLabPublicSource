// DelayLine.cpp: implementation of the DelayLine class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "DelayLine.h"

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
namespace AnimatSim
{

/*! \brief 
   Constructs an structure object..
   		
	 \return
	 No return value.

   \remarks
	 The constructor for a structure. 
*/

DelayLine::DelayLine()
{
	m_iDelaySize=0;
	m_iDelayComp=0;
	m_iSaveIdx=0;  
	m_iReadIdx=0;
}


/*! \brief 
   Destroys the structure object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the structure object..	 
*/

DelayLine::~DelayLine()
{

try
{
	m_aryRingBuf.RemoveAll();	
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of DelayLine\r\n", "", -1, FALSE, TRUE);}
}


void DelayLine::Initialize(float fltDelayTime, float fltTimeStep)
{
	m_iDelaySize = fltDelayTime/fltTimeStep;
	
	if(m_iDelaySize <= 0)
		m_iDelaySize = 1;

	m_iDelayComp = m_iDelaySize-1;

	m_aryRingBuf.SetSize(m_iDelaySize);
	for(int iIdx=0; iIdx<m_iDelaySize; iIdx++)
		m_aryRingBuf[iIdx] = 0;

	m_iSaveIdx = 0;
	m_iReadIdx = m_iDelayComp;
}

void DelayLine::AddValue(float fltVal)
{
	m_aryRingBuf[m_iSaveIdx] = fltVal;

	if(m_iSaveIdx == m_iDelayComp)
		m_iSaveIdx=0;
	else
		m_iSaveIdx++;
	
	if(m_iReadIdx == m_iDelayComp)
		m_iReadIdx=0;
	else
		m_iReadIdx++;
}

float DelayLine::ReadValue()
{return m_aryRingBuf[m_iReadIdx];}

}			//AnimatSim
