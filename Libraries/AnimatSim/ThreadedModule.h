/**
\file	RobotIOControl.h

\brief	Declares the Robot IO control interface base class.
**/

#pragma once

namespace AnimatSim
{
	namespace Behavior
	{
		/**
		\brief	A Threaded neural module base class.
			
		\details To do. 

		\author	dcofer
		\date	10/1/2014
		**/
		class ANIMAT_PORT ThreadedModule : public NeuralModule
		{
		protected:
			///True while the io thread processing loop is going on.
			bool m_bThreadProcessing;

			///Set to true once the thread begins its setup.
			bool m_bSetupStarted;

			///Set to true once the thread is setup correctly.
			bool m_bSetupComplete;

			///Flags the thread processing loop to exit.
			bool m_bStopThread;

			///Set to true to pause the thread processing. Set back to false to resume it.
			bool m_bPauseThread;
			
			///Is set to true once the thread loop is paused.
			bool m_bThreadPaused;

			///The time it takes to perform a step of the IO for all parts in this control.
			float m_fltStepThreadDuration;

			//Temporary if def to prevent this from showing up in ManagedAnimatTools. Will get rid of this
			//once I get rid of that library.
	#ifndef STD_DO_NOT_ADD_BOOST
			/// Thread responsible for doing IO processing.
			boost::thread m_Thread;

			/// Mutex responsible for waiting until the IO is finished setting up.
			boost::interprocess::interprocess_mutex m_WaitForSetupMutex;

			/// Condition used to determine when the IO is setup.
			boost::interprocess::interprocess_condition  m_WaitForSetupCond;
	#endif

			///Used to signal to the IO thread that we are waiting for their return signal.
			bool m_bWaitingForThreadNotify;

			virtual void StartThread();
			virtual void ProcessThread();
			virtual void ExitThread();
			virtual void CloseThread();

			virtual void WaitWhilePaused();
			virtual void WaitTillPaused();
			virtual void WaitForThreadNotifyReady();
			virtual void StartPause();
			virtual void ExitPause();

		public:
			ThreadedModule(void);
			virtual ~ThreadedModule(void);
						
			static ThreadedModule *CastToDerived(AnimatBase *lpBase) {return static_cast<ThreadedModule*>(lpBase);}

			virtual void PauseThread(bool bVal);
			virtual bool PauseThread();
			virtual bool ThreadPaused();

			virtual float StepThreadDuration();

			virtual void SetupThread();
			virtual void StepThread();
			virtual void ShutdownThread();

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
		};
	}
}