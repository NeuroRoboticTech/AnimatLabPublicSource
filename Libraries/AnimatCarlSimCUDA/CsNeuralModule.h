/**
\file	CarlSimModuel.h

\brief	Declares the firing rate module class.
**/

#pragma once
#include "snn.h"

/**
\brief	Contains all of the classes that imlement the firing rate neural model.
**/
namespace AnimatCarlSim
{
	/**
	\brief	Firing rate neural module.

	\details This neural module implements a firing rate neural model. The firing rate model is a more abstract representation
	of the neuron than an integrate and fire system. This type of model assumes that there is a linear relationship between 
	cell depolarization and firing rate. After the neuron has depolarized beyond its threshold its firing rate increases linearly between
	0 and 1	relative to the membrane potential and a gain value. Synapses inject current into post-synaptic neurons based on the firing rate.
	The synaptic weight is the amount of current to inject, and this is multiplied by the firing rate of the pre-synaptic neuron. this model also 
	has modulatory and gated synapses.<br>
	Another feature of this model is that there are a few different types of neruons. These primarily differ based on how they implement intrinsic
	currents. Intrinsic currents are currents that are internal to the neuron. An exmample of this is the pacemaker neuron that generates currents
	internally to model bursting behavior.
	
	\author	dcofer
	\date	3/29/2011
	**/
	class ANIMAT_CARL_SIM_PORT CsNeuralModule : public AnimatSim::Behavior::ThreadedModule  
	{
	protected:
		/// The array of neurons in this module.
		CStdPtrArray<CsNeuronGroup> m_aryNeurons;

		/// The array of synapses in this module.
		CStdPtrArray<CsSynapseGroup> m_arySynapses;

		///Pointer to the CARLsim simulator
		CpuSNN *m_lpSNN;

		///The mode of the simulation. GPU vs CPU
		int m_iSimMode;

		CsNeuronGroup *LoadNeuron(CStdXml &oXml);
		CsSynapseGroup *LoadSynapse(CStdXml &oXml);
		void LoadNetworkXml(CStdXml &oXml);

		virtual void StepThread();
		virtual void CloseThread();

		virtual void SetCARLSimulation();

	public:
		CsNeuralModule();
		virtual ~CsNeuralModule();

		/**
		\brief	Gets the module name.
		
		\author	dcofer
		\date	3/29/2011
		
		\return	.
		**/
		virtual std::string ModuleName();

		virtual CpuSNN *SNN() {return m_lpSNN;};

		virtual void SimMode(int iMode);
		virtual int SimMode();

		virtual void Kill(bool bState = true);
		virtual void Initialize();
		virtual void ResetSimulation();
		virtual void StepSimulation();
		virtual void Load(CStdXml &oXml);

#pragma region DataAccesMethods
		virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
		virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
		virtual bool AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError = true, bool bDoNotInit = false);
		virtual bool RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError = true);
#pragma endregion

		virtual void AddNeuron(std::string strXml, bool bDoNotInit = false);
		virtual void RemoveNeuron(std::string strID, bool bThrowError = true);
		virtual int FindNeuronListPos(std::string strID, bool bThrowError = true);

		virtual CStdPtrArray<CsSynapseGroup> *GetSynapses();
		/**
		\brief	Adds a synapse to this module. 

		\author	dcofer
		\date	9/28/2014

		\param [in,out]	lpSynapse	Pointer to the synapse to add. 
		**/
		virtual void AddSynapse(CsSynapseGroup *lpSynapse);
		virtual void AddSynapse(std::string strXml, bool bDoNotInit);
		virtual void RemoveSynapse(int iIndex);
		virtual void RemoveSynapse(std::string strID, bool bThrowError = true);
		virtual CsSynapseGroup *GetSynapse(int iIndex);
		virtual int TotalSynapses();
		virtual void ClearSynapses();
		virtual int FindSynapseListPos(std::string strID, bool bThrowError = true);

		virtual void SimStarting();
		virtual void SimStopping();
	};

}				//FiringRateSim
