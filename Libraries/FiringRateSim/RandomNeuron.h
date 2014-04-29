/**
\file	RandomNeuron.h

\brief	Declares the random neuron class.
**/

#pragma once

namespace FiringRateSim
{
	namespace Neurons
	{
		/**
		\brief	Random firing rate neuron.

		\details This neuron has intrinsic currents that are randomly generated. It has a high current and low current.
		The low current is a constant. The high current and the burst durations are calculated using random variables. Each one
		has a gain function. A random number between 0 and 100 is generated and fed into the gain function and this determines the
		current and/or the duration value. 
		
		\author	dcofer
		\date	3/29/2011
		**/
		class FAST_NET_PORT RandomNeuron  : public Neuron 
		{
		protected:
			/// Duration of the current mode (HI or LOW)
			float m_fltITime;

			/// Default Type of the intrinsic current that is active (HI or LOW). 
			/// Used to reset the intrinsic type at the beginning of each run.
			unsigned char m_iIntrinsicTypeDefault;

			/// Type of the intrinsic current that is active (HI or LOW)
			unsigned char m_iIntrinsicType;

			/// The active intrinsic current.
			float m_fltIntrinsic;

			/// The low intrinsic current init value
			float m_fltIlinit;

			/// The low intrinsic current
			float m_fltIl;

			/// Pointer to the graph used to calculate the hi current.
			AnimatSim::Gains::Gain *m_lpCurrentGraph;

			/// The pointer to the graph used to calculate the hi current duration.
			AnimatSim::Gains::Gain *m_lpBurstGraph;

			/// The pointer to the graph used to calculate the low current duration.
			AnimatSim::Gains::Gain *m_lpIBurstGraph;

			void HighCurrentOn();
			void LowCurrentOn();

			virtual float CalculateIntrinsicCurrent(FiringRateModule *lpModule, float fltInputCurrent);

		public:
			RandomNeuron();
			virtual ~RandomNeuron();

			float ITime();
			void ITime(float fltVal);

			unsigned char IntrinsicType();
			void IntrinsicType(unsigned char iVal);

			float IntrinsicCurrent();
			void IntrinsicCurrent(float fltVal);

			float Il();
			void Il(float fltVal);

			float Ilinit();
			void Ilinit(float fltVal);

			virtual unsigned char NeuronType();

			AnimatSim::Gains::Gain *CurrentDistribution() {return m_lpCurrentGraph;};
			void CurrentDistribution(AnimatSim::Gains::Gain *lpGain);
			void CurrentDistribution(std::string strXml);

			AnimatSim::Gains::Gain *BurstLengthDistribution() {return m_lpBurstGraph;};
			void BurstLengthDistribution(AnimatSim::Gains::Gain *lpGain);
			void BurstLengthDistribution(std::string strXml);

			AnimatSim::Gains::Gain *InterbusrtLengthDistribution() {return m_lpIBurstGraph;};
			void InterbusrtLengthDistribution(AnimatSim::Gains::Gain *lpGain);
			void InterbusrtLengthDistribution(std::string strXml);

#pragma region SnapshotMethods
			virtual long CalculateSnapshotByteSize();
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);
#pragma endregion

			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
			virtual void ResetSimulation();

			virtual void Load(CStdXml &oXml);
		};

	}			//Neurons
}				//FiringRateSim
