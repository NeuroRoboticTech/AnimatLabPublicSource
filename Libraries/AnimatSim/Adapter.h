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

			virtual void Initialize();
			virtual string SourceModule() {return m_strSourceModule;};
			virtual string TargetModule() {return m_strTargetModule;};

			//Node Overrides
			virtual void AddExternalNodeInput(float fltInput);
			virtual void AttachSourceAdapter(Structure *lpStructure, Node *lpNode);
			virtual void AttachTargetAdapter(Structure *lpStructure, Node *lpNode);
			virtual float *GetDataPointer(string strDataType);
			virtual void ResetSimulation() {};
			virtual void StepSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}			//Adapters
}				//AnimatSim
