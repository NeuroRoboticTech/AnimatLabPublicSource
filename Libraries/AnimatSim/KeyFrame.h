// AlKeyFrame.h: interface for the KeyFrame class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALKEYFRAME_H__D91DC66E_01F1_47FC_AB62_766BA63FCEF0__INCLUDED_)
#define AFX_ALKEYFRAME_H__D91DC66E_01F1_47FC_AB62_766BA63FCEF0__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatSim
{
	namespace Recording
	{

		class ANIMAT_PORT KeyFrame : public ActivatedItem  
		{
		protected:
			string m_strProjectPath;
			string m_strConfigFilename;
			short m_iCollectInterval;

		public:
			KeyFrame();
			virtual ~KeyFrame();

			int CollectInterval() {return m_iCollectInterval;};
			void CollectInterval(int iVal) {m_iCollectInterval = iVal;};

			virtual void GenerateID();

			virtual void Load(CStdXml &oXml);

			//ActiveItem overrides
			virtual BOOL operator<(ActivatedItem *lpItem);

			virtual void EnableVideoPlayback() = 0;
			virtual void DisableVideoPlayback() = 0;
			virtual void StartVideoPlayback() = 0;
			virtual void StopVideoPlayback() = 0;
			virtual void PlaybackVideoFrame() = 0;
			virtual void StepVideoPlayback(int iFrameCount) = 0;
			virtual void RecordVideoFrame() = 0;
			virtual void SaveVideo(string strPath) = 0;
			virtual void MakeCurrentFrame() = 0;
		};

		BOOL LessThanActivatedItemCompare(ActivatedItem *lpItem1, ActivatedItem *lpItem2);

	}			//Recording
}				//AnimatSim

#endif // !defined(AFX_ALKEYFRAME_H__D91DC66E_01F1_47FC_AB62_766BA63FCEF0__INCLUDED_)
