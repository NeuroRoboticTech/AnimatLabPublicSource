#pragma once

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Diagnostics;
using namespace System::Runtime::InteropServices;
#using <mscorlib.dll>
 
namespace AnimatGUI
{
	namespace Interfaces
	{
 
		/// <summary> 
		/// Summary for SimulatorInterface
		/// </summary>
		public ref class SimulatorInterfaceMock : public ManagedAnimatInterfaces::ISimulatorInterface
		{
		public:
			SimulatorInterfaceMock(void);
			~SimulatorInterfaceMock();

#pragma region EventSystems

			virtual event CreateSimHandler^ OnSimulationCreate;
			virtual event VoidHandler^ SimulationRunning;
			virtual event VoidHandler^ NeedToStopSimulation;
			virtual event ErrorHandler^ HandleNonCriticalError;
			virtual event ErrorHandler^ HandleCriticalError;

			virtual void FireNeedToStopSimulationEvent();
			virtual void FireHandleNonCriticalErrorEvent(System::String ^strError);
			virtual void FireHandleCriticalErrorEvent(System::String ^strError);

#pragma endregion

#pragma region Properties

			virtual Simulator *Sim();
			virtual long CurrentMillisecond();
			virtual bool Paused();
			virtual bool SimRunning();
			virtual System::Boolean Loaded();
			virtual System::Boolean SimOpen();
			virtual void SetProjectPath(System::String ^strPath); 
			virtual void SetLogger(ManagedAnimatInterfaces::ILogger ^lpLog);

#pragma endregion

#pragma region SimulationControl

			virtual bool AddWindow(IntPtr hParentWnd, System::String ^sWindowXml);
			virtual void RemoveWindow(IntPtr hParentWnd);
			virtual void OnWindowGetFocus(System::String ^sID);
			virtual void OnWindowLoseFocus(System::String ^sID);

			virtual void CreateAndRunSimulation(System::Boolean bPaused);
			virtual void CreateSimulation();
			virtual void CreateSimulation(System::String ^sXml);
			virtual void Simulate(bool bPaused);
			virtual void ShutdownSimulation();
			virtual bool StartSimulation();
			virtual bool PauseSimulation();
			virtual void StopSimulation();

			virtual System::String ^ErrorMessage();

			virtual void SaveSimulationFile(String ^sFile);
			virtual void TrackCamera(System::Boolean bTrackCamera, String ^sLookAtStructureID, String ^sLookAtBodyID);

#pragma endregion

#pragma region VideoPlayback

			virtual String ^AddKeyFrame(String ^strType, long lStartMillisecond, long lEndMillisecond);
			virtual void RemoveKeyFrame(String ^strID);
			virtual String ^MoveKeyFrame(String ^strID, long lStartMillisecond, long lEndMillisecond);
			virtual void EnableVideoPlayback(String ^strID);
			virtual void DisableVideoPlayback();
			virtual void StartVideoPlayback();
			virtual void StopVideoPlayback();
			virtual void StepVideoPlayback(int iFrameCount);
			virtual void MoveSimulationToKeyFrame(String ^strID);
			virtual void SaveVideo(String ^strPath);

#pragma endregion

#pragma region HelperMethods

			virtual void ReInitializeSimulation();
			virtual System::Int32 RetrieveChartData(String ^sChartKey, cli::array<System::Single, 2> ^%aryTimeData, cli::array<System::Single, 2> ^%aryData);
			virtual void GenerateCollisionMeshFile(String ^sOriginalMeshFile, String ^sCollisionMeshFile);

#pragma endregion


#pragma region DataAccess

			virtual System::Boolean AddItem(String ^sParentID, String ^sItemType, String ^sID, String ^sXml, System::Boolean bThrowError);
			virtual System::Boolean RemoveItem(String ^sParentID, String ^sItemType, String ^sID, System::Boolean bThrowError);
			virtual System::Boolean SetData(String ^sID, String ^sDataType, String ^sValue, System::Boolean bThrowError);
			virtual System::Boolean FindItem(String ^sID, System::Boolean bThrowError);

#pragma endregion

		protected: 
			//AnimatSim::Simulator *m_lpSim;
			//AnimatSim::ISimGUICallback *m_lpSimGUICallback;
			//bool m_bPaused;
			//System::Boolean m_bIsLoaded;
			////This tells if the simulation window has been started or not. It is always true when the window is open, even if the sim is paused.
			//System::Boolean m_bSimOpen; 
			//System::String ^m_strProjectFile;
			//System::String ^m_strProjectPath;
			//System::Threading::Thread ^m_newThread;
			//System::String ^m_strErrorMessage;
			//AnimatGUI::Interfaces::ILogger ^m_lpLogger;

#pragma region SimulationThreading


			virtual void RunSimulator();

			virtual void LogMsg(ManagedAnimatInterfaces::ILogger::enumLogLevel eLevel, System::String ^sMessage);
			virtual void AddSimWindow(System::String ^sWindowXml, BOOL bInit, HWND hWnd);
			virtual void RemoveSimWindow(HWND hWnd);

#pragma endregion

		};

	}
}