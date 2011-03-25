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
 
		public delegate void VoidHandler();
		public delegate void CreateSimHandler(System::String ^%strXml);

		/// <summary> 
		/// Summary for SimulatorInterface
		/// </summary>
		public ref class SimulatorInterface :  public System::ComponentModel::Component
		{
		public:
			SimulatorInterface(void);
			SimulatorInterface(System::ComponentModel::IContainer ^container);
			~SimulatorInterface();

#pragma region EventSystems

			event CreateSimHandler^ OnSimulationCreate;
			event VoidHandler^ SimulationRunning;
			event VoidHandler^ NeedToStopSimulation;

			void FireNeedToStopSimulationEvent();

#pragma endregion

#pragma region Properties

			property AnimatGUI::Interfaces::Logger^ Logger;		

			Simulator *Sim();
			long CurrentMillisecond();
			bool Paused();
			System::Boolean Loaded();
			System::Boolean SimOpen();

#pragma endregion

#pragma region SimulationControl

			bool AddWindow(IntPtr hParentWnd, System::String ^sWindowXml);
			void RemoveWindow(IntPtr hParentWnd);

			void CreateAndRunSimulation(System::Boolean bPaused);
			void CreateSimulation();
			void CreateSimulation(System::String ^sXml);
			void Simulate(bool bPaused);
			void ShutdownSimulation();
			bool StartSimulation();
			bool PauseSimulation();
			void StopSimulation();

			System::String ^ErrorMessage();

			void SaveSimulationFile(String ^sFile);
			void TrackCamera(System::Boolean bTrackCamera, String ^sLookAtStructureID, String ^sLookAtBodyID);

#pragma endregion

#pragma region VideoPlayback

			String ^AddKeyFrame(String ^strType, long lStartMillisecond, long lEndMillisecond);
			void RemoveKeyFrame(String ^strID);
			String ^MoveKeyFrame(String ^strID, long lStartMillisecond, long lEndMillisecond);
			void EnableVideoPlayback(String ^strID);
			void DisableVideoPlayback();
			void StartVideoPlayback();
			void StopVideoPlayback();
			void StepVideoPlayback(int iFrameCount);
			void MoveSimulationToKeyFrame(String ^strID);
			void SaveVideo(String ^strPath);

#pragma endregion

			void ReInitializeSimulation();
			System::Int32 RetrieveChartData(String ^sChartKey, cli::array<System::Single, 2> ^%aryTimeData, cli::array<System::Single, 2> ^%aryData);

#pragma region DataAccess

			System::Boolean AddItem(String ^sParentID, String ^sItemType, String ^sXml, System::Boolean bThrowError);
			System::Boolean RemoveItem(String ^sParentID, String ^sItemType, String ^sID, System::Boolean bThrowError);
			System::Boolean SetData(String ^sID, String ^sDataType, String ^sValue, System::Boolean bThrowError);
			System::Boolean FindItem(String ^sID, System::Boolean bThrowError);

#pragma endregion

		protected: 
			AnimatSim::Simulator *m_lpSim;
			AnimatSim::ISimGUICallback *m_lpSimGUICallback;
			bool m_bPaused;
			System::Boolean m_bIsLoaded;
			//This tells if the simulation window has been started or not. It is always true when the window is open, even if the sim is paused.
			System::Boolean m_bSimOpen; 
			System::String ^m_strProjectFile;
			System::Threading::Thread ^m_newThread;
			System::String ^m_strErrorMessage;
			AnimatGUI::Interfaces::Logger ^m_lpLogger;

#pragma region SimulationThreading


			void RunSimulator();

			void LogMsg(AnimatGUI::Interfaces::Logger::enumLogLevel eLevel, System::String ^sMessage);

#pragma endregion

		private:
			/// <summary>
			/// Required designer variable.
			/// </summary>
			System::ComponentModel::Container ^components;
			
			/// <summary>
			/// Required method for Designer support - do not modify
			/// the contents of this method with the code editor.
			/// </summary>		
			void InitializeComponent(void)
			{
				components = gcnew System::ComponentModel::Container();
			}
		};

	}
}