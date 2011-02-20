// NlClassFactory.h: interface for the ClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace [*PROJECT_NAME*]
{

	class ClassFactory : public IStdClassFactory   
	{
	public:
		ClassFactory();
		virtual ~ClassFactory();

		virtual Neuron *CreateNeuron(string strType, BOOL bThrowError = TRUE);
		virtual Synapse *CreateSynapse(string strType, BOOL bThrowError = TRUE);
		virtual DataColumn *CreateDataColumn(string strType, BOOL bThrowError = TRUE);
		virtual ExternalStimulus *CreateExternalStimulus(string strType, BOOL bThrowError = TRUE);
		virtual NeuralModule *CreateNeuralModule(string strType, BOOL bThrowError = TRUE);

		virtual CStdSerialize *CreateObject(string strClassType, string strObjectType, BOOL bThrowError = TRUE);
	};

}				//[*PROJECT_NAME*]

