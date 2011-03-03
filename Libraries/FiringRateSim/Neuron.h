#pragma once

namespace FiringRateSim
{

	/**
	\namespace	FiringRateSim::Neurons

	\brief	Contains the neuron classes for the firing rate neuron model. 
	**/
	namespace Neurons
	{

		class FAST_NET_PORT Neuron : public AnimatSim::Node   
		{
		protected:
			FiringRateModule *m_lpFastModule;
			Organism *m_lpOrganism;

			float m_fltCn;			//Membrane capacitance
			float m_fltInvCn;		//Inverse membrane capacitance
			float m_fltGn;			//Membrane conductance
			float m_fltFmin;		//Minimum Firing frequency
			float m_fltGain;		//Firing frequency gain
			float m_fltExternalI;	//Externally injected current
			float m_fltIntrinsicI; //Intrinsic current.
			float m_fltSynapticI;	//Current synaptic current.
			float m_fltAdapterI; //current added from all of the adapters.
			float m_fltAdapterMemoryI;  //Used to allow datacharts to track current input from adapters.
			float m_fltTotalMemoryI; //Total current applied to the neuron
			float m_fltVNoiseMax; //Tells the maximum noise to use when running sim
			BOOL m_bUseNoise;    //Tells if we should use noise or not.

			BOOL m_bGainType; 

			float m_fltDCTH;      // expon decline working factor for thresh accomm
			float m_fltAccomTimeConst;
			float m_fltRelativeAccom;
			BOOL m_bUseAccom;

			float m_fltVn;      //Current membrane voltage.
			float m_fltFiringFreq;  //Current firing frequency.
			float m_aryVn[2];		//Current and next Membrane voltage. Vn
			float m_fltVNoise;	//This is the random noise that should be added to the membrane voltage at a timestep

			float m_fltVth;			//Firing frequency voltage threshold
			float m_fltVthi;    //Initial firing frequency voltage threshold
			float m_aryVth[2];		//Current and next threshold voltage. Vth


			float m_fltVrest;      // this is the resting potential of the neuron.
			float m_fltVndisp;      // this is the membrane voltage that is reported back to animatlab.
			float m_fltVthdisp;		// this is the theshold voltage that is reported back to animatlab.

			CStdPtrArray<Synapse> m_arySynapses;

			virtual float CalculateFiringFrequency(float fltVn, float fltVth);
			virtual float CalculateSynapticCurrent(FiringRateModule *lpModule);
			virtual float CalculateIntrinsicCurrent(FiringRateModule *lpModule, float fltInputCurrent);

			Synapse *LoadSynapse(CStdXml &oXml);

		public:
			Neuron();
			virtual ~Neuron();

			float Cn();
			void Cn(float fltVal);

			float Gn();
			void Gn(float fltVal);

			float Vth();
			void Vth(float fltVal);

			float Fmin();
			void Fmin(float fltVal);

			float Gain();
			void Gain(float fltVal);

			float ExternalI();
			void ExternalI(float fltVal);

			float IntrinsicCurrent();
			void IntrinsicCurrent(float fltVal);

			float Vrest();
			void Vrest(float fltVal);

			float VNoiseMax();
			void VNoiseMax(float fltVal);

			float RelativeAccomodation();
			void RelativeAccomodation(float fltVal);

			float AccomodationTimeConstant();
			void AccomodationTimeConstant(float fltVal);

			BOOL GainType();
			void GainType(BOOL bVal);

			float Vn();
			float FiringFreq(FiringRateModule *lpModule);

			virtual unsigned char NeuronType();

			virtual CStdPtrArray<Synapse> *GetSynapses();
			virtual void AddSynapse(Synapse *lpSynapse);
			virtual void AddSynapse(string strXml);
			virtual void RemoveSynapse(int iIndex);
			virtual void RemoveSynapse(string strID, BOOL bThrowError = TRUE);
			virtual Synapse *GetSynapse(int iIndex);
			virtual int TotalSynapses();
			virtual void ClearSynapses();
			virtual int FindSynapseListPos(string strID, BOOL bThrowError = TRUE);

			virtual void AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput);
			virtual void Initialize(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule);
			virtual void ResetSimulation(Simulator *lpSim, Structure *lpStruct);
			virtual void StepSimulation(Simulator *lpSim, Organism *lpOrganism, FiringRateModule *lpModule);

			virtual void InjectCurrent(float fltVal);

#pragma region SnapshotMethods
			virtual long CalculateSnapshotByteSize();
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);
#pragma endregion

#pragma region DataAccesMethods
			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
			virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE);
			virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);
#pragma endregion

			//This is not used. The one above is used because we have to pass in the neuron indexes
			virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure) {};
			virtual void Load(CStdXml &oXml);
		};

	}			//Neurons
}				//FiringRateSim
