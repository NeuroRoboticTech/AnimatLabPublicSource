#pragma once

namespace AnimatSim
{

	/**
	\namespace	AnimatSim::Adapters

	\brief	Namespace for the adapter objects.

	\details Adapters are generalized mechanism that allows items in one module to talk to items in another. For example, 
	It allows a neuron in a firing rate neural module to inject current into an integrate and fire neuron, or to talk to the
	physics engine.
	**/
	namespace Adapters
	{

		/**
		\class	Adapter
		
		\brief	Adapter. 
		
		\details Adapters are generalized mechanism that allows items in one module to talk to items in another. For example, 
		It allows a neuron in a firing rate neural module to inject current into an integrate and fire neuron, or to talk to the
		physics engine.

		\author	dcofer
		\date	2/28/2011
		**/
		class ANIMAT_PORT Adapter : public Node 
		{
		protected:
			string m_strSourceModule;
			string m_strSourceID;
			string m_strSourceDataType;
			Node *m_lpSourceNode;
			float *m_lpSourceData;

			string m_strTargetModule;
			string m_strTargetID;
			string m_strTargetDataType;
			Node *m_lpTargetNode;

			Gain *m_lpGain;

		public:
			Adapter();
			virtual ~Adapter();

			virtual void Initialize(Simulator *lpSim, Structure *lpStructure);
			virtual string SourceModule() {return m_strSourceModule;};
			virtual string TargetModule() {return m_strTargetModule;};

			//Node Overrides
			virtual void AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput);
			virtual void AttachSourceAdapter(Simulator *lpSim, Structure *lpStructure, Node *lpNode);
			virtual void AttachTargetAdapter(Simulator *lpSim, Structure *lpStructure, Node *lpNode);
			virtual float *GetDataPointer(string strDataType);
			virtual void ResetSimulation(Simulator *lpSim, Structure *lpStructure) {};
			virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
			virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			virtual void Save(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
		};

	}			//Adapters
}				//AnimatSim
