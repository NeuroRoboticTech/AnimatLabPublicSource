/**
\file	CurrentStimulus.h

\brief	Declares the current stimulus class. 
**/

#pragma once


namespace AnimatSim
{
	namespace ExternalStimuli
	{
		/**
		\brief	Current stimulus for neural items. 

		\details This stimulus applies a current stimulus to a neuron type of Node object.
		It can be configured to apply either a constant, repetitive, or bursting stimulus type.
		A bursting stimulus is one that has a repetitive stimulus that occurs in bursts. This works
		by setting the cycle on/cycle off duration to set the time periods for repetitive stimuli, and 
		burst on/off for the burst periods. If you want to do a constant current then cycle on/off and burst on/off
		should be the start/end times of the stimulus. If you want a repetitive current then set burst on/off to the
		start/end time of the stimulus, but set the cycle on/off to some other values in between. If you want bursting
		then set cycle on/off to values less than the burst on/off values.
		
		\author	dcofer
		\date	3/16/2011
		**/
		class ANIMAT_PORT CurrentStimulus  : public ExternalStimulus
		{
		protected:
			/// GUID ID of the neuron we are stimulating.
			string m_strTargetNodeID;

			/// Pointer to the float data value within the neruon where we will be adding the new current.
			/// This is obtained using the GetDataPointer method.
			float *m_lpExternalCurrent;

			/// The post-fix string current equation
			string m_strCurrentEquation;

			/// Pointer to the post-fix equation evaluator.
			CStdPostFixEval *m_lpCurrentOnEval;

			/// Tells the type of current to apply. (Constant, Repetitive, Bursting)
			int m_iType;  

			/// The active current at that time step.
			float m_fltActiveCurrent;

			/// The current to apply when the cycle is on.
			float m_fltCurrentOn;

			/// The current to apply when the cycle is off.
			float m_fltCurrentOff;

			/// The current to apply when the burst is off.
			float m_fltCurrentBurstOff;

			/// The initial active current that is used when reseting the simulation
			float m_fltInitialActiveCurrent;

			//There are the durations in time.
			/// Duration of the cycle on period in time.
			float m_fltCycleOnDuration;

			/// Duration of the cycle off period in time.
			float m_fltCycleOffDuration;

			/// Duration of the burst on period in time.
			float m_fltBurstOnDuration;

			/// Duration of the burst off period in time.
			float m_fltBurstOffDuration;

			//The durations are converted to time slice values for easier comparisons.
			/// Duration of the cycle on period in time slices.
			long m_lCycleOnDuration;

			/// Duration of the cycle off period in time slices.
			long m_lCycleOffDuration;

			/// Duration of the burst on period in time slices.
			long m_lBurstOnDuration;

			/// Duration of the burst off period in time slices.
			long m_lBurstOffDuration;

			/// The time slice where the cycle starts.
			long m_lCycleStart;

			/// The time slice where the burst starts.
			long m_lBurstStart;

			/// Tells whether a cycle is on or not.
			BOOL m_bCycleOn;

			/// Tells whether a burst is on or not.
			BOOL m_bBurstOn;

			virtual float GetCurrentOn();
			virtual void SetSliceData();

		public:
			CurrentStimulus();
			virtual ~CurrentStimulus();
			
			//virtual int Type();
			virtual string Type();
			virtual void Type(string strValue);
			
			virtual void AlwaysActive(BOOL bVal);

			virtual string TargetNodeID();
			virtual void TargetNodeID(string strID);

			virtual float CurrentOn();
			virtual void CurrentOn(float fltVal);

			virtual float CurrentOff();
			virtual void CurrentOff(float fltVal);

			virtual float CurrentBurstOff();
			virtual void CurrentBurstOff(float fltVal);

			virtual float CycleOnDuration();
			virtual void CycleOnDuration(float fltVal);

			virtual float CycleOffDuration();
			virtual void CycleOffDuration(float fltVal);

			virtual float BurstOnDuration();
			virtual void BurstOnDuration(float fltVal);

			virtual float BurstOffDuration();
			virtual void BurstOffDuration(float fltVal);

			virtual string CurrentEquation();
			virtual void CurrentEquation(string strEquation);

			virtual void Load(CStdXml &oXml);

			//ActiveItem overrides
			virtual void Initialize();  
			virtual void ResetSimulation();  
			virtual void Activate();
			virtual void StepSimulation();
			virtual void Deactivate();

			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
		};

	}			//ExternalStimuli
}				//VortexAnimatSim
