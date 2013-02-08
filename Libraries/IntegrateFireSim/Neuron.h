/**
\file	IntegrateFireSim\Neuron.h

\brief	Declares the neuron class.
**/

#pragma once

class IntegrateFireNeuralModule;

/**
\namespace	IntegrateFireSim

\brief	Contains all of the classes to implement a basic integrate and fire neural model. 
**/
namespace IntegrateFireSim
{
	/**
	\brief	Integrate and fire neuron model. 

	\details This class implements an integrate and fire neuron model.
	
	\author	dcofer
	\date	3/30/2011
	**/
	class ADV_NEURAL_PORT Neuron : public AnimatSim::Node
	{
	protected:
		/// The pointer to the parent IntegrateFireNeuralModule
		IntegrateFireNeuralModule *m_lpIGFModule;


	/////////////////////////////////////
	// LOADABLE PARAMETERS

		/// The static spike peak.
		static double m_dSpikePeak;

		/// The static spike strength used when calculating the action potential
		static double m_dSpikeStrength;

		/// The static after-hyperpolarizing equil pot. Typically equil pot for K
		static double m_dAHPEquilPot;

		/// The static calcium equil pot
		static double m_dCaEquilPot;

		/// The static absolute refractory period after an action potential
		static double m_dAbsoluteRefr;

		/// The static absolute refractory period in timeslices after an action potential
		static long m_lAbsoluteRefr;

		/// Integer ID for the neuron. This is its index in the array of neruons in the neural module.
		int m_iNeuronID;

		/// Sets whether the neuron is disabled or not.
		BOOL m_bZapped;

		// individual, basic properties
			/// The resting potential of the neuron
			double m_dRestingPot;

			/// Size of the neuron. This is essentially equivalent to the membrane conductance. 
			double m_dSize;

			/// The time constant for the neuron.
			double m_dTimeConst;

			/// The membrane capacitance.
			double m_dCm;

			/// The initial voltage threshold for the neuron.
			double m_dInitialThresh;

			/// The amount of relative accommodation. This ranges from 0 to 1.
			double m_dRelativeAccom;

			/// The accommodation time constant.
			double m_dAccomTimeConst;

			/// The after-hyperpolarizing conductance amplitude.
			double m_dAHPAmp;

			/// The after-hyperpolarizing time constant
			double m_dAHPTimeConst;

	// burster properties
			/// The maximum conductance of the calcium current.
			double m_dGMaxCa;

			/// Activation mid point of the calcium current.
			double m_dVM;		

			/// Activation slope of the calcium current.
			double m_dSM;	

			/// activation time constant the calcium current.
			double m_dMTimeConst;	

			/// Inactivation mid point of the calcium current.
			double m_dVH;

			/// Inactivation slope of the calcium current.
			double m_dSH;

			/// Inactivation time constant the calcium current.
			double m_dHTimeConst;

			/// An array of tonic inputs that can be applied to the neuron.
			CStdArray<double> m_aryTonicInputPeriod;

			/// An array of tonic input types that can be applied to the neuron.
			CStdArray<int> m_aryTonicInputPeriodType;

			/// A tonic current stimulus that can be applied to the neuron.
			double m_dToniCurrentStimulusulus;

			/// The noise being applied to the membrane.
			double m_dNoise;

			/// The array of ion channels.
			CStdPtrArray<IonChannel> m_aryIonChannels;

			/// The pointer to the calcium activation object.
			CaActivation *m_lpCaActive;

			/// The pointer to the calcium inactivation object.
			CaActivation *m_lpCaInactive;

	//////////////////////////////
	// WORKING STUFF
		/// The statoc time step.
		static double m_dDT;

		/// The membrane potential.
		double m_dMemPot;

		/// The next membrane potential.
		double m_dNewMemPot;

		/// The voltage threshold
		double m_dThresh;

		/// true if spike occured.
		BOOL m_bSpike;		// spike flag

		// electrical synapse current
		/// The electrical synaptic current.
		double m_dElecSynCur;

		/// The electrical synaptic condutance.
		double m_dElecSynCond;

		// non-spiking chemical synapse current
		/// The non-spiking synaptic current
		double m_dNonSpikingSynCur;

		/// The non-spiking synaptic condutance.
		double m_dNonSpikingSynCond;

		// calculation stuff
		/// The refractory count down
		long m_lRefrCountDown;

		/// exponential decline working factor for threshold accommodation
		double m_dDCTH;	

		/// exponential decline factor for AHP
		double m_dDGK;	

		/// cummulative amplitude of AHP conductance
		double m_dGK;	

		/// cummulative total contuctance
		double m_dGTot;	
		
		/// Reported g total
		float m_fltGTotal; 

		/// The stimulus current.
		double m_dStim;

		/// The adapter current.
		float m_fltAdapterI;

		/// The adapter current memory. This is used to allow datacharts to track current input from adapters.
		float m_fltAdapterMemoryI;  

		/// The external current.
		float m_fltExternalI;

		/// The ioin channel current.
		float m_fltChannelI;

		/// The ion channel current memory. This is used to allow datacharts to track current input from ion channels.
		float m_fltChannelMemoryI;

		/// The total current.
		float m_fltTotalI;

		/// The total current memory. This is used to allow datacharts to track current input from ion channels.
		float m_fltTotalMemoryI;

		/// The ca current memory
		float m_fltICaMemory;
		
		/// The memory potential. This is for reporting purposes.
		float m_fltMemPot;

