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
		string m_strActivationType;

	public:

		/**
		\brief	Constructor.

		\author	dcofer
		\date	3/31/2011

		\param [in,out]	lpParent 	Pointer to the parent Neuron. 
		\param	strActivationType	Type of the  activation. 
		**/
		CaActivation(Neuron *lpParent, string strActivationType);
		virtual ~CaActivation();

		virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
		virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
		virtual void Load(CStdXml &oXml);
	};

}				//IntegrateFireSim

