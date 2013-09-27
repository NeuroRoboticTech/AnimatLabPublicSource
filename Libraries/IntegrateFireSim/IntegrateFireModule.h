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
		bool m_bTTX;

		/// true if cadium is applied to the nervous system.
		bool m_bCd;

		/// true if this nervous system is using a hodgkin-huxely model.
		bool m_bHH;

	// NervousSystem/Synapses/SpikingSynapses
		/// true to retain hebbian memory
		bool m_bRetainHebbMemory;

		/// true to use critical period during hebbian learning.
		bool m_bUseCriticalPeriod;

		/// The start time of the critical period for hebbian learning.
		double m_dStartCriticalPeriod;

		/// The end time of the critical period for hebbian learning.
		double m_dEndCriticalPeriod;

		/// true to freeze hebbian learning.
		bool m_bFreezeHebb;

	// internal Hebb stuff
		/// true if hebbian learning needs to be initialized
		bool m_bNeedInitialiseHebb;		

		/// true to randomise hebbian learning values.
		bool m_bRandomisedHebb;

		/// true to freeze learning.
		bool m_bFreezeLearning;	// used internally as flag, not saved

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

		void Cd(bool bVal);
		bool Cd();

		void TTX(bool bVal);
		bool TTX();

		void HH(bool bVal);
		bool HH();

		virtual void TimeStep(float fltVal);
		virtual float TimeStep();

		void RetainHebbMemory(bool bVal);
		bool RetainHebbMemory();

		void UseCriticalPeriod(bool bVal);
		bool UseCriticalPeriod();

		void StartCriticalPeriod(double dVal); 
		double StartCriticalPeriod();

		void EndCriticalPeriod(double dVal);
		double EndCriticalPeriod();

		void FreezeHebb(bool bVal);
		bool FreezeHebb();

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
		virtual std::string ModuleName();

#pragma endregion


	// the CALCULATION
		void PreCalc();
		void CalcUpdate();
		void PostCalc();
		double GetScaleElecCond(double minG,double maxG,double jV, double ThreshV,double SaturateV);
		void ScaleCondForVoltDep(double& G,double postV,double maxV,double minV,double scl);
		void ScaleCondForNonSpiking(double& G,double PreV,double ThreshV,double SaturateV);


#pragma region DataAccesMethods
		virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
		virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);
		virtual bool AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError = true, bool bDoNotInit = false);
		virtual bool RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError = true);
#pragma endregion

		virtual void AddNeuron(std::string strXml, bool bDoNotInit);
		virtual void RemoveNeuron(std::string strID, bool bThrowError = true);
		virtual int FindNeuronListPos(std::string strID, bool bThrowError = true);

		virtual void AddSynapse(std::string strXml, bool bDoNotInit);
		virtual void RemoveSynapse(std::string strID, bool bThrowError = true);
		virtual int FindSynapseListPos(std::string strID, bool bThrowError = true);

		virtual void AddSynapseType(std::string strXml, bool bDoNotInit);
		virtual void RemoveSynapseType(std::string strID, bool bThrowError = true);
		virtual int FindSpikingChemListPos(std::string strID, bool bThrowError = true);
		virtual int FindNonSpikingChemListPos(std::string strID, bool bThrowError = true);
		virtual int FindElectricalListPos(std::string strID, bool bThrowError = true);

		virtual void Kill(bool bState = true);

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

