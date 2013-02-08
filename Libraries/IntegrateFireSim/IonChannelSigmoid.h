/**
\file	IonChannelSigmoid.h

\brief	Declares the ion channel sigmoid class.
**/

#pragma once

namespace IntegrateFireSim
{

	/**
	\namespace	IntegrateFireSim::Gains

	\brief	Gain classes for the integrate and fire neural model. 
	**/
	namespace Gains
	{
		/**
		\brief	Ion channel sigmoid gain. 

		\details This gain implements the following function: (A + (B/(H + exp(C*(X+D)) + E*exp(F*(X+G)) ))), where X is the input.
		
		\author	dcofer
		\date	3/31/2011
		**/
		class ADV_NEURAL_PORT IonChannelSigmoid : public AnimatSim::Gains::Gain 
		{
		protected:
			/// The A parameter
			float m_fltA;

			/// The B parameter
			float m_fltB;

			/// The C parameter
			float m_fltC;

			/// The D parameter
			float m_fltD;

			/// The E parameter
			float m_fltE;

			/// The F parameter
			float m_fltF;

			/// The G parameter
			float m_fltG;

			/// The H parameter
			float m_fltH;

		public:
			IonChannelSigmoid();
			virtual ~IonChannelSigmoid();

			virtual float CalculateGain(float fltInput);
			virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);
			virtual void Load(CStdXml &oXml);
		};
	}			//Gains
}				//IntegrateFireSim
