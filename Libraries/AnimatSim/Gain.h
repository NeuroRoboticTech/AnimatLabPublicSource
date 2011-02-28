
#pragma once

namespace AnimatSim
{

	/**
	\namespace	AnimatSim::Gains

	\brief	Contains the different gain type classes that can be used. 

	\details A gain is essentially an equation to convert one parameter into a different value. It is 
	used extensively throughout the system, for example in the adapters.
	**/
	namespace Gains
	{

		class ANIMAT_PORT Gain : public AnimatBase 
		{
		protected:
			BOOL m_bUseLimits;
			float m_fltLowerLimit;
			float m_fltLowerOutput;
			float m_fltUpperLimit;
			float m_fltUpperOutput;

			BOOL InLimits(float fltInput)
			{
				if( m_bUseLimits && ( (fltInput < m_fltLowerLimit) || (fltInput > m_fltUpperLimit) ) ) 
					return FALSE;
				else
					return TRUE;
			}

			float CalculateLimitOutput(float fltInput)
			{
				if(fltInput < m_fltLowerLimit)
					return m_fltLowerOutput;

				if(fltInput > m_fltUpperLimit)
					return m_fltUpperOutput;

				return 0;
			}

		public:
			Gain();
			virtual ~Gain();

			BOOL UseLimits() {return m_bUseLimits;};
			void UseLimits(BOOL bVal) {m_bUseLimits = bVal;};

			float LowerLimit() {return m_fltLowerLimit;};
			void LowerLimit(float fltVal) {m_fltLowerLimit = fltVal;};

			float UpperLimit() {return m_fltUpperLimit;};
			void UpperLimit(float fltVal) {m_fltUpperLimit = fltVal;};

			float LowerOutput() {return m_fltLowerOutput;};
			void LowerOutput(float fltVal) {m_fltLowerOutput = fltVal;};

			float UpperOutput() {return m_fltUpperOutput;};
			void UpperOutput(float fltVal) {m_fltUpperOutput = fltVal;};

			virtual float CalculateGain(float fltInput) = 0;
			virtual void Load(CStdXml &oXml);
		};

		Gain ANIMAT_PORT *LoadGain(string strName, CStdXml &oXml);

	}			//Gains
}				//AnimatSim
