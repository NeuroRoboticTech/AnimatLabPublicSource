// EventTrigger.h: interface for the EventTrigger class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_EVENTTRIGGER_H__6A5F4DA1_A192_406C_97B1_C7278E63B05F__INCLUDED_)
#define AFX_EVENTTRIGGER_H__6A5F4DA1_A192_406C_97B1_C7278E63B05F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace RealisticNeuralNet
{
	namespace ExternalStimuli
	{

		class EventTrigger
		{
		public:
			EventTrigger();
			virtual ~EventTrigger();
			
			double GetTimeOfNextEvent() {return m_dTimeOfNextEvent;}
			BOOL GetEventTrigger(double time);	// returns TRUE if just entered event
			void SetParams(int type=0,double delay=0.,double interval=1.,double burstDur=1.,double burstInterval=1.,double burstTrainDur=1.);
			void Init() {m_dTimeOfNextEvent=m_dDelay;}

			void SetType(int type) {m_iType=type;}
			int GetType() {return m_iType;}
			void SetDelay(double delay) {m_dDelay=delay;}
			double GetDelay() {return m_dDelay;}
			void SetInterval(double interval) {m_dInterval=interval;}
			double GetInterval() {return m_dInterval;}
			void SetBurstDur(double burstDur) {m_dBurstDur=burstDur;}
			double GetBurstDur() {return m_dBurstDur;}
			void SetBurstInterval(double burstInt) {m_dBurstInterval=burstInt;}
			double GetBurstInterval() {return m_dBurstInterval;}
			void SetBurstTrainDur(double burstTrainDur) {m_dBurstTrainDur=burstTrainDur;}
			double GetBurstTrainDur() {return m_dBurstTrainDur;}

		protected:
			void CalcTimeOfNextEvent(double time);	//sets TONE internally given current time
			double m_dTimeOfNextEvent;

			int		m_iType;
			double	m_dDelay;
			double	m_dInterval;
			double	m_dBurstDur;
			double	m_dBurstInterval;
			double	m_dBurstTrainDur;

		};

	}			//ExternalStimuli
}				//RealisticNeuralNet

#endif // !defined(AFX_EVENTTRIGGER_H__6A5F4DA1_A192_406C_97B1_C7278E63B05F__INCLUDED_)
