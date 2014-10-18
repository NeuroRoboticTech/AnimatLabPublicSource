/**
\file	Synapse.h

\brief	Declares the synapse class.
**/

#pragma once

namespace AnimatCarlSim
{

	/**
	\brief	Firing rate synapse model.

	\details This synapse type has a weight that is a current value. It injects a portion of that weight
	into the post-synaptic neuron based on the pre-synaptic neurons firing rate. I = W*F. (Where W is the
	weight, F is the firing rate, and I is the current.)
		
	\author	dcofer
	\date	3/29/2011
	**/
	class ANIMAT_CARL_SIM_PORT CsSynapseIndividual : public CsSynapseGroup   
	{
	protected:
		///The Ith neuron of the pre-synaptic group to connect.
		int m_iFromIdx;

		///The Jth neuron of the post-synaptic group to connect.
		int m_iToIdx;

		///The synapse index key used in the synapse map.
		//CStdIPoint m_iSynpaseKey;
		std::pair<int, int> m_vSynapseKey;

	public:
		CsSynapseIndividual();
		virtual ~CsSynapseIndividual();
		
		virtual void FromIdx(int iVal);
		virtual int FromIdx();
		
		virtual void ToIdx(int iVal);
		virtual int ToIdx();

		virtual std::pair<int, int> SynapseIndexKey();

		virtual void SetCARLSimulation();
		virtual bool SetCARLSimulation(int iFromIdx, int iToIdx, float& weight, float& maxWt, float& delay, bool& connected);

#pragma region DataAccesMethods
		virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
		virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
#pragma endregion

		virtual void Load(CStdXml &oXml);

	};

}				//AnimatCarlSim
