/**
\file	TonicNeuron.h

\brief	Declares the tonic neuron class.
**/

#pragma once

namespace FiringRateSim
{
	namespace Neurons
	{
		/**
		\brief	Tonic firing rate neuron.

		\details This neuron has a tonic intrinsic current that is applied to the neuron.
		
		\author	dcofer
		\date	3/29/2011
		**/
		class FAST_NET_PORT TonicNeuron : public Neuron  
		{
		protected:
			/// The tonic current.
			float m_fltIh;

			virtual float CalculateIntrinsicCurrent(FiringRateModule *lpModule, float fltInputCurrent);

		public:
			TonicNeuron();
			virtual ~TonicNeuron();

			float Ih();
			void Ih(float fltVal);

			virtual unsigned char NeuronType();

			virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

			virtual void Load(CStdXml &oXml);
		};

	}			//Neurons
}				//FiringRateSim
