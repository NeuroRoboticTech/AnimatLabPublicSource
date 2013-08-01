/**
\file	ElectricalSynapse.h

\brief	Declares the electrical synapse class.
**/

#pragma once

namespace IntegrateFireSim
{
	namespace Synapses
	{

		class ADV_NEURAL_PORT ElectricalSynapse : public SynapseType  
		{
		public:
			ElectricalSynapse();
			virtual ~ElectricalSynapse();
			virtual void Load(CStdXml &oXml);

#pragma region Accessor-Mutators

			void LowCoupling(double dVal);
			double LowCoupling();

			void HighCoupling(double dVal);
			double HighCoupling();

			void TurnOnThreshold(double dVal);
			double TurnOnThreshold();

			void TurnOnSaturate(double dVal);
			double TurnOnSaturate();

#pragma endregion

			virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

		protected:
			/// The low coupling voltage.
			double m_dLowCoup;

			/// The hi coupling voltage.
			double m_dHiCoup;

			/// The turn on voltage level
			double m_dTurnOnV;

			/// The saturation voltage level
			double m_dSaturateV;

		friend class IntegrateFireSim::IntegrateFireNeuralModule;
		};

	}			//Synapses
}				//IntegrateFireSim
