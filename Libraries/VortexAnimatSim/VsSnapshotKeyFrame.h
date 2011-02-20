// VsSnapshotKeyFrame.h: interface for the VsSnapshotKeyFrame class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSSNAPSHOTKEYFRAME_H__76C24FAB_0AFC_4822_BCA0_D3055E824E6E__INCLUDED_)
#define AFX_VSSNAPSHOTKEYFRAME_H__76C24FAB_0AFC_4822_BCA0_D3055E824E6E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatLibrary
{
	namespace Recording
	{
		namespace KeyFrames
		{

			class VORTEX_PORT VsSnapshotKeyFrame : public KeyFrame 
			{
			protected:
				MdtRecorder *m_lpRecorder;
				BYTE *m_aryBytes;
				long m_lByteSize;

			public:
				VsSnapshotKeyFrame();
				virtual ~VsSnapshotKeyFrame();

				//ActiveItem overrides
				virtual string Type() {return "SNAPSHOT";};

				virtual void GenerateID(Simulator *lpSim);

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

				virtual void Load(Simulator *lpSim, CStdXml &oXml);
			};

		}			//KeyFrames
	}				//Recording
}					//VortexAnimatLibrary

#endif // !defined(AFX_VSSNAPSHOTKEYFRAME_H__76C24FAB_0AFC_4822_BCA0_D3055E824E6E__INCLUDED_)