		/// The threshold potential memory. Used to allow us to chart the threshold if needed.
		float m_fltThresholdMemory;  

		/// The electrical synaptic current memory. Used for reporting purposes.
		float m_fltElecSynCurMemory;

		/// The spiking synaptic current memory. Used for reporting purposes.
		float m_fltSpikingSynCurMemory;

		/// The non-spiking synaptic current memory. Used for reporting purposes.
		float m_fltNonSpikingSynCurMemory;

		/// The spike memory. Used for reporting purposes.
		float m_fltSpike;

		/// Number of ion channels.
		int m_iIonChannels;

		// for bursting	
		/// The activation variable.
		double m_dM;

		/// The inactivation variable.
		double m_dH;

		/// The Reporting variable for E
		float m_fltEMemory;

		//Vars to calculate the firing frequency of this neuron.
		/// Time of the last spike
		double m_fltLastSpikeTime;

		/// The firing frequency
		float m_fltFiringFreq;

		//Used to return the membrane conductance of this neuron in GetDataPointers.
		/// The membrane conductance. Used for reporting purposes.
		float m_fltGm;

		/// The rest potential. Used for reporting purposes. 
		float m_fltVrest;

		/// Current conductance of each synaptic type
		CStdArray<double> m_arySynG;	
		
		/// facilitated initial g increase caused by spontaneous input
		CStdArray<double> m_aryFacilSponSynG;	

		/// Time to next spontaneous occurrence of this syn type
		CStdArray<double> m_aryNextSponSynTime;	

		/// exponential decline factor in syn G.
		CStdArray<double> m_aryDG;	

		/// exponential decline factor in facilitation.
		CStdArray<double> m_aryFacilD;		

		virtual IonChannel *LoadIonChannel(CStdXml &oXml);
		IonChannel *FindIonChannel(string strID, BOOL bThrowError);

	protected:
		//void ClearSpikeTimes();
		//void StoreSpikeForFreqAnalysis(IntegrateFireNeuralModule *lpNS);
		void CalculateFiringFreq(IntegrateFireNeuralModule *lpNS);
		virtual void AddIonChannel(string strXml, BOOL bDoNotInit);
		virtual void RemoveIonChannel(string strID, BOOL bThrowError = TRUE);
	
	public:
		Neuron();
		virtual ~Neuron();
		virtual void Load(CStdXml &oXml);

#pragma region Accessor-Mutators

		int NeuronID();
		void NeuronID(int iID);

		virtual BOOL Enabled();
		virtual void Enabled(BOOL bValue);

	//////
	// ENGINE
		double GetRestingPot();
		double GetMemPot();
		double GetThresh();
		BOOL GetSpike();
		BOOL GetZapped();
		void IncrementStim(double stim);
		void InElectricalSynapseCurr(double cur);
		void InElectricalSynapseCond(double cond);
		void IncNonSpikingSynCurr(double cur);
		void IncNonSpikingSynCond(double cond);

		CStdPtrArray<IonChannel> *IonChannels();

		void RestingPotential(double dVal) ;
		double RestingPotential();

		void Size(double dVal);
		double Size();

		void TimeConstant(double dVal);
		double TimeConstant();

		void InitialThreshold(double dVal) ;
		double InitialThreshold();

		void RelativeAccomodation(double dVal);
		double RelativeAccomodation();

		void AccomodationTimeConstant(double dVal);
		double AccomodationTimeConstant();

		void AHPAmplitude(double dVal);
		double AHPAmplitude();

		void AHPTimeConstant(double dVal);
		double AHPTimeConstant();

		void BurstGMaxCa(double dVal);
		double BurstGMaxCa();

		void BurstVm(double dVal);
		double BurstVm();

		void BurstSm(double dVal);
		double BurstSm();

		void BurstMTimeConstant(double dVal);
		double BurstMTimeConstant();

		void BurstVh(double dVal);
		double BurstVh();

		void BurstSh(double dVal);
		double BurstSh();

		void BurstHTimeConstant(double dVal);
		double BurstHTimeConstant();
		
		void TonicStimulus(double dblVal);
		double TonicStimulus();
		
		void TonicNoise(double dblVal);
		double TonicNoise();

#pragma endregion

		void PreCalc(IntegrateFireNeuralModule *lpNS);
		void CalcUpdate(IntegrateFireNeuralModule *lpNS);
		void CalcUpdateFinal(IntegrateFireNeuralModule *lpNS);
		void PostCalc(IntegrateFireNeuralModule *lpNS);

		virtual int FindIonChannelListPos(string strID, BOOL bThrowError = TRUE);

		//Node Overrides
#pragma region DataAccesMethods
		virtual float *GetDataPointer(const string &strDataType);
		virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
		virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
		virtual BOOL AddItem(const string &strItemType, const string &strXml, BOOL bThrowError = TRUE, BOOL bDoNotInit = FALSE);
		virtual BOOL RemoveItem(const string &strItemType, const string &strID, BOOL bThrowError = TRUE);
#pragma endregion

		virtual void AddExternalNodeInput(float fltInput);
		virtual void ResetSimulation();
		//Node Overrides
					
		virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure,  AnimatSim::Behavior::NeuralModule *lpModule, Node *lpNode, BOOL bVerify);
		virtual void VerifySystemPointers();

	friend class IntegrateFireSim::IntegrateFireNeuralModule;
	};

}				//IntegrateFireSim
