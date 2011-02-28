// SigmoidGain.h: interface for the SigmoidGain class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_SIGMOID_GAIN_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
#define AFX_SIGMOID_GAIN_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace AnimatSim
{
	namespace Gains
	{

			class ANIMAT_PORT SigmoidGain : public Gain 
			{
			protected:
				float m_fltA;
				float m_fltB;
				float m_fltC;
				float m_fltD;

			public:
				SigmoidGain();
				virtual ~SigmoidGain();

				virtual float CalculateGain(float fltInput);
				virtual void Load(CStdXml &oXml);
			};

	}			//Gains
}				//AnimatSim

#endif // !defined(AFX_SIGMOID_GAIN_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
