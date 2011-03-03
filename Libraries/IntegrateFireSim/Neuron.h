
#pragma once

class IntegrateFireNeuralModule;

/**
\namespace	IntegrateFireSim

\brief	Contains all of the classes to implement a basic integrate and fire neural model. 
**/
namespace IntegrateFireSim
{

	class ADV_NEURAL_PORT Neuron : public AnimatSim::Node
	{
	protected:
		IntegrateFireNeuralModule *m_lpRealModule;


	/////////////////////////////////////
	// LOADABLE PARAMETERS

		static double m_dSpikePeak;
		static double m_dSpikeStrength;
		static double m_dAHPEquilPot;		// equil pot for K
		static double m_dCaEquilPot;
		static double m_dAbsoluteRefr;

		float m_fltIonChannelStandin;

		int m_iNeuronID;

	// electrical state
		BOOL m_bZapped;
	// individual, basic properties
			double m_dRestingPot;
			double m_dSize;
			double m_dTimeConst;
			double m_dCm;
			double m_dInitialThresh;
			double m_dRelativeAccom;
			double m_dAccomTimeConst;
			double m_dAHPAmp;
			double m_dAHPTimeConst;
	// burster properties
			double m_dGMaxCa;
			double m_dVM;		//activation mid point
			double m_dSM;		// activation slope
			double m_dMTimeConst;	// activation time constant
			double m_dVH;		// inactivation
			double m_dSH;
			double m_dHTimeConst;

		CStdArray<double> m_aryTonicInputPeriod;
		CStdArray<int> m_aryTonicInputPeriodType;
		double m_dToniCurrentStimulusulus;
		double m_dNoise;

		CStdPtrArray<IonChannel> m_aryIonChannels;
		CaActivation *m_lpCaActive;
		CaActivation *m_lpCaInactive;

	//////////////////////////////
	// WORKING STUFF
		static double m_dDT;
		double m_dMemPot;
		double m_dNewMemPot;
		double m_dThresh;
		BOOL m_bSpike;		// spike flag

		// electrical synapse current
		double m_dElecSynCur;
		double m_dElecSynCond;
		// non-spiking chemical synapse current
		double m_dNonSpikingSynCur;
		double m_dNonSpikingSynCond;
		// calculation stuff
		double m_dRefrCountDown;
		double m_dDCTH;	// expon decline working factor for thresh accomm
		double m_dDGK;	// expon decline factor for AHP
		double m_dGK;	// cummulative amp of AHP conductance
		double m_dGTot;	// cummulative total contuctance
		float m_fltGTotal; //Reported g total

		double m_dStim;
		float m_fltAdapterI;
		float m_fltAdapterMemoryI;  //Used to allow datacharts to track current input from adapters.
		float m_fltExternalI;
		float m_fltChannelI;
		float m_fltChannelMemoryI;
		float m_fltTotalI;
		float m_fltTotalMemoryI;
		float m_fltMemPot;
		float m_fltThresholdMemory;  //Used to allow us to chart the threshold if needed.
		float m_fltElecSynCurMemory;
		float m_fltSpikingSynCurMemory;
		float m_fltNonSpikingSynCurMemory;
		float m_fltSpike;

		int m_iIonChannels;

		// for bursting	
		double m_dM;
		double m_dH;

		//Vars to calculate the firing frequency of this neuron.
		double m_fltLastSpikeTime;
		float m_fltFiringFreq;

		//Used to return the membrane conductance of this neuron in GetDataPointers.
		float m_fltGm;
		float m_fltVrest;

		CStdArray<double> m_arySynG;	// current conductance of each synaptic type
		
		CStdArray<double> m_aryFacilSponSynG;	// facilitated initial g increase caused by spontaneous input
		CStdArray<double> m_aryNextSponSynTime;	// time to next spontaneous occurrence of this syn type

		CStdArray<double> m_aryDG;	// exponential decline factor in syn G COULD THIS BE STATIC???? (or put in synapse??)
		CStdArray<double> m_aryFacilD;		// exponential decline factor in facilitation COULD THIS BE STATIC???? (or put in synapse??)

		virtual IonChannel *LoadIonChannel(CStdXml &oXml);
		IonChannel *FindIonChannel(string strID, BOOL bThrowError);

	protected:
		//void ClearSpikeTimes();
		//void StoreSpikeForFreqAnalysis(IntegrateFireNeuralModule *lpNS);
		void CalculateFiringFreq(IntegrateFireNeuralModule *lpNS);
		virtual void AddIonChannel(string strXml);
		virtual void RemoveIonChannel(string strID, BOOL bThrowError = TRUE);
	
	public:
		Neuron();
		virtual ~Neuron();
		virtual void Load(CStdXml &oXml);

