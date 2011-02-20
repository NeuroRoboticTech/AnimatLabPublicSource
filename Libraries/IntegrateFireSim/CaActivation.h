// CaActivation.h: interface for the CaActivation class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

class IntegrateFireNeuralModule;

namespace IntegrateFireSim
{

	class ADV_NEURAL_PORT CaActivation : public AnimatSim::AnimatBase
	{
	protected:
		Neuron *m_lpParent;
		string m_strActivationType;

	public:
		CaActivation(Neuron *lpParent, string strActivationType);
		virtual ~CaActivation();

		virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
		virtual void Load(CStdXml &oXml);
	};

}				//IntegrateFireSim

