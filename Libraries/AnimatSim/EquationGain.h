// EquationGain.h: interface for the EquationGain class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_EQUATION_GAIN_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
#define AFX_EQUATION_GAIN_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_

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

			class ANIMAT_PORT EquationGain : public Gain 
			{
			protected:
				string m_strGainEquation;
				CStdPostFixEval *m_lpEval;

			public:
				EquationGain();
				virtual ~EquationGain();

				virtual float CalculateGain(float fltInput);
				virtual void Load(CStdXml &oXml);
			};

	}			//Gains
}				//AnimatSim

#endif // !defined(AFX_EQUATION_GAIN_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
