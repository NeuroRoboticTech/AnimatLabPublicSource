/**
\file	ModulatePropSynapse.h

\brief	Declares the neuron property modulatory synapse class.
**/

#pragma once

namespace FiringRateSim
{
	namespace Synapses
	{
		/**
		\brief	Firing rate synapse model for modulating neuron properties.

		\details This synapse can modulate any of the neural properties based on the firing rate of the pre-synaptic neuron
		
		\author	dcofer
		\date	6/17/2014
		**/
		class FAST_NET_PORT ModulateNeuronPropSynapse : public Synapse   
		{
		protected:
			/// Pointer to the graph used to calculate the hi current.
			AnimatSim::Gains::Gain *m_lpGain;

			/// The name of the property we are modulating.
			std::string m_strPropertyName;

			///Pointer to the property we are modulating.
			float *m_lpPropertyData;

		public:
			ModulateNeuronPropSynapse();
			virtual ~ModulateNeuronPropSynapse();
			
			AnimatSim::Gains::Gain *ModulationGain() {return m_lpGain;};
			void ModulationGain(AnimatSim::Gains::Gain *lpGain);
			void ModulationGain(std::string strXml);
						
			virtual void PropertyName(std::string strPropName);
			virtual std::string PropertyName();

#pragma region DataAccesMethods
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
#pragma endregion

			virtual void Process(float &fltCurrent);

			virtual void ResetSimulation();
			virtual void Initialize();
			virtual void Load(CStdXml &oXml);
		};

	}			//Synapses
}				//FiringRateSim
