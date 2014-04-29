/**
\file	BistableNeuron.h

\brief	Declares the bistable neuron class.
**/

#pragma once

namespace FiringRateSim
{
	namespace Neurons
	{
		/**
		\brief	Bistable firing rate neuron.

		\details This is a very simple bi-stable neuron model. When the steady-state membrane voltage is pulled above
		a threshold then the hi current is turned on. It will remain on until the threshold is pulled back down below the
		threshold. Then the low current will be turned on.
		
		\author	dcofer
		\date	3/29/2011
		**/
		class FAST_NET_PORT BistableNeuron  : public Neuron 
		{
		protected:
			/// The active intrinsic current
			float m_fltIntrinsic;

			/// The threshold voltage
			float m_fltVsth;

			/// The low current
			float m_fltIl;

			/// The high current
			float m_fltIh;

			virtual float CalculateIntrinsicCurrent(FiringRateModule *lpModule, float fltInputCurrent);

		public:
			BistableNeuron();
			virtual ~BistableNeuron();

			float Vsth();
			void Vsth(float fltVal);

			float IntrinsicCurrent();
			void IntrinsicCurrent(float fltVal);

			float Il();
			void Il(float fltVal);

			float Ih();
			void Ih(float fltVal);

			virtual unsigned char NeuronType();

			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
			virtual void ResetSimulation();

			virtual void Load(CStdXml &oXml);
		};

	}			//Neurons
}				//FiringRateSim
