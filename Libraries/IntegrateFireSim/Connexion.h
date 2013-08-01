/**
\file	Connexion.h

\brief	Declares the connexion class.
**/

#pragma once

namespace IntegrateFireSim
{
	/**
	\namespace	IntegrateFireSim::Synapses

	\brief	Synapse classes for the integrate and fire neural model. 
	**/
	namespace Synapses
	{
		/**
		\brief	Synaptic Connexion.

		\details This is a synaptic connection between two neurons. It uses the SynapseType to determine how it behaves.
		(non-spiking chemical, spiking chemical, or eletrical). It has a base conductance value that is varied depending on the type of
		synapse. For example, a spiking chemical synapse increases the conductance to the maximum value when a spike occurs, and then
		exponentially declines to zero.
		
		\author	dcofer
		\date	3/31/2011
		**/
		class ADV_NEURAL_PORT Connexion : public AnimatSim::Link  
		{
		public:
			Connexion();
			Connexion(int type, int id, double d,float topBlock=75,float botBlock=25);
			virtual ~Connexion();
			virtual void Load(CStdXml &oXml);

			virtual void ResetSimulation();
			virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
			virtual float *GetDataPointer(const string &strDataType);
			
			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure,  AnimatSim::Behavior::NeuralModule *lpModule, Node *lpNode, bool bVerify);
			virtual void VerifySystemPointers();

#pragma region Accessor-Mutators

			void BaseConductance(double dVal);
			double BaseConductance();

			void Delay(double dVal);
			double Delay();

			string SynapseTypeID();
			void SynapseTypeID(string strID);

			string SourceID();
			void SourceID(string strID);

			string TargetID();
			void TargetID(string strID);

			void ResetIDs();

#pragma endregion

		protected:
			/// The pointer to the parent IntegrateFireNeuralModule.
			IntegrateFireNeuralModule *m_lpIGFModule;

		// LOADABLE
			/// GUID ID for the source Neuron.
			string m_strSourceID;

			/// GUID ID for the target Neuron.
			string m_strTargetID;

			/// GUID ID for the synapse type to use.
			string m_strSynapseTypeID;

			/// Pointer to the synapse type for this connection.
			SynapseType *m_lpSynType;  

			/// Pointer to the source Neuron.
			Neuron *m_lpSource;

			/// Pointer to the target Neuron.
			Neuron *m_lpTarget;

			/// Zero-based index of the source neuron.
			int m_iSource;

			/// Zero-based index of the target neuron.
			int m_iTarget;

			/// Zero-based index of the synaspe type.
			int m_iType;

			/// Identifier ID for this connection.
			int m_iID;

			/// The synaptic delay.
			double m_dDelay;

			/// facilitated increase in g when input occurs
			double m_dGFacilCx;		

			/// reported variable for facilitated increase in g when input occurs
			float m_fltGFailCxReport;

			/// The synaptic conductance.
			double m_dG;

			/// The reported variable for synaptic conductance.
			float m_fltGReport;

			/// Sets whether there is a partial block hold
			//double m_dPartialBlockHold;

		// WORKING
		// stuff for spiking chemical synapses
			/// Delay line for synaptic delays.
			DoubleList m_TransitCx;

			double FacilD();
			double RelFacil();
			bool VoltDep();
			bool Hebbian();
			double HebbIncrement();
			double HebbTimeWindow();
			double MaxGHebb();
			bool AllowForgetting();
			double ForgettingWindow();
			double Consolidation();
			double MaxGVoltDepRel();
 
			/// The base conductance.
			double m_dBaseG;			// standard baseline conductance
			
			/// List of hebbian events.
			DoubleList m_HebbList;

			/// The time since previous hebb event
			double m_dTimeSincePrevHebbEvent;

			/// The partial block
			//double m_dPartialBlock;

			/// The previous spike latency
			double m_dPreviousSpikeLatency;

			/**
			\brief	Appends the transit spike.
			
			\author	dcofer
			\date	3/31/2011
			**/
			void AppendTransitSpike() {m_TransitCx.AddTail(m_dDelay);}

			void DecrementLatencies(double dt,bool FreezeLearning=false);
			void DecrementFacilitation();

			/**
			\brief	Gets the time to next spike pointer.
			
			\author	dcofer
			\date	3/31/2011
			
			\return	null if it fails, else the time to next spike pointer.
			**/
			double *GetTimeToNextSpikePtr() {return m_TransitCx.First();}
			double ProcessOutput(bool bFreezeHebb=false);
			double GetProspectiveCond(bool bFreezeHebb);
			void IncrementHebbian();


		friend class IntegrateFireSim::IntegrateFireNeuralModule;
		};

	}			//Synapses
}				//IntegrateFireSim
