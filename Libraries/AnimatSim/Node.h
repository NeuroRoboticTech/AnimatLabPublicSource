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
		/// The pointer to this node's organism
		Organism *m_lpOrganism;

		/// Keeps track of the enabled state at sim startup.
		bool m_bInitEnabled;

		///This is used for reporting the enabled state in a GetDataPointer call.
		float m_fltEnabled;

		///If this is true then this node represents a whole bunch of similar nodes.
		///It creates these nodes when it is created and when it is modified it modifies all the other nodes
		bool m_bTemplateNode;

		///This tells how many nodes are represented by this node.
		int m_iTemplateNodeCount;

		///This is a python change script that is run any time this template node is modified.
		std::string m_strTemplateChangeScript;

		CStdArray<std::string> m_aryTemplateChildNodes;

		virtual void UpdateData();

		virtual void SetupTemplateNodes();
		virtual void DestroyTemplateNodes();
		virtual void TemplateNodeChanged();
		
	public:
		Node();
		virtual ~Node();

		static Node *CastToDerived(AnimatBase *lpBase) {return static_cast<Node*>(lpBase);}

		virtual bool Enabled();
		virtual void Enabled(bool bValue);

		virtual void Kill(bool bState = true);

		virtual bool TemplateNode();
		virtual void TemplateNode(bool bVal);

		virtual int TemplateNodeCount();
		virtual void TemplateNodeCount(int iVal);

		virtual std::string TemplateChangeScript();
		virtual void TemplateChangeScript(std::string strVal);

		virtual void Copy(CStdSerialize *lpSource);

		/**
		\brief	Adds an external node input.
		
		\details This is used by the adapter to add a new external value to this node. It is up to the
		node to interpret what that value means. For example, if it is a neuron then it can interpret it
		to be a current. This value is added to the current total so that multiple adapters can call this
		in a given time step. It is cleared out to zero at the beginning of the time step. You can now also 
		specify which data you are adding to for this method call. This allows adapters to be setup to change
		multiple different variables in the system.
		
		\author	dcofer
		\date	6/16/2014
		
		\param	iTargetDataType	The index of the target data type we are adding to. 
		\param	fltInput	The new input. 
		**/
		virtual void AddExternalNodeInput(int iTargetDataType, float fltInput) = 0;

		virtual int GetTargetDataTypeIndex(const std::string &strDataType);

		virtual void ResetSimulation();

#pragma region DataAccesMethods

			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify);
			virtual void VerifySystemPointers();
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

	};

}				//AnimatSim

