// AlSimulationRecorder.h: interface for the SimulationRecorder class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALSIMULATIONRECORDER_H__FE7FF2D1_7842_4EB2_943D_DEF45D430958__INCLUDED_)
#define AFX_ALSIMULATIONRECORDER_H__FE7FF2D1_7842_4EB2_943D_DEF45D430958__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatSim
{
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

#endif // !defined(AFX_ALSIMULATIONRECORDER_H__FE7FF2D1_7842_4EB2_943D_DEF45D430958__INCLUDED_)
