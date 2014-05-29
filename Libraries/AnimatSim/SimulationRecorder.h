/**
\file	SimulationRecorder.h

\brief	Declares the simulation recorder class.
**/

#pragma once

namespace AnimatSim
{

	/**
	\namespace	AnimatSim::Recording

	\brief	Contains classes related to recording the simulation parameters at a specific time frame so it can 
	be reset to that point, or replayed from that point. 
	**/
	namespace Recording
	{
		/**
		\brief	Records the simulation keyframes and videos. 
		
		\author	dcofer
		\date	3/24/2011
		**/
		class ANIMAT_PORT SimulationRecorder : public ActivatedItemMgr  
		{
		protected:
			/// Array of video keyframes.
			CStdArray<KeyFrame *> m_aryVideoFrames;

			/// Array of key frames.
			CStdArray<KeyFrame *> m_arySingleFrames;

			KeyFrame *LoadKeyFrame(CStdXml &oXml);

		public:
			SimulationRecorder();
			virtual ~SimulationRecorder();
						
			static SimulationRecorder *CastToDerived(AnimatBase *lpBase) {return static_cast<SimulationRecorder*>(lpBase);}

			virtual void Add(ActivatedItem *lpItem);
			virtual KeyFrame *Add(std::string strType, long lStart, long lEnd);

			virtual void Load(CStdXml &oXml);
		};

	}			//Recording
}				//AnimatSim
