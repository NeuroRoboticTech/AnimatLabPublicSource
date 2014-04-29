/**
\file	PacemakerNeuron.h

\brief	Declares the pacemaker neuron class.
**/

#pragma once


namespace FiringRateSim
{
	namespace Neurons
	{ 

		class FAST_NET_PORT PacemakerNeuron  : public Neuron
		{
		protected:
			/// The hyperpolarizing current that brings the membrane potential back down after it has been firing.
			float m_fltIl;

			/// The slope of the linear function that is used to calculate the length of time that Il current remains active.
			float m_fltIh;

			/// A lower steady state threshold. If the steady state voltage of the neuron goes below this value then the Il current is locked on until that voltage rises above this threshold.
			float m_fltVssm;

			/// The slope of the linear function that is used to calculate the length of time that Il current remains active.
			float m_fltMtl;

			/// The intercept of the linear function that is used to calculate the length of time that Il current remains active.
			float m_fltBtl;

			/// This is the length of time that the Ih current remains active.
			float m_fltTh;

			/// Time that the current intrinsic current mode is active.
			float m_fltITime;

			/// The interburst interval
			float m_fltInterburstInterval;

			/// The steady state voltage
			float m_fltVss;

			/// Type of the intrinsic current that is active. (HI or LOW)
			unsigned char m_iIntrinsicType;

			void HighCurrentOn();
			void LowCurrentOn(float fltVss);

			virtual float CalculateIntrinsicCurrent(FiringRateModule *lpModule, float fltInputCurrent);

		public:
			PacemakerNeuron();
			virtual ~PacemakerNeuron();

			float Il();
			void Il(float fltVal);

			float Ih();
			void Ih(float fltVal);

			float Vssm();
			void Vssm(float fltVal);

			float Mtl();
			void Mtl(float fltVal);

			float Btl();
			void Btl(float fltVal);

			float Th();
			void Th(float fltVal);

			float ITime();
			void ITime(float fltVal);

			unsigned char IntrinsicType();
			void IntrinsicType(unsigned char iVal);

			virtual unsigned char NeuronType();

			virtual void ResetSimulation();
			virtual void StepSimulation();

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma region SnapshotMethods
			virtual long CalculateSnapshotByteSize();
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);
#pragma endregion

			virtual void Load(CStdXml &oXml);
		};

	}			//Neurons
}				//FiringRateSim
