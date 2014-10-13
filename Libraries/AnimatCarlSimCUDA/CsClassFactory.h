/**
\file	ClassFactory.h

\brief	Declares the class factory class.
**/

#pragma once

/**
\namespace	AnimatCarlSim

\brief	Contains the classes for a firing rate neural model. 
**/
namespace AnimatCarlSim
{
	/**
	\brief	Firing rate neural module class factory. 
	
	\author	dcofer
	\date	3/30/2011
	**/
	class CsClassFactory : public IStdClassFactory   
	{
	public:
		CsClassFactory();
		virtual ~CsClassFactory();

		/**
		\brief	Creates a neuron.

		\author	dcofer
		\date	3/30/2011

		\param	strType	   	Type of the class to create. 
		\param	bThrowError	true to throw error if problem. 

		\return	Pointer to created object.
		**/
		virtual Node *CreateNeuron(std::string strType, bool bThrowError = true);

		/**
		\brief	Creates a synapse.

		\author	dcofer
		\date	3/30/2011

		\param	strType	   	Type of the class to create. 
		\param	bThrowError	true to throw error if problem. 

		\return	Pointer to created object.
		**/
		virtual CsSynapseGroup *CreateSynapse(std::string strType, bool bThrowError = true);

		/**
		\brief	Creates an external stimuli.

		\author	dcofer
		\date	3/30/2011

		\param	strType	   	Type of the class to create. 
		\param	bThrowError	true to throw error if problem. 

		\return	Pointer to created object.
		**/
		virtual ExternalStimulus *CreateExternalStimulus(std::string strType, bool bThrowError = true);


		/**
		\brief	Creates a neural module.

		\author	dcofer
		\date	3/30/2011

		\param	strType	   	Type of the std::string. 
		\param	bThrowError	true to throw error. 

		\return	null if it fails, else.
		**/
		virtual NeuralModule *CreateNeuralModule(std::string strType, bool bThrowError = true);

		virtual DataColumn *CreateDataColumn(std::string strType, bool bThrowError = true);
		virtual Adapter *CreateAdapter(std::string strType, bool bThrowError = true);

		virtual CStdSerialize *CreateObject(std::string strClassType, std::string strObjectType, bool bThrowError = true);
	};

}				//FiringRateSim
