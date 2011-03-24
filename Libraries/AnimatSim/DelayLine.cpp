/**
\file	DelayLine.cpp

\brief	Implements the delay line class.
**/

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "DelayLine.h"

namespace AnimatSim
{

/**
\brief	Default constructor.

\author	dcofer
\date	3/24/2011
**/
DelayLine::DelayLine()
{
	m_iDelaySize=0;
	m_iDelayComp=0;
	m_iSaveIdx=0;  
	m_iReadIdx=0;
}

/**
\brief	Destructor.

\author	dcofer
\date	3/24/2011
**/
DelayLine::~DelayLine()
{

try
{
	m_aryRingBuf.RemoveAll();	
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of DelayLine\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Initializes the Delay line.

\author	dcofer
\date	3/24/2011

\param	fltDelayTime	Time of the delay. 
\param	fltTimeStep 	The time step duration. 
**/
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

/**
\brief	Adds a value to the begining of the line. 

\details This increments the delay line appropriately.

\author	dcofer
\date	3/24/2011

\param	fltVal	The new value. 
**/
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

/**
\brief	Reads the current value at the end of the delay line.

\author	dcofer
\date	3/24/2011

\return	The value at the end of the line.
**/
float DelayLine::ReadValue()
{return m_aryRingBuf[m_iReadIdx];}

}			//AnimatSim
