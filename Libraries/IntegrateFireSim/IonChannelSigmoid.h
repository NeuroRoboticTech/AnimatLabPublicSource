
#pragma once

namespace IntegrateFireSim
{

	/**
	\namespace	IntegrateFireSim::Gains

	\brief	Gain classes for the integrate and fire neural model. 
	**/
	namespace Gains
	{

		class ADV_NEURAL_PORT IonChannelSigmoid : public AnimatSim::Gains::Gain 
		{
		protected:
			float m_fltA;
			float m_fltB;
			float m_fltC;
			float m_fltD;
			float m_fltE;
			float m_fltF;
			float m_fltG;
			float m_fltH;

		public:
			IonChannelSigmoid();
			virtual ~IonChannelSigmoid();

			virtual float CalculateGain(float fltInput);
			virtual void Load(CStdXml &oXml);
		};
	}			//Gains
}				//IntegrateFireSim
