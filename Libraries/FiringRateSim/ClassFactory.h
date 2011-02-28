#pragma once

/**
\namespace	FiringRateSim

\brief	Contains the classes for a firing rate neural model. 
**/
namespace FiringRateSim
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

}				//FiringRateSim
