// ActivatedItem.h: interface for the ActivatedItem class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ACTIVATEDITEM_H__2C8CC259_1E72_4768_9F41_B7B7775BB76C__INCLUDED_)
#define AFX_ACTIVATEDITEM_H__2C8CC259_1E72_4768_9F41_B7B7775BB76C__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatSim
{

	class ANIMAT_PORT ActivatedItem : public AnimatBase  
	{
	protected:
		BOOL m_bEnabled;
		BOOL m_bLoadedTime;
		long m_lStartSlice;
		long m_lEndSlice;
		float m_fltStartTime;
		float m_fltEndTime;
		int m_iStepInterval;  //Tells how many timesteps should elapse before it call StepSimulation again.
		int m_iStepIntervalCount;     //Keeps track of how many time steps have occured since it was zeroed again.
		BOOL m_bAlwaysActive;
		BOOL m_bInitialized;
		BOOL m_bIsActivated;

	public:
		ActivatedItem();
		virtual ~ActivatedItem();

		virtual BOOL Enabled();
		virtual void Enabled(BOOL bVal);

		virtual BOOL LoadedTime();
		virtual void LoadedTime(BOOL bVal);

		virtual long StartSlice();
		virtual void StartSlice(long lVal);

		virtual long EndSlice();
		virtual void EndSlice(long lVal);

		virtual float StartTime();
		virtual void StartTime(float fltVal);

		virtual float EndTime();
		virtual void EndTime(float fltVal);

		virtual int StepInterval();
		virtual void StepInterval(int iVal);

		virtual int StepIntervalCount();
		virtual void StepIntervalCount(int iVal);

		virtual BOOL AlwaysActive();
		virtual void AlwaysActive(BOOL bVal);

		BOOL IsActivated() {return m_bIsActivated;}
		BOOL IsInitialized() {return m_bInitialized;}

		BOOL NeedToActivate(Simulator *lpSim);
		BOOL NeedToDeactivate(Simulator *lpSim);

		virtual BOOL operator<(ActivatedItem *lpItem) = 0;

		virtual void Initialize(Simulator *lpSim);
		virtual void ReInitialize(Simulator *lpSim);
		virtual BOOL NeedToStep();
		virtual void ResetSimulation(Simulator *lpSim);
		virtual void StepSimulation(Simulator *lpSim) = 0;
		virtual void Activate(Simulator *lpSim);
		virtual void Deactivate(Simulator *lpSim);

		virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

		virtual BOOL Overlaps(ActivatedItem *lpItem);

		virtual void Load(Simulator *lpSim, CStdXml &oXml);
		virtual void Trace(ostream &oOs);
	};

	BOOL LessThanActivatedItemCompare(ActivatedItem *lpItem1, ActivatedItem *lpItem2);

}			//AnimatSim

#endif // !defined(AFX_ACTIVATEDITEM_H__2C8CC259_1E72_4768_9F41_B7B7775BB76C__INCLUDED_)
