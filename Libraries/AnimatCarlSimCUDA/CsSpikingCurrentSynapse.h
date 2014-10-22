/**
\file	CsSpikingCurrentSynapse.h

\brief	Declares the synapse class.
**/

#pragma once

namespace AnimatCarlSim
{

	/**
	\brief	Firing rate synapse model.

	\details This synapse type has a weight that is a current value. It injects a portion of that weight
	into the post-synaptic neuron based on the pre-synaptic neurons firing rate. I = W*F. (Where W is the
	weight, F is the firing rate, and I is the current.)
		
	\author	dcofer
	\date	3/29/2011
	**/
	class ANIMAT_CARL_SIM_PORT CsSpikingCurrentSynapse : public AnimatSim::Link   
	{
	protected:
		/// GUID ID of the pre-synaptic neruon.
		std::string m_strFromID;

		/// GUID ID of the pre-synaptic neruon.
		std::string m_strToID;

		/// The pointer to pre-synaptic neuron
		CsNeuronGroup *m_lpFromNeuron;

		/// The pointer to post-synaptic neuron
		AnimatSim::Node *m_lpToNeuron;
		
		///The decay rate for which a current pulse
		float m_fltPulseDecay;

		///The magnitude amount of the current to apply for a single pulse
		float m_fltPulseMagnitude;

		///The sign of the current to apply for a single pulse
		float m_fltPulseSign;

		///The exponential decay constant calculated from the time step and decay rate.
		float m_fltPulseTC;

		///Tells whether this entire population is monitored or just individual cells
		bool m_bWholePopulation;

		///An array of neuron indices for individual neurons we want to stimulate
		CStdMap<int, int> m_aryCells;
		
		///Keeps track of the number of steps till we need to test for spikes to process
		int m_iStepsPerTest;

		///Keeps track of how my steps we have take tiil we are ready to test again.
		int m_iStepsPerTestCount;

		//Keeps track of the current Test time to check for spikes
		unsigned long m_ulSpikeTestTime;

		///The magnitude of the current applied to the target neuron.
		float m_fltCurrentMagnitude;

		///The actual applied current with sign
		float m_fltAppliedCurrent;

		///The amount by which the current is decremented each step
		float m_fltDecrementCurrent;

		long m_lTotalSpikesAdded;

		CStdArray<long> m_arySpikeTimes;

		virtual void LoadCells(CStdXml &oXml);

		virtual void ProcessSpikes();
		virtual void CalculateStepsPerTest();

		virtual void MonitorSpikeEventFired(int iGroupID, int iNeuronID, long lTimeIdx);

#ifndef STD_DO_NOT_ADD_BOOST
			//Signal for when a spike occurs
			boost::signals2::connection m_MonitoredSpikeEvent;

			///mutex used to try and access matches variable.
			boost::interprocess::interprocess_mutex m_AccessSpikes;
#endif

	public:
		CsSpikingCurrentSynapse();
		virtual ~CsSpikingCurrentSynapse();
								
		static CsSpikingCurrentSynapse *CastToDerived(AnimatBase *lpBase) {return static_cast<CsSpikingCurrentSynapse*>(lpBase);}
		
		virtual void PulseDecay(float fltVal);
		virtual float PulseDecay();

		virtual void PulseCurrent(float fltVal);
		virtual float PulseCurrent();
		
		virtual void Coverage(std::string strType);

		virtual bool WholePopulation();
		virtual void WholePopulation(bool bVal);

		virtual void Cells(std::string strXml);

		/**
		\brief	Gets the pre-synaptic neuron.
			
		\author	dcofer
		\date	3/29/2011
			
		\return	Pointer to the neuron.
		**/
		CsNeuronGroup *FromNeuron() {return m_lpFromNeuron;};

#pragma region DataAccesMethods
		virtual float *GetDataPointer(const std::string &strDataType);
		virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
		virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
#pragma endregion

		virtual void TimeStepModified();
		virtual void VerifySystemPointers();
		virtual void Initialize();
		virtual void ResetSimulation();
		virtual void StepSimulation();
		virtual void Load(CStdXml &oXml);
	};

}				//AnimatCarlSim
