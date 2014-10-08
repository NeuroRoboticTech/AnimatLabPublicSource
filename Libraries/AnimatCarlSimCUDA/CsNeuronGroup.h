/**
\file	CsNeuronGroup.h

\brief	Declares the CsNeuronGroup class.
**/

#pragma once

namespace AnimatCarlSim
{

	/**
	\brief	Firing Rate Neuron model.

	\details This Neuron implements a firing rate neural model. The firing rate model is a more abstract representation
	of the neuron than an integrate and fire system. This type of model assumes that there is a linear relationship between 
	cell depolarization and firing rate. After the neuron has depolarized beyond its threshold its firing rate increases linearly between
	0 and 1	relative to the membrane potential and a gain value. Synapses inject current into post-synaptic neurons based on the firing rate.
	The synaptic weight is the amount of current to inject, and this is multiplied by the firing rate of the pre-synaptic neuron. this model also 
	has modulatory and gated synapses.<br>
	Another feature of this model is that there are a few different types of neruons. These primarily differ based on how they implement intrinsic
	currents. Intrinsic currents are currents that are internal to the neuron. An exmample of this is the pacemaker neuron that generates currents
	internally to model bursting behavior.
		
	\author	dcofer
	\date	3/29/2011
	**/
	class ANIMAT_CARL_SIM_PORT CsNeuronGroup : public AnimatSim::Node, SpikeMonitor   
	{
	protected:
		/// Pointer to parent CsNeuralModule.
		CsNeuralModule *m_lpCsModule;

		///The number of neurons in this group
		unsigned int m_uiNeuronCount;

		//The type of these neurons. Either EXCITATORY_NEURON or INHIBITORY_NEURON
		int m_iNeuronType;

		///The group ID of this set of neurons
		int m_iGroupID;

		///The Izhikevich A parameter
		float m_fltA;

		///The standard deviation for Izhikevich parameter A
		float m_fltStdA;

		///The Izhikevich B parameter
		float m_fltB;

		///The standard deviation for Izhikevich parameter B
		float m_fltStdB;

		///The Izhikevich C parameter
		float m_fltC;

		///The standard deviation for Izhikevich parameter C
		float m_fltStdC;

		///The Izhikevich D parameter
		float m_fltD;

		///The standard deviation for Izhikevich parameter D
		float m_fltStdD;
		
		///Enables or disables conductance based neuron modelling.
		///If false the neurons in this group will use current based modelling.
		bool m_bEnableCOBA;

		///Time constant of AMPA decay (ms); for example, 5.0 
		float m_fltT_AMPA;

		///Time constant of NMDA decay (ms); for example, 150.0 
		float m_fltT_NMDA;

		///Time constant of GABAa decay (ms); for example, 6.0  
		float m_fltT_GABAa;

		///Time constant of GABAb decay (ms); for example, 150.0
		float m_fltT_GABAb;
					
	public:
		CsNeuronGroup();
		virtual ~CsNeuronGroup();

		virtual void SetCARLSimulation();

		virtual void NeuronCount(unsigned int iVal);
		virtual unsigned int NeuronCount();

		virtual void NeuronType(int iVal);
		virtual int NeuronType();

		virtual void GroupID(int iVal);
		virtual int GroupID();

		virtual void A(float fltVal);
		virtual float A();

		virtual void StdA(float fltVal);
		virtual float StdA();

		virtual void B(float fltVal);
		virtual float B();

		virtual void StdB(float fltVal);
		virtual float StdB();

		virtual void C(float fltVal);
		virtual float C();

		virtual void StdC(float fltVal);
		virtual float StdC();

		virtual void D(float fltVal);
		virtual float D();

		virtual void StdD(float fltVal);
		virtual float StdD();

		virtual void EnableCOBA(bool bVal);
		virtual bool EnableCOBA();

		virtual void T_AMPA(float fltVal);
		virtual float T_AMPA();

		virtual void T_NMDA(float fltVal);
		virtual float T_NMDA();

		virtual void T_GABAa(float fltVal);
		virtual float T_GABAa();

		virtual void T_GABAb(float fltVal);
		virtual float T_GABAb();

		virtual void update(CpuSNN* s, int grpId, unsigned int* NeuronIds, unsigned int *timeCounts);

		virtual void AddExternalNodeInput(int iTargetDataType, float fltInput);
		virtual void Copy(CStdSerialize *lpSource);

		virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify);
		virtual void VerifySystemPointers();
		virtual void Initialize();
		virtual void StepSimulation();

#pragma region DataAccesMethods
		virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
		virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
#pragma endregion

		virtual void Load(CStdXml &oXml);
	};

}				//AnimatCarlSim
