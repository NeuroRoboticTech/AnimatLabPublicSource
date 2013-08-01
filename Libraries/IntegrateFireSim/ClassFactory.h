/**
\file	ClassFactory.h

\brief	Declares the class factory class.
**/

#pragma once

namespace IntegrateFireSim
{
	/**
	\brief	Class factory for the integrate and fire neural module. 
	
	\author	dcofer
	\date	3/31/2011
	**/
	class ClassFactory : public IStdClassFactory   
	{
	public:
		ClassFactory();
		virtual ~ClassFactory();

		/**
		\brief	Creates an external stimulus.

		\author	dcofer
		\date	3/31/2011

		\param	strType	   	Type of class to create. 
		\param	bThrowError	true to throw error if there is a problem. 

		\return	Pointer to the created object.
		**/
		virtual ExternalStimulus *CreateExternalStimulus(string strType, bool bThrowError = true);

		/**
		\brief	Creates a neural module.

		\author	dcofer
		\date	3/31/2011

		\param	strType	   	Type of class to create. 
		\param	bThrowError	true to throw error if there is a problem. 

		\return	Pointer to the created object.
		**/
		virtual NeuralModule *CreateNeuralModule(string strType, bool bThrowError = true);

		/**
		\brief	Creates a data column.

		\author	dcofer
		\date	3/31/2011

		\param	strType	   	Type of class to create. 
		\param	bThrowError	true to throw error if there is a problem. 

		\return	Pointer to the created object.
		**/
		virtual DataColumn *CreateDataColumn(string strType, bool bThrowError = true);

		/**
		\brief	Creates a gain.

		\author	dcofer
		\date	3/31/2011

		\param	strType	   	Type of class to create. 
		\param	bThrowError	true to throw error if there is a problem. 

		\return	Pointer to the created object.
		**/
		virtual Gain *CreateGain(string strType, bool bThrowError = true);

		/**
		\brief	Creates a neuron.

		\author	dcofer
		\date	3/31/2011

		\param	strType	   	Type of class to create. 
		\param	bThrowError	true to throw error if there is a problem. 

		\return	Pointer to the created object.
		**/
		virtual Neuron *CreateNeuron(string strType, bool bThrowError = true);

		/**
		\brief	Creates a synapse.

		\author	dcofer
		\date	3/31/2011

		\param	strType	   	Type of class to create. 
		\param	bThrowError	true to throw error if there is a problem. 

		\return	Pointer to the created object.
		**/
		virtual Connexion *CreateSynapse(string strType, bool bThrowError = true);

		/**
		\brief	Creates a synapse type.

		\author	dcofer
		\date	3/31/2011

		\param	strType	   	Type of class to create. 
		\param	bThrowError	true to throw error if there is a problem. 

		\return	Pointer to the created object.
		**/
		virtual SynapseType *CreateSynapseType(string strType, bool bThrowError = true);

		/**
		\brief	Creates an ion channel.

		\author	dcofer
		\date	3/31/2011

		\param	strType	   	Type of class to create. 
		\param	bThrowError	true to throw error if there is a problem. 

		\return	Pointer to the created object.
		**/
		virtual IonChannel *CreateIonChannel(string strType, bool bThrowError = true);

		virtual CStdSerialize *CreateObject(string strClassType, string strObjectType, bool bThrowError = true);
	};

}				//IntegrateFireSim
