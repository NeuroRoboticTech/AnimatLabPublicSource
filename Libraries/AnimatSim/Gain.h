// Gain.h: interface for the Gain class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_GAIN_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
#define AFX_GAIN_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

/*! \brief 
   xxxx.

   \remarks
   xxxx
		 
   \sa
	 xxx
	 
	 \ingroup AnimatSim
*/

namespace AnimatSim
{
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

#endif // !defined(AFX_GAIN_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
