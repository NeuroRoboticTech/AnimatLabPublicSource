// ClassFactory.h: interface for the ClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_NLCLASSFACTORY_H__73275845_57BE_400D_AE40_1E548C6419D4__INCLUDED_)
#define AFX_NLCLASSFACTORY_H__73275845_57BE_400D_AE40_1E548C6419D4__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace IntegrateFireSim
{

	class ClassFactory : public IStdClassFactory   
	{
	public:
		ClassFactory();
		virtual ~ClassFactory();

		virtual ExternalStimulus *CreateExternalStimulus(string strType, BOOL bThrowError = TRUE);
		virtual NeuralModule *CreateNeuralModule(string strType, BOOL bThrowError = TRUE);
		virtual DataColumn *CreateDataColumn(string strType, BOOL bThrowError = TRUE);
		virtual Gain *CreateGain(string strType, BOOL bThrowError = TRUE);
		virtual Neuron *CreateNeuron(string strType, BOOL bThrowError = TRUE);
		virtual Connexion *CreateSynapse(string strType, BOOL bThrowError = TRUE);
		virtual SynapseType *CreateSynapseType(string strType, BOOL bThrowError = TRUE);
		virtual IonChannel *CreateIonChannel(string strType, BOOL bThrowError = TRUE);

		virtual CStdSerialize *CreateObject(string strClassType, string strObjectType, BOOL bThrowError = TRUE);
	};

}				//IntegrateFireSim

#endif // !defined(AFX_NLCLASSFACTORY_H__73275845_57BE_400D_AE40_1E548C6419D4__INCLUDED_)
