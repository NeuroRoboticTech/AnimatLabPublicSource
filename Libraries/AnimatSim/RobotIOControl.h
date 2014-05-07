#pragma once

namespace AnimatSim
{
	namespace Robotics
	{

		class ANIMAT_PORT RobotIOControl : public AnimatBase
		{
		protected:
			RobotInterface *m_lpParentInterface;

			///A list of child parts that are connected to this part through
			///different joints. 
			CStdPtrArray<RobotPartInterface> m_aryParts;

			//True while the io thread processing loop is going on.
			bool m_bIOThreadProcessing;


			///Set to true once the IO is setup correctly.
			bool m_bSetupComplete;

			///Flags the thread processing loop to exit.
			bool m_bStopIO;
			
			//The time it takes to perform a step of the IO for all parts in this control.
			float m_fltStepIODuration;

			//Temporary if def to prevent this from showing up in ManagedAnimatTools. Will get rid of this
			//once I get rid of that library.
#ifndef STD_DO_NOT_ADD_BOOST
			/// Thread responsible for doing IO processing.
			boost::thread m_ioThread;

			boost::interprocess::interprocess_mutex m_WaitForIOSetupMutex;
			boost::interprocess::interprocess_condition  m_WaitForIOSetupCond;
#endif

			virtual RobotPartInterface *LoadPartInterface(CStdXml &oXml);
			virtual RobotPartInterface *AddPartInterface(std::string strXml);
			virtual void RemovePartInterface(std::string strID, bool bThrowError = true);
			virtual int FindChildListPos(std::string strID, bool bThrowError = true);

			virtual void StartIOThread();
			virtual void ProcessIO() = 0;
			virtual void ExitIOThread();

		public:
			RobotIOControl(void);
			virtual ~RobotIOControl(void);

			virtual void ParentInterface(RobotInterface *lpParent);
			virtual RobotInterface *ParentInterface();

			virtual CStdPtrArray<RobotPartInterface>* Parts();
			
			virtual float StepIODuration();

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
			virtual bool AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError = true, bool bDoNotInit = false);
			virtual bool RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError = true);

			virtual void SetupIO();
			virtual void StepIO();

			virtual void Initialize();
			virtual void SimStopping();
			virtual void StepSimulation();
			virtual void ResetSimulation();
			virtual void AfterResetSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}
}