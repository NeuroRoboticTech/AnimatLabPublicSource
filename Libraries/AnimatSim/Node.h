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

		/**
		\fn	virtual void Node::AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput);
		
		\brief	Adds an external node input. 

		\details This is used by the adapter to add a new external value to this node. It is up to the node
		to interpret what that value means. For example, if it is a neuron then it can interpret it to be 
		a current. This value is added to the current total so that multiple adapters can call this in a 
		given time step. It is cleared out to zero at the beginning of the time step.
		
		\param [in,out]	lpSim		The pointer to a simulation. 
		\param [in,out]	lpStructure	The pointer to a structure. 
		\param	fltInput			The new input. 
		**/
		virtual void AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput) = 0;

		/**
		\fn	virtual void Node::StepSimulation(Simulator *lpSim, Structure *lpStructure) = 0;
		
		
		\brief	Step the simulation for this object.

		\details This is called on an object each time it is stepped in the simulation.
		this is where its simulation code is processed. However, StepSimulation is not 
		necessarily called every single time that the simulation as a whole is stepped. 
		A good example of this is that neural modules can have different integration time
		steps. So a firing rate module may have a DT of 0.5 ms, while an integrate and fire
		model may have one of 0.1 ms. So the firing rate module would only get its StepSimulation
		method called every 5th time that the other module was called. This is all handed in 
		the StepSimulation method of the Simulator and NervousSystem.
		
		\param [in,out]	lpSim		The pointer to a simulation. 
		\param [in,out]	lpStructure	The pointer to a structure. 
		**/
		virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure) = 0;

		virtual void AttachSourceAdapter(Simulator *lpSim, Structure *lpStructure, Adapter *lpAdapter);
		virtual void AttachTargetAdapter(Simulator *lpSim, Structure *lpStructure, Adapter *lpAdapter);
		virtual float *GetDataPointer(string strDataType) = 0;
		virtual void Initialize(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule);
		virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule);
		virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
	};

}				//AnimatSim

