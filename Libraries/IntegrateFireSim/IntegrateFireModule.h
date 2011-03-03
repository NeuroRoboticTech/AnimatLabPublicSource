// IntegrateFireModule.h: interface for the IntegrateFireNeuralModule class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_NERVOUSSYSTEM_H__6581CA8B_B028_4C79_A98B_33514514B867__INCLUDED_)
#define AFX_NERVOUSSYSTEM_H__6581CA8B_B028_4C79_A98B_33514514B867__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

// these forward declarations are necessary because 
//class Neuron;
//class SpikingChemicalSynapse;
//class NonSpikingChemicalSynapse;
//class ElectricalSynapse;
//class CurrentStimulus;
//class Connexion;

namespace IntegrateFireSim
{

	class ADV_NEURAL_PORT IntegrateFireNeuralModule : public AnimatSim::Behavior::NeuralModule 
	{
	protected:
		// NervousSystem
		double m_dTimeStep;

		BOOL m_bTTX;
		BOOL m_bCd;
		BOOL m_bHH;

	// NervousSystem/Synapses/SpikingSynapses
		BOOL m_bRetainHebbMemory;
		BOOL m_bUseCriticalPeriod;
		double m_dStartCriticalPeriod;
		double m_dEndCriticalPeriod;
		BOOL m_bFreezeHebb;

	// internal Hebb stuff
		BOOL m_bNeedInitialiseHebb;		
		BOOL m_bRandomisedHebb;
		BOOL m_bFreezeLearning;	// used internally as flag, not saved

		CStdPtrArray<Neuron> m_aryNeurons;
		CStdPtrArray<SpikingChemicalSynapse> m_arySpikingChemSyn;
		CStdPtrArray<NonSpikingChemicalSynapse> m_aryNonSpikingChemSyn;
		CStdPtrArray<ElectricalSynapse> m_aryElecSyn;
		//CStdPtrArray<CurrentStimulus> m_aryStim;
		CStdPtrArray<Connexion> m_aryConnexion;

		Neuron *LoadNeuron(CStdXml &oXml);
		SynapseType *LoadSynapseType(CStdXml &oXml);
		SpikingChemicalSynapse *LoadSpikingChemSyn(CStdXml &oXml, int iIndex);
		NonSpikingChemicalSynapse *LoadNonSpikingChemSyn(CStdXml &oXml, int iIndex);
		ElectricalSynapse *LoadElecSyn(CStdXml &oXml, int iIndex);
		//CurrentStimulus *LoadStim(CStdXml &oXml);
		Connexion *LoadConnexion(CStdXml &oXml);
		void InitSynapse(Connexion *pCx);

	/////////////////////////////
	// ENGINE
		double m_dCurrentTime;

		virtual void LoadInternal(CStdXml &oXml);

	public:
		IntegrateFireNeuralModule();
		virtual ~IntegrateFireNeuralModule();

		void ResetIDs();

	////////////////////////
		void SetCurrentTime(double t) {m_dCurrentTime=t;}
		double GetCurrentTime() {return m_dCurrentTime;}
		double GetTimeStep() {return m_dTimeStep;}

	// neuron stuff
		int GetNeuronCount() {return m_aryNeurons.size();}
		Neuron *GetNeuronAt(int i) {return m_aryNeurons[i];}

	// stim stuff
		//int GetStimCount() {return m_aryStim.size();}
		//CurrentStimulus *GetStimAt(int i) {return m_aryStim[i];}

	// connexion stuff
		int GetConnexionCount() {return m_aryConnexion.size();}
		Connexion *GetConnexionAt(int i) {return m_aryConnexion[i];}

#pragma region Accessor-Mutators

		void Cd(BOOL bVal) {m_bCd = bVal;}; 
		BOOL Cd() {return m_bCd;}

		void TTX(BOOL bVal) {m_bTTX = bVal;}; 
		BOOL TTX() {return m_bTTX;}

		void HH(BOOL bVal) {m_bHH = bVal;}; 
		BOOL HH() {return m_bHH;}

