/**
\file	C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatSim\DelayLine.h

\brief	Declares the delay line class.
**/

#pragma once

namespace AnimatSim
{
		/**
		\brief	Implements a Delay line of float values.

		\details This is useful in things like synaptic delays. We need to specify the size of the 
		delay, which specifies the size of the buffer, and then we can add float values to one end.
		At each time step it moves down the pipe, until it finally falls out of the pipe at the end
		of the delay.
		
		\author	dcofer
		\date	3/24/2011
		**/
		class ANIMAT_PORT DelayLine
		{
		protected:
			/// Ring buffer of float values.
			CStdArray<float> m_aryRingBuf;

			///Size of the ring buffer
			int m_iDelaySize; 

			///Size of the ring buffer-1
			int m_iDelayComp; 

			///The index where the next new value will be saved in the ring
			int m_iSaveIdx;  

			///The index where we read the current top value off the ring.
			int m_iReadIdx;  

		public:
			DelayLine();
			virtual ~DelayLine();

			void Initialize(float fltDelayTime, float fltTimeStep);  
			void AddValue(float fltVal);
			float ReadValue();
		};


}				//AnimatSim
