// Brain.h: interface for the FiringRateModule class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_BRAIN_H__092F1F0E_3D90_4E91_81E8_19022622D6EA__INCLUDED_)
#define AFX_BRAIN_H__092F1F0E_3D90_4E91_81E8_19022622D6EA__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace FiringRateSim
{

	class FAST_NET_PORT FiringRateModule : public AnimatSim::Behavior::NeuralModule  
	{
	protected:
		CStdIPoint m_oNetworkSize;
		CStdPtrArray<Neuron> m_aryNeurons;

		BOOL m_bActiveArray;

		Neuron *LoadNeuron(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
		void LoadNetworkXml(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);

		void GenerateAutoSeed();

	public:
		FiringRateModule();
		virtual ~FiringRateModule();

		//NeuralModule overrides
		virtual string ModuleName() {return Nl_NeuralModuleName();};

		virtual void Kill(Simulator *lpSim, Organism *lpOrganism, BOOL bState = TRUE);
		virtual void ResetSimulation(Simulator *lpSim, Organism *lpOrganism);

		virtual void Initialize(Simulator *lpSim, Structure *lpStructure);
		virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
		virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
		virtual void Save(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml) {};

#pragma region DataAccesMethods
		virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
		virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE);
		virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);
#pragma endregion

		virtual void AddNeuron(string strXml);
		virtual void RemoveNeuron(string strID, BOOL bThrowError = TRUE);
		virtual int FindNeuronListPos(string strID, BOOL bThrowError = TRUE);

		BOOL ActiveArray();
		void ActiveArray(BOOL bVal);
		BOOL InactiveArray();
		void InactiveArray(BOOL bVal);

#pragma region SnapshotMethods
			virtual long CalculateSnapshotByteSize();
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);
#pragma endregion

	};

}				//FiringRateSim

#endif // !defined(AFX_BRAIN_H__092F1F0E_3D90_4E91_81E8_19022622D6EA__INCLUDED_)
