// VsVideoKeyFrame.h: interface for the VsVideoKeyFrame class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSVIDEOKEYFRAME_H__76C24FAB_0AFC_4822_BCA0_D3055E824E6E__INCLUDED_)
#define AFX_VSVIDEOKEYFRAME_H__76C24FAB_0AFC_4822_BCA0_D3055E824E6E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatLibrary
{
	namespace Recording
	{
		namespace KeyFrames
		{

			class VORTEX_PORT VsVideoKeyFrame : public KeyFrame  
			{
			protected:
				MdtRecorder *m_lpRecorder;
				KeyFrame *m_lpSimStopPoint;
				BOOL m_bSingleStepMode;
				CStdArray<long> m_aryFrameTimeSlices;

			public:
				VsVideoKeyFrame();
				virtual ~VsVideoKeyFrame();

				//ActiveItem overrides
				virtual string Type() {return "VIDEO";};

				virtual void Initialize(Simulator *lpSim);
				virtual void Activate(Simulator *lpSim);
				virtual void StepSimulation(Simulator *lpSim);
				virtual void Deactivate(Simulator *lpSim);

				virtual void EnableVideoPlayback(Simulator *lpSim);
				virtual void DisableVideoPlayback(Simulator *lpSim);
				virtual void StartVideoPlayback(Simulator *lpSim);
				virtual void StopVideoPlayback(Simulator *lpSim);
				virtual void PlaybackVideoFrame(Simulator *lpSim);
				virtual void StepVideoPlayback(Simulator *lpSim, int iFrameCount);
				virtual void RecordVideoFrame(Simulator *lpSim);
				virtual void SaveVideo(Simulator *lpSim, string strPath);
				virtual void MakeCurrentFrame(Simulator *lpSim);
			};

		}			//KeyFrames
	}				//Recording
}					//VortexAnimatLibrary

#endif // !defined(AFX_VSVIDEOKEYFRAME_H__76C24FAB_0AFC_4822_BCA0_D3055E824E6E__INCLUDED_)
