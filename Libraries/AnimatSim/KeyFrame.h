/**
\file	KeyFrame.h

\brief	Declares the key frame class.
**/

#pragma once

namespace AnimatSim
{
	namespace Recording
	{

		class ANIMAT_PORT KeyFrame : public ActivatedItem  
		{
		protected:
			/// Full pathname of the string project file
			std::string m_strProjectPath;

			/// Filename of the string configuration file
			std::string m_strConfigFilename;

			/// Zero-based index of the collect interval
			short m_iCollectInterval;

		public:
			KeyFrame();
			virtual ~KeyFrame();
						
			static KeyFrame *CastToDerived(AnimatBase *lpBase) {return static_cast<KeyFrame*>(lpBase);}

			int CollectInterval();
			void CollectInterval(int iVal);

			virtual void GenerateID();

			virtual void Load(CStdXml &oXml);

			virtual bool operator<(ActivatedItem *lpItem);

			/**
			\brief	Enables the video playback.
			
			\author	dcofer
			\date	3/24/2011
			**/
			virtual void EnableVideoPlayback() = 0;

			/**
			\brief	Disables the video playback.
			
			\author	dcofer
			\date	3/24/2011
			**/
			virtual void DisableVideoPlayback() = 0;

			/**
			\brief	Starts a video playback.
			
			\author	dcofer
			\date	3/24/2011
			**/
			virtual void StartVideoPlayback() = 0;

			/**
			\brief	Stop video playback.
			
			\author	dcofer
			\date	3/24/2011
			**/
			virtual void StopVideoPlayback() = 0;

			/**
			\brief	Playback video frame.
			
			\author	dcofer
			\date	3/24/2011
			**/
			virtual void PlaybackVideoFrame() = 0;

			/**
			\brief	Step video playback.
			
			\author	dcofer
			\date	3/24/2011
			
			\param	iFrameCount	Number of frames. 
			**/
			virtual void StepVideoPlayback(int iFrameCount) = 0;

			/**
			\brief	Record video frame.
			
			\author	dcofer
			\date	3/24/2011
			**/
			virtual void RecordVideoFrame() = 0;

			/**
			\brief	Saves a video.
			
			\author	dcofer
			\date	3/24/2011
			
			\param	strPath	Full pathname of the string file. 
			**/
			virtual void SaveVideo(std::string strPath) = 0;

			/**
			\brief	Makes the current frame.
			
			\author	dcofer
			\date	3/24/2011
			**/
			virtual void MakeCurrentFrame() = 0;
		};

	}			//Recording
}				//AnimatSim
