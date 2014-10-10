/**
\file	CaActivation.h

\brief	Declares the ca activation class.
**/

#pragma once

class IntegrateFireNeuralModule;

namespace IntegrateFireSim
{
	/**
	\brief	Calcium activation\inactivation. 

	\details This class implements the calcium activation/inactivation functions
	
	\author	dcofer
	\date	3/31/2011
	**/
	class ADV_NEURAL_PORT CaActivation : public AnimatSim::AnimatBase
	{
	protected:
		/// Pointer to the parent Neuron
		Neuron *m_lpParent;

		/// Type of the activation
		std::string m_strActivationType;

	public:

		/**
		\brief	Constructor.

		\author	dcofer
		\date	3/31/2011

		\param [in,out]	lpParent 	Pointer to the parent Neuron. 
		\param	strActivationType	Type of the  activation. 
		**/
		CaActivation(Neuron *lpParent, std::string strActivationType);
		virtual ~CaActivation();

		virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
		virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
		virtual void Load(CStdXml &oXml);
	};

}				//IntegrateFireSim