		int NeuronID() {return m_iNeuronID;};
		void NeuronID(int iID) {m_iNeuronID = iID;};

		virtual BOOL Enabled() {return m_bZapped;};
		virtual void Enabled(BOOL bValue) {m_bZapped = bValue;};

	//////
	// ENGINE
		double GetRestingPot() {return m_dRestingPot;}
		double GetMemPot() {return m_bZapped?0:(m_bSpike?m_dSpikePeak:m_dMemPot);}
		double GetThresh() {return m_bZapped?0:m_dThresh;}
		BOOL GetSpike() {return m_bZapped?FALSE:m_bSpike;}
		BOOL GetZapped() {return m_bZapped;}
		void IncrementStim(double stim) {m_dStim+=stim;}
		void InElectricalSynapseCurr(double cur) {m_dElecSynCur+=cur;}
		void InElectricalSynapseCond(double cond) {m_dElecSynCond+=cond;}
		void IncNonSpikingSynCurr(double cur) {m_dNonSpikingSynCur+=cur;}
		void IncNonSpikingSynCond(double cond) {m_dNonSpikingSynCond+=cond;}

		CStdPtrArray<IonChannel> *IonChannels() {return &m_aryIonChannels;};

#pragma region Accessor-Mutators

		void RestingPotential(double dVal) 
		{
			//The mempot variables are calculated, so we do not want to just re-set them to the new value.
			//instead lets adjust them by the difference between the old and new resting potential.
			double dDiff = dVal - m_dRestingPot;

			m_dRestingPot = dVal;
			m_dMemPot += dDiff;
			m_dNewMemPot += dDiff;
		};
		double RestingPotential() {return m_dRestingPot;};

		void Size(double dVal) 
		{
			m_dSize = dVal;
			m_fltGm = (float) (1/(m_dSize*1e6));
			m_dCm = m_dTimeConst*m_dSize;
		};
		double Size() {return m_dSize;};

		void TimeConstant(double dVal) 
		{
			m_dTimeConst = dVal;
			m_dCm = m_dTimeConst*m_dSize;
		};
		double TimeConstant() {return m_dTimeConst;};

		void InitialThreshold(double dVal) {m_dInitialThresh = dVal;};
		double InitialThreshold() {return m_dInitialThresh;};

		void RelativeAccomodation(double dVal) {m_dRelativeAccom = dVal;};
		double RelativeAccomodation() {return m_dRelativeAccom;};

		void AccomodationTimeConstant(double dVal) 
		{
			m_dAccomTimeConst = dVal;
			m_dDCTH=exp(-m_dDT/m_dAccomTimeConst);
		};
		double AccomodationTimeConstant() {return m_dAccomTimeConst;};

		void AHPAmplitude(double dVal) {m_dAHPAmp = dVal;};
		double AHPAmplitude() {return m_dAHPAmp;};

		void AHPTimeConstant(double dVal) 
		{
			m_dAHPTimeConst = dVal;
			m_dDGK=exp(-m_dDT/m_dAHPTimeConst);
		};
		double AHPTimeConstant() {return m_dAHPTimeConst;};

		void BurstGMaxCa(double dVal) {m_dGMaxCa = dVal;};
		double BurstGMaxCa() {return m_dGMaxCa;};

		void BurstVm(double dVal) {m_dVM = dVal;};
		double BurstVm() {return m_dVM;};

		void BurstSm(double dVal) {m_dSM = dVal;};
		double BurstSm() {return m_dSM;};

		void BurstMTimeConstant(double dVal) {m_dMTimeConst = dVal;};
		double BurstMTimeConstant() {return m_dMTimeConst;};

		void BurstVh(double dVal) {m_dVH = dVal;};
		double BurstVh() {return m_dVH;};

		void BurstSh(double dVal) {m_dSH = dVal;};
		double BurstSh() {return m_dSH;};

		void BurstHTimeConstant(double dVal) {m_dHTimeConst = dVal;};
		double BurstHTimeConstant() {return m_dHTimeConst;};
		
#pragma endregion

		void PreCalc(IntegrateFireNeuralModule *lpNS);
		void CalcUpdate(IntegrateFireNeuralModule *lpNS);
		void CalcUpdateFinal(IntegrateFireNeuralModule *lpNS);
		void PostCalc(IntegrateFireNeuralModule *lpNS);

		virtual int FindIonChannelListPos(string strID, BOOL bThrowError = TRUE);

		//Node Overrides
#pragma region DataAccesMethods
		virtual float *GetDataPointer(string strDataType);
		virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
		virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE);
		virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);
#pragma endregion

		virtual void AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput);
		virtual void ResetSimulation(Simulator *lpSim, Structure *lpStruct);
		virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
		//Node Overrides

	friend class IntegrateFireNeuralModule;
	};

}				//IntegrateFireSim
