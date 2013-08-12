/**
\file	ClassFactory.h

\brief	Declares the class factory class.
**/

#pragma once

/**
\namespace	FiringRateSim

\brief	Contains the classes for a firing rate neural model. 
**/
namespace FiringRateSim
{
	/**
	\brief	Firing rate neural module class factory. 
	
	\author	dcofer
	\date	3/30/2011
	**/
	class ClassFactory : public IStdClassFactory   
	{
	public:
		ClassFactory();
		virtual ~ClassFactory();

		/**
		\brief	Creates a neuron.

		\author	dcofer
		\date	3/30/2011

		\param	strType	   	Type of the class to create. 
		\param	bThrowError	true to throw error if problem. 

		\return	Pointer to created object.
		**/
		virtual Neuron *CreateNeuron(string strType, bool bThrowError = true);

		/**
		\brief	Creates a synapse.

		\author	dcofer
		\date	3/30/2011

		\param	strType	   	Type of the class to create. 
		\param	bThrowError	true to throw error if problem. 

		\return	Pointer to created object.
		**/
		virtual Synapse *CreateSynapse(string strType, bool bThrowError = true);

		/**
		\brief	Creates a data column.

		\author	dcofer
		\date	3/30/2011

		\param	strType	   	Type of the class to create. 
		\param	bThrowError	true to throw error if problem. 

		\return	Pointer to created object.
		**/
		virtual DataColumn *CreateDataColumn(string strType, bool bThrowError = true);

		/**
		\brief	Creates an external stimulus.

		\author	dcofer
		\date	3/30/2011

		\param	strType	   	Type of the class to create. 
		\param	bThrowError	true to throw error if problem. 

		\return	Pointer to created object.
		**/
		virtual ExternalStimulus *CreateExternalStimulus(string strType, bool bThrowError = true);

		/**
		\brief	Creates a neural module.

		\author	dcofer
		\date	3/30/2011

		\param	strType	   	Type of the string. 
		\param	bThrowError	true to throw error. 

		\return	null if it fails, else.
		**/
		virtual NeuralModule *CreateNeuralModule(string strType, bool bThrowError = true);

		virtual CStdSerialize *CreateObject(string strClassType, string strObjectType, bool bThrowError = true);
	};

}				//FiringRateSim
