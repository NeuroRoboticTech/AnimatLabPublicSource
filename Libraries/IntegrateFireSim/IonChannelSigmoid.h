// IonChannelSigmoid.h: interface for the IonChannelSigmoid class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ION_CHANNEL_SIGMOID_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
#define AFX_ION_CHANNEL_SIGMOID_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_

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

namespace IntegrateFireSim
{
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

#endif // !defined(AFX_ION_CHANNEL_SIGMOID_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
