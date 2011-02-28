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

		class ANIMAT_PORT SimulationRecorder : public ActivatedItemMgr  
		{
		protected:
			CStdArray<KeyFrame *> m_aryVideoFrames;
			CStdArray<KeyFrame *> m_arySingleFrames;

			KeyFrame *LoadKeyFrame(Simulator *lpSim, CStdXml &oXml);

		public:
			SimulationRecorder();
			virtual ~SimulationRecorder();

			virtual void Add(Simulator *lpSim, ActivatedItem *lpItem);
			virtual KeyFrame *Add(Simulator *lpSim, string strType, long lStart, long lEnd);

			virtual void Load(Simulator *lpSim, string strProjectPath, string strFileName);
			virtual void Load(Simulator *lpSim, CStdXml &oXml);
		};

	}			//Recording
}				//AnimatSim
