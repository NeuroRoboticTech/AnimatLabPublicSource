/**
\file	Node.h

\brief	Declares the node class. 
**/

#pragma once


namespace AnimatSim
{
	/**
	\class	Node
	
	\brief	Base class for body parts and neural network nodes. 
	
	\details This is the base class used for body parts and network nodes. It contains
	links the simulation, its containing structure and the neural module. It also has some
	common methods and variables used by these types of objects.

	\author	dcofer
	\date	2/24/2011
	**/

	class ANIMAT_PORT Node : public AnimatBase 
	{
	protected:
		Simulator *m_lpSim; ///< The pointer to a simulation
		Structure *m_lpStructure; ///< The pointer to a structure
		NeuralModule *m_lpModule; ///< The pointer to a neuralmodule

		///Determines if this node is enabled. This will only have any effect if this node can be disabled.
		///The majority of nodes, like rigid bodies, can not be disabled.
		BOOL m_bEnabled;

		///Keeps track of the enabled state before this node is killed.
		///If the node is killed and then turned on again we can reset the
		///enabled state to what it was before it was killed.
		BOOL m_bEnabledMem;

		///This is used for reporting the enabled state in a GetDataPointer call.
		float m_fltEnabled;

		virtual void UpdateData(Simulator *lpSim, Structure *lpStructure);

	public:
		Node();
		virtual ~Node();

		virtual Simulator *GetSimulator();
		virtual Structure *GetStructure();
		virtual NeuralModule *GetNeuralModule();

		virtual BOOL Enabled();
		virtual void Enabled(BOOL bValue);

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

