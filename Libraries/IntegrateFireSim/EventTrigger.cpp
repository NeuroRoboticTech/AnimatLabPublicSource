// EventTrigger.cpp: implementation of the EventTrigger class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "EventTrigger.h"

namespace RealisticNeuralNet
{
	namespace ExternalStimuli
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

EventTrigger::EventTrigger()
{
	SetParams();	// defaults adequate
	Init();
}

EventTrigger::~EventTrigger()
{

}

void EventTrigger::SetParams(int type,double delay,double interval,double burstDur,double burstInterval,double burstTrainDur)
{
	m_iType=type;
	m_dDelay=delay;
	m_dInterval=interval;
	m_dBurstDur=burstDur;
	m_dBurstInterval=burstInterval;
	m_dBurstTrainDur=burstTrainDur;

	m_dTimeOfNextEvent=delay;
}

BOOL EventTrigger::GetEventTrigger(double time)
{
	if (time>=m_dTimeOfNextEvent)
	{
		CalcTimeOfNextEvent(time);
		return TRUE;
	}
	return FALSE;
}

void EventTrigger::CalcTimeOfNextEvent(double time)	//sets TONE internally given current time
{
	if (time<m_dDelay)	// not yet hit first (maybe only) event
	{
		m_dTimeOfNextEvent=m_dDelay;
		return;
	}

	if (m_iType==0)
	{
		m_dTimeOfNextEvent=DBL_MAX;
		return;
	}

	if (m_iType==1)	// continuous train of single events
	{
		double ttn=time-m_dDelay;
		ASSERT(ttn>=0);
		ttn=(fmod(ttn,m_dInterval));
		m_dTimeOfNextEvent=time+(m_dInterval-ttn);	// time to start of next repetition
	}

	if (m_iType==2)	// single burst of events
	{
		double ttn=time-m_dDelay;
		ASSERT(ttn>=0);
		ttn=(fmod(ttn,m_dInterval));
		m_dTimeOfNextEvent=time+(m_dInterval-ttn);	// time to start of next repetition
		if (m_dTimeOfNextEvent>(m_dDelay+m_dBurstDur))
			m_dTimeOfNextEvent=DBL_MAX;
		return;
	}

	if (m_iType==3)	// continuous train of bursts of events
	{
		double ttn=time-m_dDelay;
		ASSERT(ttn>=0);

		ttn=fmod(ttn,m_dBurstInterval);	// time in burst train
		if (ttn>(m_dBurstDur-m_dInterval))	// in interburst interval
		{
			m_dTimeOfNextEvent=time+(m_dBurstInterval-ttn);	// time to start of next burst
			return;
		}
		ttn=fmod(ttn,m_dInterval);
		m_dTimeOfNextEvent=time+(m_dInterval-ttn);	// time to start of next repetition

		return;
	}

	if (m_iType==4)	// time-limitted train of bursts of events
	{
		double ttn=time-m_dDelay;
		ASSERT(ttn>=0);

		ttn=fmod(ttn,m_dBurstInterval);	// time in burst train
		if (ttn>(m_dBurstDur-m_dInterval))	// in interburst interval
		{
			m_dTimeOfNextEvent=time+(m_dBurstInterval-ttn);	// time to start of next burst
		if (m_dTimeOfNextEvent>(m_dDelay+m_dBurstTrainDur))
			m_dTimeOfNextEvent=DBL_MAX;
			return;
		}
		ttn=fmod(ttn,m_dInterval);
		m_dTimeOfNextEvent=time+(m_dInterval-ttn);	// time to start of next repetition

		if (m_dTimeOfNextEvent>(m_dDelay+m_dBurstTrainDur))
			m_dTimeOfNextEvent=DBL_MAX;

		return;
	}
}

	}			//ExternalStimuli
}				//RealisticNeuralNet