		virtual void TimeStep(float fltVal);
		virtual float TimeStep() {return m_fltTimeStep;};

		void RetainHebbMemory(BOOL bVal) {m_bRetainHebbMemory = bVal;}; 
		BOOL RetainHebbMemory() {return m_bRetainHebbMemory;};

		void UseCriticalPeriod(BOOL bVal) {m_bUseCriticalPeriod = bVal;}; 
		BOOL UseCriticalPeriod() {return m_bUseCriticalPeriod;};

		void StartCriticalPeriod(double dVal) {m_dStartCriticalPeriod = dVal;}; 
		double StartCriticalPeriod() {return m_dStartCriticalPeriod;};

		void EndCriticalPeriod(double dVal) {m_dEndCriticalPeriod = dVal;}; 
		double EndCriticalPeriod() {return m_dEndCriticalPeriod;};

		void FreezeHebb(BOOL bVal) {m_bFreezeHebb = bVal;}; 
		BOOL FreezeHebb() {return m_bFreezeHebb;};

		void SpikePeak(double dVal); 
		double SpikePeak();

		void SpikeStrength(double dVal); 
		double SpikeStrength();

		void CaEquilPot(double dVal); 
		double CaEquilPot();

		void AbsoluteRefr(double dVal); 
		double AbsoluteRefr();

#pragma endregion

	// Synapse stuff
		int GetSpikingChemSynCount() {return m_arySpikingChemSyn.size();}
		SpikingChemicalSynapse *GetSpikingChemSynAt(int i) {return m_arySpikingChemSyn[i];}
		int GetNonSpikingChemSynCount() {return m_aryNonSpikingChemSyn.size();}
		NonSpikingChemicalSynapse *GetNonSpikingChemSynAt(int i) {return m_aryNonSpikingChemSyn[i];}
		int GetElecSynCount() {return m_aryElecSyn.size();}
		ElectricalSynapse *GetElecSynAt(int i) {return m_aryElecSyn[i];}

	// the CALCULATION
		void PreCalc();
		void CalcUpdate();
		void PostCalc();
		double GetScaleElecCond(double minG,double maxG,double jV, double ThreshV,double SaturateV);
		void ScaleCondForVoltDep(double& G,double postV,double maxV,double minV,double scl);
		void ScaleCondForNonSpiking(double& G,double PreV,double ThreshV,double SaturateV);


		//NeuralModule Overrides
		virtual string ModuleName() {return Rn_NeuralModuleName();};

#pragma region DataAccesMethods
		virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
		virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE);
		virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);
#pragma endregion

		virtual void AddNeuron(string strXml);
		virtual void RemoveNeuron(string strID, BOOL bThrowError = TRUE);
		virtual int FindNeuronListPos(string strID, BOOL bThrowError = TRUE);

		virtual void AddSynapse(string strXml);
		virtual void RemoveSynapse(string strID, BOOL bThrowError = TRUE);
		virtual int FindSynapseListPos(string strID, BOOL bThrowError = TRUE);

		virtual void AddSynapseType(string strXml);
		virtual void RemoveSynapseType(string strID, BOOL bThrowError = TRUE);
		virtual int FindSpikingChemListPos(string strID, BOOL bThrowError = TRUE);
		virtual int FindNonSpikingChemListPos(string strID, BOOL bThrowError = TRUE);
		virtual int FindElectricalListPos(string strID, BOOL bThrowError = TRUE);

		virtual void Kill(Simulator *lpSim, Organism *lpOrganism, BOOL bState = TRUE);
		virtual void ResetSimulation(Simulator *lpSim, Organism *lpOrganism);

		virtual void Initialize(Simulator *lpSim, Structure *lpStructure);

		virtual long CalculateSnapshotByteSize()  {return 0;};
		virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex) {};
		virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex) {};

		virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
		virtual void Load(CStdXml &oXml);
		//NeuralModule Overrides

	};

}				//IntegrateFireSim


#endif // !defined(AFX_NERVOUSSYSTEM_H__6581CA8B_B028_4C79_A98B_33514514B867__INCLUDED_)
