/**
\file	RobotIOControl.h

\brief	Declares the Robot IO control interface base class.
**/

#pragma once

namespace AnimatSim
{
	namespace Robotics
	{

		/**
		\brief	A Robot IO controller base class.
			
		\details Robot IO controllers are responsible for setting up and maintaining the communications channels
		used by a specific IO control system. A lot of the basic functionality to do this is included in this base
		class. You will need to derive a new class for your specific IO system and then override a few key methods 
		like OpenIO, SetupIO, StepIO, CloseIO, and StepSimulation. Each Robot IO controller has a separate IO thread
		that it starts during the initialization phase. IO processing is performed within that thread and when the 
		simulation steps it gets data that was read in during the IO thread, and sets data that the IO will send out.
		This means that the IO update process and the simulation step method are not going to be synchronized. One
		will be moving faster than the other. So you need to be careful of how you are transfering data back and forth
		between these two threads. 

		Each Robot IO controller contains a list of RobotPartInterface objects. Each robot part interface uses the 
		communications channel that was opened by the IO controller to either directly talk to the real hardware, or
		it uses the IO controller to do that indirectly. 

		\author	dcofer
		\date	8/9/2014
		**/
		class ANIMAT_PORT RobotIOControl : public AnimatBase
		{
		protected:
			///Pointer to the parent robot interface
			RobotInterface *m_lpParentInterface;

			///A list of child parts that are connected to this part through
			///different joints. 
			CStdPtrArray<RobotPartInterface> m_aryParts;

			///True while the io thread processing loop is going on.
			bool m_bIOThreadProcessing;

			///Set to true once the IO begins its setup.
			bool m_bSetupStarted;

			///Set to true once the IO is setup correctly.
			bool m_bSetupComplete;

			///Flags the thread processing loop to exit.
			bool m_bStopIO;

			///Set to true to pause the IO processing. Set back to false to resume it.
			bool m_bPauseIO;
			
			///Is set to true once the IO loop is paused.
			bool m_bIOPaused;

			///The time it takes to perform a step of the IO for all parts in this control.
			float m_fltStepIODuration;

			///The total number of parts that are part of any round robin cycle of updates.
			int m_iCyclePartCount;

			///The index of the part that should be processed on the current step.
			int m_iCyclePartIdx;

			//Temporary if def to prevent this from showing up in ManagedAnimatTools. Will get rid of this
			//once I get rid of that library.
#ifndef STD_DO_NOT_ADD_BOOST
			/// Thread responsible for doing IO processing.
			boost::thread m_ioThread;

			/// Mutex responsible for waiting until the IO is finished setting up.
			boost::interprocess::interprocess_mutex m_WaitForIOSetupMutex;

			/// Condition used to determine when the IO is setup.
			boost::interprocess::interprocess_condition  m_WaitForIOSetupCond;
#endif

			///Used to signal to the IO thread that we are waiting for their return signal.
			bool m_bWaitingForThreadNotify;

			virtual RobotPartInterface *LoadPartInterface(CStdXml &oXml);
			virtual RobotPartInterface *AddPartInterface(std::string strXml);
			virtual void RemovePartInterface(std::string strID, bool bThrowError = true);
			virtual int FindChildListPos(std::string strID, bool bThrowError = true);

			virtual void StartIOThread();
			virtual void ProcessIO();
			virtual void ExitIOThread();
			virtual bool OpenIO() = 0;
			virtual void CloseIO() = 0;

			virtual void WaitWhilePaused();
			virtual void WaitTillPaused();
			virtual void WaitForThreadNotifyReady();
			virtual void StartPause();
			virtual void ExitPause();

		public:
			RobotIOControl(void);
			virtual ~RobotIOControl(void);
						
			static RobotIOControl *CastToDerived(AnimatBase *lpBase) {return static_cast<RobotIOControl*>(lpBase);}

			virtual void ParentInterface(RobotInterface *lpParent);
			virtual RobotInterface *ParentInterface();

			virtual CStdPtrArray<RobotPartInterface>* Parts();
			
			virtual void PauseIO(bool bVal);
			virtual bool PauseIO();
			virtual bool IOPaused();

			virtual float StepIODuration();

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
			virtual bool AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError = true, bool bDoNotInit = false);
			virtual bool RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError = true);

			virtual void SetupIO();
			virtual void StepIO();
			virtual void ShutdownIO();

			virtual void Initialize();
			virtual void SimStopping();
			virtual void StepSimulation();
			virtual void ResetSimulation();
			virtual void AfterResetSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}
}