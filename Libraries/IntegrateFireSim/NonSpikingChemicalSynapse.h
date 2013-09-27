/**
\file NonSpikingChemicalSynapse.h

\brief	Declares the non spiking chemical synapse class.
**/

#pragma once


namespace IntegrateFireSim
{
	namespace Synapses
	{
		/**
		\brief	Non-spiking chemical synapse type.

		\details This synapse type changes its conductance in a graded manner based on the pre-synaptic neurons
		membrane potential. This emulates a steady release of neurotransmitter onto the post-synaptic cell.
		
		\author	dcofer
		\date	3/31/2011
		**/
		class ADV_NEURAL_PORT NonSpikingChemicalSynapse : public SynapseType  
		{
		public:
			NonSpikingChemicalSynapse();
			virtual ~NonSpikingChemicalSynapse();
			virtual void Load(CStdXml &oXml);

#pragma region Accessor-Mutators

			void EquilibriumPotential(double dVal);
			double EquilibriumPotential();

			void MaxSynapticConductance(double dVal);
			double MaxSynapticConductance();

			void PreSynapticThreshold(double dVal);
			double PreSynapticThreshold();

			void PreSynapticSaturationLevel(double dVal);
			double PreSynapticSaturationLevel();

#pragma endregion

			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);

		protected:
			/// The equilibruim potential
			double m_dEquil;

			/// base syn amp, before vd or hebb
			double m_dSynAmp;

			/// The threshold voltage
			double m_dThreshV;

			/// The saturation voltage
			double m_dSaturateV;

		friend class IntegrateFireSim::IntegrateFireNeuralModule;
		};

	}			//Synapses
}				//IntegrateFireSim
