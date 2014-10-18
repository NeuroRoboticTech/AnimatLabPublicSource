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
		
		///The delay buffer mode for adapters. This lets the user specify if they want a delay buffer, and if so then should
		///it only be active during the simulation or always?
		enum eDelayBufferMode
		{
			NoDelayBuffer,
			DelayBufferInSimOnly,
			DelayBufferAlwaysOn
		};

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

			/// DateType of the target variable that will be converted. 
			///This is converted to an int and passed to AddExternalNodeInput to 
			///define where the input should be routed.
			std::string m_strTargetDataType;

			///The m_strTargetDataType string is converted to an integer for use within the simulation loop to maximixe speed.
			int m_iTargetDataType;

			/// Pointer to the target node
			Node *m_lpTargetNode;

			/// Pointer to the Gain that will be used to convert the source value into the target value.
			Gain *m_lpGain;

			///This is true if either the target or soruce are connected to the physics engine. It is false
			///if target and source are neural only.
			bool m_bConnectedToPhysics;

			///This is the value that will was calculated for this adpater. If you are using a delay buffer this may be different than m_fltNextVal
			float m_fltCalculatedVal;

			///This is the value that will be passed into the adpters target
			float m_fltNextVal;

			//Determines whether this adapter uses a delay buffer. 
			eDelayBufferMode m_eDelayBufferMode;

			//The duration of the delay buffer.
			float m_fltDelayBufferInterval;

			///This buffer is used if the adapter has been setup to have delays between calcuating values and 
			///setting them in the target object. 
			CStdCircularArray<float> m_aryDelayBuffer;

			///Determines whether the m_bRobotAdpaterSynch flag applies to this adapter. Adpaters between neural elements should not 
			///need to be synched because they are not dependent on IO timing. This flag allows you to control this by setting it to false
			///for adapters that do not need it.
			bool m_bSynchWithRobot;

			///This is how often we need to update this particular adapter
			float m_fltSynchUpdateInterval;

			///The number of ticks between each call to update for this adapter till the next update time.
			int m_iSynchUpdateInterval;

			///This determines the interval after IO Disable duration when the first update of this adapter takes place.
			///This is primarily used to allow you to simulate IO updates are that are distributed over time, like in a round
			///robin scheme. An example of this is the Dynamixel USB motors that get updated in a round robin. 
			float m_fltSynchUpdateStartInterval;

			///The number of ticks between the first call to update for this adapter till the next update time.
			int m_iSynchUpdateStartInterval;

			///The target count for synching
			int m_iSynchTarget;

			///Keeps track of the last time we did a synch for the robot.
			int m_iSynchCount;

			///If you are modeling a robot then you can use this to scale the IO of this adapter to match the real response of the robot.
			///An example where this might be used is while simulating a motor. Real motors usually end up having a slightly slower response
			///time than you get in the simulation. The value specified here is a percentage with 1 at 100%. To slow the simulated response time
			///down slightly you could set it to 0.95 instead. This will only be applied during the simulation, NOT during the running of the real
			///robot. This is only so you can try and tune your simulation repsonse to more closely match the real robot response.
			float m_fltRobotIOScale;

			///It is sometimes useful to disable IO operations briefly at the start of the simulation to give neural systems a chance to stabilize.
			///This param defines how long it should disable IO for this adapter at the start of the sim.
			float m_fltInitIODisableDuration;

			///Set to 1 each time the adapter was updated and 0 otherwise. Use to chart when the adpater was updated.
			float m_fltUpdatedValue;

			virtual void AddGain(std::string strXml);
			virtual void SetOriginID(std::string strXml);
			virtual void SetDestinationID(std::string strXml);

			virtual void SetDelayBufferSize();

			virtual bool NeedsRobotSynch();

			virtual void ApplyExternalNodeInput(int iTargetDataType, float fltNextVal);

		public:
			Adapter();
			virtual ~Adapter();
							
			static Adapter *CastToDerived(AnimatBase *lpBase) {return static_cast<Adapter*>(lpBase);}

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

			virtual eDelayBufferMode DelayBufferMode();
			virtual void DelayBufferMode(eDelayBufferMode eMode);

			virtual float DelayBufferInterval();
			virtual void DelayBufferInterval(float fltVal);

			virtual void DetachAdaptersFromSimulation();

			virtual float RobotIOScale();
			virtual void RobotIOScale(float fltVal);

			virtual bool SynchWithRobot();
			virtual void SynchWithRobot(bool bVal);

			virtual float SynchUpdateInterval();
			virtual void SynchUpdateInterval(float fltVal);

			virtual float SynchUpdateStartInterval();
			virtual void SynchUpdateStartInterval(float fltVal);

			virtual float InitIODisableDuration();
			virtual void InitIODisableDuration(float fltVal);

			virtual void Initialize();
			virtual void TimeStepModified();
			virtual void ResetSimulation();
			virtual void AddExternalNodeInput(int iTargetDataType, float fltInput);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
			virtual float *GetDataPointer(const std::string &strDataType);
			virtual void StepSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}			//Adapters
}				//AnimatSim
