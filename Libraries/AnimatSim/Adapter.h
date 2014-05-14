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
			/// Name of the source NeuralModule
			std::string m_strSourceModule;

			/// GUID ID of the source node.
			std::string m_strSourceID;

			/// DateType of the source variable that will be converted. This is retrieved using the GetDataPointer method.
			std::string m_strSourceDataType;

			/// Pointer to the source node
			Node *m_lpSourceNode;

			/// Pointer to the source data varaible.
			float *m_lpSourceData;

			/// Name of the target NeuralModule
			std::string m_strTargetModule;

			/// GUID ID of the target node.
			std::string m_strTargetID;

			/// DateType of the target variable that will be converted. This is retrieved using the GetDataPointer method.
			std::string m_strTargetDataType;

			/// Pointer to the target node
			Node *m_lpTargetNode;

			/// Pointer to the Gain that will be used to convert the source value into the target value.
			Gain *m_lpGain;

			///This is true if either the target or soruce are connected to the physics engine. It is false
			///if target and source are neural only.
			bool m_bConnectedToPhysics;

			///This is the previous value that was passed into the adpters target
			float m_fltPrevVal;

			virtual void AddGain(std::string strXml);
			virtual void SetOriginID(std::string strXml);
			virtual void SetDestinationID(std::string strXml);

		public:
			Adapter();
			virtual ~Adapter();

			virtual std::string SourceModule();
			virtual void SourceModule(std::string strName);

			virtual std::string SourceID();
			virtual void SourceID(std::string strID);

			virtual std::string SourceDataType();
			virtual void SourceDataType(std::string strType);

			virtual Node *SourceNode();

			virtual std::string TargetModule();
			virtual void TargetModule(std::string strName);

			virtual std::string TargetID();
			virtual void TargetID(std::string strID);

			virtual std::string TargetDataType();
			virtual void TargetDataType(std::string strType);

			virtual Node *TargetNode();

			virtual Gain *GetGain();
			virtual void SetGain(Gain *lpGain);

			virtual bool ConnectedToPhysics();

			virtual void DetachAdaptersFromSimulation();

			virtual void Initialize();
			virtual void AddExternalNodeInput(float fltInput);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
			virtual float *GetDataPointer(const std::string &strDataType);
			virtual void ResetSimulation() {};
			virtual void StepSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}			//Adapters
}				//AnimatSim
