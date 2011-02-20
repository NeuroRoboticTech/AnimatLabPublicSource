// BellGain.h: interface for the BellGain class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_BELLGAIN_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
#define AFX_BELLGAIN_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_

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

			class ANIMAT_PORT BellGain : public Gain 
			{
			protected:
				float m_fltA;
				float m_fltB;
				float m_fltC;
				float m_fltD;

			public:
				BellGain();
				virtual ~BellGain();

				float A() {return m_fltA;}
				void A(float fltVal) {m_fltA = fltVal;};

				float B() {return m_fltB;}
				void B(float fltVal) {m_fltB = fltVal;};

				float C() {return m_fltC;}
				void C(float fltVal) {m_fltC = fltVal;};

				float D() {return m_fltD;}
				void D(float fltVal) {m_fltD = fltVal;};

				virtual float CalculateGain(float fltInput);
				virtual void Load(CStdXml &oXml);
			};

	}			//Gains
}				//AnimatSim

#endif // !defined(AFX_BELLGAIN_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
