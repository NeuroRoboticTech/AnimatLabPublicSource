/**
\file	Neuron.h

\brief	Declares the neuron class.
**/

#pragma once

namespace FiringRateSim
{

	/**
	\namespace	FiringRateSim::Neurons

	\brief	Contains the neuron classes for the firing rate neuron model. 
	**/
	namespace Neurons
	{
		/**
		\brief	Firing Rate Neuron model.

		\details This Neuron implements a firing rate neural model. The firing rate model is a more abstract representation
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
		class FAST_NET_PORT Neuron : public AnimatSim::Node   
		{
		protected:
			/// Pointer to the parent FiringRateModule.
			FiringRateModule *m_lpFRModule;

			///Membrane capacitance
			float m_fltCn;

			///Inverse membrane capacitance
			float m_fltInvCn;	

			///Membrane conductance
			float m_fltGn;		

			///Minimum Firing frequency
			float m_fltFmin;		

			///Firing frequency gain. This is multiplied by the membrane voltage 
			/// that is above the threshold to determine the firing rate.
			float m_fltGain;	

			///Externally injected current
			float m_fltExternalI;	

			///Intrinsic current.
			float m_fltIntrinsicI; 

			///Current synaptic current.
			float m_fltSynapticI;	

			///current added from all of the adapters.
			float m_fltAdapterI; 
			
			///Used to allow datacharts to track current input from adapters.
			float m_fltAdapterMemoryI;  

			///Total current applied to the neuron
			float m_fltTotalMemoryI; 

			///Tells the maximum noise to use when running sim
			float m_fltVNoiseMax; 

			///Tells if we should use noise or not.
			BOOL m_bUseNoise;    

			/// Tells whether to use the old type gain or new type gain.
			BOOL m_bGainType; 

			/// expon decline working factor for thresh accomm
			float m_fltDCTH;      

			/// The accomodation time constant tells how fast the neuron accomodates to a new membrane potential
			float m_fltAccomTimeConst;

			/// The relative accomodation rate.
			float m_fltRelativeAccom;

			/// true use accomodation
			BOOL m_bUseAccom;

			///Current membrane voltage.
			float m_fltVn;     

			///Current firing frequency.
			float m_fltFiringFreq;  

			///Current and next Membrane voltage. Vn
			float m_aryVn[2];		

			///This is the random noise that should be added to the membrane voltage at a timestep
			float m_fltVNoise;	

			///Firing frequency voltage threshold
			float m_fltVth;			

			///Initial firing frequency voltage threshold
			float m_fltVthi;    

			///Current and next threshold voltage. Vth
			float m_aryVth[2];		

			/// this is the resting potential of the neuron.
			float m_fltVrest;      

			/// this is the membrane voltage that is reported back to animatlab.
			float m_fltVndisp;      

			/// this is the theshold voltage that is reported back to animatlab.
			float m_fltVthdisp;		

			/// The array of synapses that are in-coming to this neuron
			CStdPtrArray<Synapse> m_arySynapses;

			virtual float CalculateFiringFrequency(float fltVn, float fltVth);
			virtual float CalculateSynapticCurrent(FiringRateModule *lpModule);
			virtual float CalculateIntrinsicCurrent(FiringRateModule *lpModule, float fltInputCurrent);

			Synapse *LoadSynapse(CStdXml &oXml);

		public:
			Neuron();
			virtual ~Neuron();

			virtual float Cn();
			virtual void Cn(float fltVal);

			virtual float Gn();
			virtual void Gn(float fltVal);

			virtual float Vth();
			virtual void Vth(float fltVal);

			virtual float Fmin();
			virtual void Fmin(float fltVal);

			virtual float Gain();
			virtual void Gain(float fltVal);

			virtual float ExternalI();
			virtual void ExternalI(float fltVal);

			virtual float IntrinsicCurrent();
			virtual void IntrinsicCurrent(float fltVal);

			virtual float Vrest();
			virtual void Vrest(float fltVal);

			virtual float VNoiseMax();
			virtual void VNoiseMax(float fltVal);

			virtual BOOL UseNoise();
			virtual void UseNoise(BOOL bVal);

			virtual BOOL UseAccom();
			virtual void UseAccom(BOOL bVal);

			virtual float RelativeAccommodation();
			virtual void RelativeAccommodation(float fltVal);

			virtual float AccommodationTimeConstant();
			virtual void AccommodationTimeConstant(float fltVal);

			virtual BOOL GainType();
			virtual void GainType(BOOL bVal);

			virtual float Vn();
			virtual float FiringFreq(FiringRateModule *lpModule);

			virtual unsigned char NeuronType();

			virtual CStdPtrArray<Synapse> *GetSynapses();

			/**
			\brief	Adds a synapse to this neuron. 

			\author	dcofer
			\date	3/29/2011

			\param [in,out]	lpSynapse	Pointer to the synapse to add. 
			**/
			virtual void AddSynapse(Synapse *lpSynapse);
			virtual void AddSynapse(string strXml, BOOL bDoNotInit);
			virtual void RemoveSynapse(int iIndex);
			virtual void RemoveSynapse(string strID, BOOL bThrowError = TRUE);
			virtual Synapse *GetSynapse(int iIndex);
			virtual int TotalSynapses();
			virtual void ClearSynapses();
			virtual int FindSynapseListPos(string strID, BOOL bThrowError = TRUE);

			virtual void AddExternalNodeInput(float fltInput);

			/**
			\brief	Sets the system pointers.
		
			\details There are a number of system pointers that are needed for use in the objects. The
			primariy one being a pointer to the simulation object itself so that you can get global
			parameters like the scale units and so on. However, each object may need other types of pointers
			as well, for example neurons need to have a pointer to their parent structure/organism, and to
			the NeuralModule they reside within. So different types of objects will need different sets of
			system pointers. We call this method to set the pointers just after creation and before Load is
			called. We then call VerifySystemPointers here, during Load and during Initialize in order to
			ensure that the correct pointers have been set for each type of objects. These pointers can then
			be safely used throughout the rest of the system. 
		
			\author	dcofer
			\date	3/2/2011
		
			\param [in,out]	lpSim		The pointer to a simulation. 
			\param [in,out]	lpStructure	The pointer to the parent structure. 
			\param [in,out]	lpModule	The pointer to the parent module module. 
			\param [in,out]	lpNode		The pointer to the parent node. 
			\param	bVerify				true to call VerifySystemPointers. 
			**/
			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify);
			virtual void VerifySystemPointers();
			virtual void Initialize();
			virtual void TimeStepModified();
			virtual void ResetSimulation();
			virtual void StepSimulation();

			virtual void InjectCurrent(float fltVal);

#pragma region SnapshotMethods
			virtual long CalculateSnapshotByteSize();
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);
#pragma endregion

#pragma region DataAccesMethods
			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
			virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE, BOOL bDoNotInit = FALSE);
			virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);
#pragma endregion

			virtual void Load(CStdXml &oXml);
		};

	}			//Neurons
}				//FiringRateSim
