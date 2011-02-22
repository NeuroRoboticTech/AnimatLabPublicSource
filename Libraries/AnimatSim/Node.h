/**
\file	Node.h

\brief	Base class for node items.

**/

#pragma once


namespace AnimatSim
{
	/**
	\class	Node
	
	\brief	Base class for node items.

	\details Base class for 
	
	\author	Dacofer
	\date	2/21/2011
	**/

	class ANIMAT_PORT Node : public AnimatBase 
	{
	protected:
		Simulator *m_lpSim;
		Structure *m_lpStructure;
		NeuralModule *m_lpModule;

		///Determines if this node is enabled. This will only have any effect if this node can be disabled.
		///The majority of nodes, like rigid bodies, can not be disabled.
		BOOL m_bEnabled;

		///Keeps track of the enabled state before this node is killed.
		///If the node is killed and then turned on again we can reset the
		///enabled state to what it was before it was killed.
		BOOL m_bEnabledMem;

		///This is for reporting purposes.
		float m_fltEnabled;

		virtual void UpdateData(Simulator *lpSim, Structure *lpStructure);

	public:
		Node();
		virtual ~Node();

		virtual Simulator *GetSimulator() {return m_lpSim;};
		virtual Structure *GetStructure() {return m_lpStructure;};
		virtual NeuralModule *GetNeuralModule() {return m_lpModule;};

		virtual BOOL Enabled() {return m_bEnabled;};
		virtual void Enabled(BOOL bValue) 
		{
			m_bEnabled = bValue;
			m_fltEnabled = (float) m_bEnabled;
		};

		virtual void Kill(Simulator *lpSim, Organism *lpOrganism, BOOL bState = TRUE);
		virtual void ResetSimulation(Simulator *lpSim, Structure *lpStructure) = 0;
		virtual void AfterResetSimulation(Simulator *lpSim, Structure *lpStructure) {};

		virtual void AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput) = 0;
		virtual void AttachSourceAdapter(Simulator *lpSim, Structure *lpStructure, Adapter *lpAdapter);
		virtual void AttachTargetAdapter(Simulator *lpSim, Structure *lpStructure, Adapter *lpAdapter);
		virtual float *GetDataPointer(string strDataType) = 0;
		virtual void *GetDataItem(string strItemType, string strID, BOOL bThrowError = TRUE); 
		virtual void Initialize(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule);
		virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule);
		virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure) = 0;
		virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
		virtual void Save(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml) {};
	};

}				//AnimatSim

