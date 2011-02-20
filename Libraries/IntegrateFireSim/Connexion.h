// Connexion.h: interface for the Connexion class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_CONNEXION_H__C287771C_6873_4883_829A_CB726940617B__INCLUDED_)
#define AFX_CONNEXION_H__C287771C_6873_4883_829A_CB726940617B__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


namespace IntegrateFireSim
{
	namespace Synapses
	{

		class ADV_NEURAL_PORT Connexion : public AnimatBase  
		{
		public:
			Connexion();
			Connexion(int type, int id, double d,float topBlock=75,float botBlock=25);
			virtual ~Connexion();
			virtual void Load(CStdXml &oXml, IntegrateFireNeuralModule *lpNS);

			virtual void ResetSimulation(Simulator *lpSim, Structure *lpStruct);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

#pragma region Accessor-Mutators

		void BaseConductance(double dVal) 
		{
			//The mempot variables are calculated, so we do not want to just re-set them to the new value.
			//instead lets adjust them by the difference between the old and new resting potential.
			double dDiff = dVal - m_dBaseG;

			m_dBaseG = dVal;
			m_dG += dDiff;
			m_dGFacilCx += dDiff;
		};
		double BaseConductance() {return m_dBaseG;};

		void Delay(double dVal) {m_dDelay = dVal;};
		double Delay() {return m_dDelay;};

		string SynapseTypeID() {return m_strSynapseTypeID;};
		void SynapseTypeID(string strID) {m_strSynapseTypeID = strID;};

		string SourceID() {return m_strSourceID;};
		void SourceID(string strID) {m_strSourceID = strID;};

		string TargetID() {return m_strTargetID;};
		void TargetID(string strID) {m_strTargetID = strID;};

		void ResetIDs();

#pragma endregion

		protected:
			IntegrateFireNeuralModule *m_lpModule;

		// LOADABLE
			string m_strSourceID;
			string m_strTargetID;
			string m_strSynapseTypeID;

			SynapseType *m_lpSynType;  //Pointer to the synapse type for this connection.
			Neuron *m_lpSource;
			Neuron *m_lpTarget;

			int m_iSource;
			int m_iTarget;
			int m_iType;
			int m_iID;
			double m_dDelay;
			double m_dGFacilCx;		// facilitated increase in g when input occurs
			double m_dG;
			double m_dPartialBlockHold;

		// WORKING
		// stuff for spiking chemical synapses
			DoubleList m_TransitCx;

			double FacilD();
			double RelFacil();
			BOOL VoltDep();
			BOOL Hebbian();
			double HebbIncrement();
			double HebbTimeWindow();
			double MaxGHebb();
			BOOL AllowForgetting();
			double ForgettingWindow();
			double Consolidation();
			double MaxGVoltDepRel();
 
			double m_dBaseG;			// standard baseline conductance
			
			DoubleList m_HebbList;
			double m_dTimeSincePrevHebbEvent;
			double m_dPartialBlock;
			double m_dPreviousSpikeLatency;

			void AppendTransitSpike() {m_TransitCx.AddTail(m_dDelay);}
			void DecrementLatencies(double dt,BOOL FreezeLearning=FALSE);
			void DecrementFacilitation();
			double *GetTimeToNextSpikePtr() {return m_TransitCx.First();}
			double ProcessOutput(BOOL bFreezeHebb=FALSE);
			double GetProspectiveCond(BOOL bFreezeHebb);
			void IncrementHebbian();


		friend class IntegrateFireNeuralModule;
		};

	}			//Synapses
}				//IntegrateFireSim

#endif // !defined(AFX_CONNEXION_H__C287771C_6873_4883_829A_CB726940617B__INCLUDED_)
