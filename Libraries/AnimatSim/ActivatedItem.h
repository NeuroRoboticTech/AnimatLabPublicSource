/**
\file	ActivatedItem.h

\brief	Declares the activated item class. 
**/

#pragma once

namespace AnimatSim
{
	/**
	\class	ActivatedItem
	
	\brief	Base class for all activated items. 

	\details Activated items are types of objects that are activated and deactivated 
	at specific points in time. Examples of these types of objects are DataChart's and 
	ExternalStimulus. They have a start time and end time defined for them. When the simulation 
	reaches the start time then it activates the item and when the sim reaches the end time
	it is deactivated. It is also possible to set it so that it is always active during the 
	entire simulation. This base class and the ActivatedItemMgr take care of most of the 
	details to allow this functionality to work.
	
	\author	dcofer
	\date	3/1/2011
	**/
	class ANIMAT_PORT ActivatedItem : public AnimatBase  
	{
	protected:
		/// Tells if this item is enabled or not. If it is not enabled then it is not run.
		BOOL m_bEnabled;

		/// Keeps track of whether we loaded in time values or time slices. This is used
		/// during the initialization procedures.
		BOOL m_bLoadedTime;

		/// The time slice where this item becomes active.
		long m_lStartSlice;

		/// The time slice where this item is deactived.
		long m_lEndSlice;

		/// The actual simulation time where this item becomes activated. This is used
		/// to calculate the StartSlice.
		float m_fltStartTime;

		/// The actual simulation time where this item becomes deactivated. This is used
		/// to calculate the EndSlice.
		float m_fltEndTime;

		/// Tells how many timesteps should elapse before it call StepSimulation again.
		/// This is used in cases where we do not want the item to be run every simulation
		/// time step, but instead of X steps. StepIntervalCount keeps track of how many steps 
		/// until the interval is met. 
		int m_iStepInterval;  

		/// Keeps track of how many time steps have occured since it was zeroed again.
		int m_iStepIntervalCount;     

		/// Determines whether this item is always active during simulation
		BOOL m_bAlwaysActive;

		/// true if item has been initialized
		BOOL m_bInitialized;

		/// true if item has been activated
		BOOL m_bIsActivated;

		virtual void SetSliceData();

	public:
		ActivatedItem();
		virtual ~ActivatedItem();

		virtual BOOL Enabled();
		virtual void Enabled(BOOL bVal);

		virtual BOOL LoadedTime();
		virtual void LoadedTime(BOOL bVal);

		virtual long StartSlice();
		virtual void StartSlice(long lVal, BOOL bReInit = TRUE);

		virtual long EndSlice();
		virtual void EndSlice(long lVal, BOOL bReInit = TRUE);

		virtual float StartTime();
		virtual void StartTime(float fltVal, BOOL bReInit = TRUE);

		virtual float EndTime();
		virtual void EndTime(float fltVal, BOOL bReInit = TRUE);

		virtual int StepInterval();
		virtual void StepInterval(int iVal);

		virtual int StepIntervalCount();
		virtual void StepIntervalCount(int iVal);

		virtual BOOL AlwaysActive();
		virtual void AlwaysActive(BOOL bVal);

		BOOL IsActivated();
		BOOL IsInitialized();

		BOOL NeedToActivate();
		BOOL NeedToDeactivate();

		/**
		\fn	virtual BOOL ActivatedItem::operator<(ActivatedItem *lpItem) = 0;
		
		\brief	Less-than comparison operator. 

		\details This is a pure virtual definition of this method. It must be defined
		in the derived class.
		
		\author	dcofer
		\date	3/1/2011
		
		\param [in,out]	lpItem	The pointer to an item to compare with. 
		
		\return	true if the first parameter is less than the second. 
		**/
		virtual BOOL operator<(ActivatedItem *lpItem) = 0;

		virtual void Initialize();
		virtual void ReInitialize();
		virtual BOOL NeedToStep();
		virtual void ResetSimulation();
		virtual void Activate();
		virtual void Deactivate();
		virtual void TimeStepModified();

		virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
		virtual BOOL Overlaps(ActivatedItem *lpItem);
		virtual void Load(CStdXml &oXml);
	};

	BOOL LessThanActivatedItemCompare(ActivatedItem *lpItem1, ActivatedItem *lpItem2);

}			//AnimatSim
