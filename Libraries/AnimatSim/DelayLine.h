// DelayLine.h: interface for the DelayLine class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_DELAY_LINE_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
#define AFX_DELAY_LINE_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_

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
		class ANIMAT_PORT DelayLine
		{
		protected:
			CStdArray<float> m_aryRingBuf;

			int m_iDelaySize; //Size of the ring buffer
			int m_iDelayComp; //Size of the ring buffer-1
			int m_iSaveIdx;  //The index where the next new value will be saved in the ring
			int m_iReadIdx;  //The index where we read the current top value off the ring.

		public:
			DelayLine();
			virtual ~DelayLine();

			void Initialize(float fltDelayTime, float fltTimeStep);  
			void AddValue(float fltVal);
			float ReadValue();
		};


}				//AnimatSim

#endif // !defined(AFX_DELAY_LINE_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
