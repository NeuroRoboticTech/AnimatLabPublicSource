// RandomNeuron.h: interface for the RandomNeuron class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_RANDOMNEURON_H__D9D0316C_3191_429A_B3DF_1B52CF9E68BC__INCLUDED_)
#define AFX_RANDOMNEURON_H__D9D0316C_3191_429A_B3DF_1B52CF9E68BC__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace FiringRateSim
{
	namespace Neurons
	{

		class FAST_NET_PORT RandomNeuron  : public Neuron 
		{
		protected:
			float m_fltITime;
			unsigned char m_iIntrinsicType;
			float m_fltIntrinsic;

			float m_fltIl;

			AnimatSim::Gains::Gain *m_lpCurrentGraph;
			AnimatSim::Gains::Gain *m_lpBurstGraph;
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

			virtual unsigned char NeuronType();

			AnimatSim::Gains::Gain *CurrentDistribution() {return m_lpCurrentGraph;};
			void CurrentDistribution(AnimatSim::Gains::Gain *lpGain);
			void CurrentDistribution(string strXml);

			AnimatSim::Gains::Gain *BurstLengthDistribution() {return m_lpBurstGraph;};
			void BurstLengthDistribution(AnimatSim::Gains::Gain *lpGain);
			void BurstLengthDistribution(string strXml);

			AnimatSim::Gains::Gain *InterbusrtLengthDistribution() {return m_lpIBurstGraph;};
			void InterbusrtLengthDistribution(AnimatSim::Gains::Gain *lpGain);
			void InterbusrtLengthDistribution(string strXml);

#pragma region SnapshotMethods
			virtual long CalculateSnapshotByteSize();
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);
#pragma endregion

			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

			virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
		};

	}			//Neurons
}				//FiringRateSim

#endif // !defined(AFX_RANDOMNEURON_H__D9D0316C_3191_429A_B3DF_1B52CF9E68BC__INCLUDED_)
