/**
\file
C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\IntegrateFireSim\IntegrateFireModule.h

\brief	Declares the integrate fire module class.
**/

#pragma once


namespace IntegrateFireSim
{
	/**
	\brief	Integrate and fire neural module. 

	\details This neural module implements the integrate and fire neural models.
	
	\author	dcofer
	\date	3/30/2011
	**/
	class ADV_NEURAL_PORT IntegrateFireNeuralModule : public AnimatSim::Behavior::NeuralModule 
	{
	protected:
		// NervousSystem
		/// The time step of the neural module
		double m_dTimeStep;

		/// true if ttx drug is applied to the nervous system.
		BOOL m_bTTX;

		/// true if cadium is applied to the nervous system.
		BOOL m_bCd;

		/// true if this nervous system is using a hodgkin-huxely model.
		BOOL m_bHH;

	// NervousSystem/Synapses/SpikingSynapses
		/// true to retain hebbian memory
		BOOL m_bRetainHebbMemory;

		/// true to use critical period during hebbian learning.
		BOOL m_bUseCriticalPeriod;

		/// The start time of the critical period for hebbian learning.
		double m_dStartCriticalPeriod;

		/// The end time of the critical period for hebbian learning.
		double m_dEndCriticalPeriod;

		/// true to freeze hebbian learning.
		BOOL m_bFreezeHebb;

	// internal Hebb stuff
		/// true if hebbian learning needs to be initialized
		BOOL m_bNeedInitialiseHebb;		

		/// true to randomise hebbian learning values.
		BOOL m_bRandomisedHebb;

		/// true to freeze learning.
		BOOL m_bFreezeLearning;	// used internally as flag, not saved

		/// The array of neurons in this neural module.
		CStdPtrArray<Neuron> m_aryNeurons;

		/// The array of spiking chem synapses in this neural module.
		CStdPtrArray<SpikingChemicalSynapse> m_arySpikingChemSyn;

		/// The array of non-spiking chemical synapses in this neural module.
		CStdPtrArray<NonSpikingChemicalSynapse> m_aryNonSpikingChemSyn;

		/// The array of electrical synapses in this neural module.
		CStdPtrArray<ElectricalSynapse> m_aryElecSyn;

		/// The array of connexions in this neural module.
		CStdPtrArray<Connexion> m_aryConnexion;

		Neuron *LoadNeuron(CStdXml &oXml);
		SynapseType *LoadSynapseType(CStdXml &oXml);
		SpikingChemicalSynapse *LoadSpikingChemSyn(CStdXml &oXml, int iIndex);
		NonSpikingChemicalSynapse *LoadNonSpikingChemSyn(CStdXml &oXml, int iIndex);
		ElectricalSynapse *LoadElecSyn(CStdXml &oXml, int iIndex);
		Connexion *LoadConnexion(CStdXml &oXml);
		void InitSynapse(Connexion *pCx);

	/////////////////////////////
	// ENGINE
		/// Current time of the simulation
		double m_dCurrentTime;

		virtual void LoadInternal(CStdXml &oXml);

	public:
		IntegrateFireNeuralModule();
		virtual ~IntegrateFireNeuralModule();

		void ResetIDs();

#pragma region Accessor-Mutators

	////////////////////////
		void SetCurrentTime(double t);
		double GetCurrentTime();
		double GetTimeStep();

	// neuron stuff
		int GetNeuronCount();
		Neuron *GetNeuronAt(int i);

	// connexion stuff
		int GetConnexionCount();
		Connexion *GetConnexionAt(int i);

		void Cd(BOOL bVal);
		BOOL Cd();

		void TTX(BOOL bVal);
		BOOL TTX();

		void HH(BOOL bVal);
		BOOL HH();

		virtual void TimeStep(float fltVal);
		virtual float TimeStep();

		void RetainHebbMemory(BOOL bVal);
		BOOL RetainHebbMemory();

		void UseCriticalPeriod(BOOL bVal);
		BOOL UseCriticalPeriod();

		void StartCriticalPeriod(double dVal); 
		double StartCriticalPeriod();

		void EndCriticalPeriod(double dVal);
		double EndCriticalPeriod();

		void FreezeHebb(BOOL bVal);
		BOOL FreezeHebb();

		void SpikePeak(double dVal); 
		double SpikePeak();

		void SpikeStrength(double dVal); 
		double SpikeStrength();

		void CaEquilPot(double dVal); 
		double CaEquilPot();

		void AbsoluteRefr(double dVal); 
		double AbsoluteRefr();

		void AHPEquilPot(double dVal); 
		double AHPEquilPot();

		// Synapse stuff
		int GetSpikingChemSynCount();
		SpikingChemicalSynapse *GetSpikingChemSynAt(int i);
		int GetNonSpikingChemSynCount();
		NonSpikingChemicalSynapse *GetNonSpikingChemSynAt(int i);
		int GetElecSynCount();
		ElectricalSynapse *GetElecSynAt(int i);

		//NeuralModule Overrides
		virtual string ModuleName();

#pragma endregion


	// the CALCULATION
		void PreCalc();
		void CalcUpdate();
		void PostCalc();
		double GetScaleElecCond(double minG,double maxG,double jV, double ThreshV,double SaturateV);
		void ScaleCondForVoltDep(double& G,double postV,double maxV,double minV,double scl);
		void ScaleCondForNonSpiking(double& G,double PreV,double ThreshV,double SaturateV);


#pragma region DataAccesMethods
		virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
		virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
		virtual BOOL AddItem(const string &strItemType, const string &strXml, BOOL bThrowError = TRUE, BOOL bDoNotInit = FALSE);
		virtual BOOL RemoveItem(const string &strItemType, const string &strID, BOOL bThrowError = TRUE);
#pragma endregion

		virtual void AddNeuron(string strXml, BOOL bDoNotInit);
		virtual void RemoveNeuron(string strID, BOOL bThrowError = TRUE);
		virtual int FindNeuronListPos(string strID, BOOL bThrowError = TRUE);

		virtual void AddSynapse(string strXml, BOOL bDoNotInit);
		virtual void RemoveSynapse(string strID, BOOL bThrowError = TRUE);
		virtual int FindSynapseListPos(string strID, BOOL bThrowError = TRUE);

		virtual void AddSynapseType(string strXml, BOOL bDoNotInit);
		virtual void RemoveSynapseType(string strID, BOOL bThrowError = TRUE);
		virtual int FindSpikingChemListPos(string strID, BOOL bThrowError = TRUE);
		virtual int FindNonSpikingChemListPos(string strID, BOOL bThrowError = TRUE);
		virtual int FindElectricalListPos(string strID, BOOL bThrowError = TRUE);

		virtual void Kill(BOOL bState = TRUE);

		virtual long CalculateSnapshotByteSize()  {return 0;};
		virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex) {};
		virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex) {};

		virtual void ResetSimulation();
		virtual void Initialize();
		virtual void StepSimulation();
		virtual void TimeStepModified();
		virtual void Load(CStdXml &oXml);
		//NeuralModule Overrides

	};

}				//IntegrateFireSim

