/**
\file	FiringRateModule.h

\brief	Declares the firing rate module class.
**/

#pragma once

/**
\brief	Contains all of the classes that imlement the firing rate neural model.
**/
namespace FiringRateSim
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
	class FAST_NET_PORT FiringRateModule : public AnimatSim::Behavior::NeuralModule  
	{
	protected:
		/// Size of the neural network.
		CStdIPoint m_oNetworkSize;

		/// The array of neurons in this module.
		CStdPtrArray<Neuron> m_aryNeurons;

		/// Within the neuron it keeps a two bit array to keep track of the previous and current membrane potential calculations.
		/// This tells which of these array elements is currently the active one.
		bool m_bActiveArray;

		Neuron *LoadNeuron(CStdXml &oXml);
		void LoadNetworkXml(CStdXml &oXml);

	public:
		FiringRateModule();
		virtual ~FiringRateModule();

		/**
		\brief	Gets the module name.
		
		\author	dcofer
		\date	3/29/2011
		
		\return	.
		**/
		virtual std::string ModuleName() {return Nl_NeuralModuleName();};

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

		bool ActiveArray();
		void ActiveArray(bool bVal);
		bool InactiveArray();
		void InactiveArray(bool bVal);

#pragma region SnapshotMethods
			virtual long CalculateSnapshotByteSize();
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);
#pragma endregion

	};

}				//FiringRateSim
