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

		void UpdateDataCallback(void *lpSim);
		void StartSimulationCallback(void *lpSim);
		void PauseSimulationCallback(void *lpSim);
		void EndingSimulationCallback(void *lpSim);

		/// <summary> 
		/// Summary for SimulatorInterface
		/// </summary>
		public ref class SimulatorInterface :  public System::ComponentModel::Component
		{
		public:
			SimulatorInterface(void)
			{
				InitializeComponent();
				m_lpSim = NULL;
				m_bPaused = true;
				m_bIsLoaded = false;
				m_bSimOpen = false;
			}

			SimulatorInterface(System::ComponentModel::IContainer ^container) : components(nullptr)
			{
				/// <summary>
				/// Required for Windows.Forms Class Composition Designer support
				/// </summary>

				container->Add(this);
				InitializeComponent();
			}

			~SimulatorInterface()
			{
				//if(m_iInstanceID>-1) m_arySimulators->RemoveAt(m_iInstanceID);
				if(m_lpSim) delete m_lpSim;
				if (components)
				{
					delete components;
				}
			}

#pragma region EventSystems

			void FireUpdateDataEvent()    
			{
				OnUpdateData();
			}

			void FireStartSimulationEvent()    
			{
				OnStartSimulation();
			}

			void FirePauseSimulationEvent()    
			{
				OnPauseSimulation();
			}

			void FireEndingSimulationEvent()    
			{
				OnEndingSimulation();
			}

			void FireResetSimulationEvent()    
			{
				OnResetSimulation();
			}

			void FireSimulationErrorEvent()    
			{
				OnSimulationError();
			}

			//static Object* m_arySimulators __gc[] = new Array;
			//int m_iInstanceID;

			event VoidHandler^ OnUpdateData;
			event VoidHandler^ OnStartSimulation;
			event VoidHandler^ OnPauseSimulation;
			event VoidHandler^ OnEndingSimulation;
			event VoidHandler^ OnResetSimulation;
			event VoidHandler^ OnSimulationError;
			event CreateSimHandler^ OnSimulationCreate;
			event VoidHandler^ SimulationRunning;

#pragma endregion

#pragma region Properties

			property AnimatGUI::Interfaces::Logger^ Logger;		

			//__property void set_Logger(AnimatGUI::Interfaces::Logger *lpLogger)
			//{m_lpLogger = lpLogger;}

			//__property AnimatGUI::Interfaces::Logger *get_Logger()
			//{return m_lpLogger;}

			Simulator *Sim() {return m_lpSim;};

			long CurrentMillisecond()
			{
				if(m_lpSim) 
					return m_lpSim->Millisecond();
				else
					return 0;
			}

			bool Paused()
			{
				if(m_lpSim)
					return m_lpSim->Paused();
				else
					return true;
			}

			System::Boolean Loaded()
			{
				return m_bIsLoaded;
			}

			System::Boolean SimOpen()
			{
				return m_bSimOpen;
			}

#pragma endregion

#pragma region SimulationControl


			void CreateAndRunSimulation(System::Boolean bPaused)
			{
				//If the sim is already running then do not attempt to start it again.
				if(m_lpSim) return;

				CreateSimulation();
				Simulate(bPaused);
			}

			void CreateSimulation()
			{
				//If the sim is already running then do not attempt to start it again.
				if(m_lpSim) return;

				System::String ^sSimXml = "";
				OnSimulationCreate(sSimXml);

				if(sSimXml->Length == 0)
					throw gcnew System::Exception("No simulation XML was generated during creation of simulation.");

				CreateSimulation(sSimXml);
			}

			void CreateSimulation(System::String ^sXml)
			{
				try
				{					
					if(m_lpSim)
						return;

					LogMsg(Logger->enumLogLevel::Info, "Starting CreateSimulation");

					CStdXml oXml;
					oXml.Deserialize(Util::StringToStd(sXml));
					
					if(m_newThread)
						throw gcnew System::Exception("A thread is already running. You can not create a new simulation while one is currently running.");

					LogMsg(Logger->enumLogLevel::Debug, "About to create the simulation.");	
	
					m_lpSim = AnimatSim::Simulator::CreateSimulator(oXml);					

					LogMsg(Logger->enumLogLevel::Debug, "About to load the simulation.");
					m_lpSim->Load(oXml);
					m_lpSim->Paused(true);
					m_bIsLoaded = true;

					LogMsg(Logger->enumLogLevel::Info, "Finished CreateSimulation");
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to create the simulation.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to create the simulation.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			void Simulate(bool bPaused)
			{
				try
				{
					LogMsg(Logger->enumLogLevel::Info, "Starting Simulate");

					//if(strProjectFile->Length() == 0)
					//	throw new System::Exception("No project file was specified for the simulator to run.");
					if(!m_lpSim)
						throw gcnew System::Exception("You must first create a simulation before you can run it.");

					m_bPaused = bPaused;
					System::Threading::ThreadStart ^startThread = gcnew System::Threading::ThreadStart(this, &SimulatorInterface::RunSimulator);
					m_newThread = gcnew System::Threading::Thread(startThread);
					m_newThread->Start();

					//Lets block here until the the m_bSimOpen = True. This will mean that the sim has been created, loaded, and initialized.
					int iCount = 0;
					while(!m_bSimOpen)
					{
						Sleep(10);
						if(iCount > 1e6)
							throw gcnew System::Exception("The simulation failed to start correctly");
						iCount++;
					}

					LogMsg(Logger->enumLogLevel::Info, "Finished Simulate");
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to start the simulation.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to start the simulation.";
					throw gcnew System::Exception(m_strErrorMessage);
				}

			}

			//Returns a bool telling whether it had to start the sim or not.
			bool AddWindow(IntPtr hParentWnd, System::String ^sWindowXml)
			{
				try
				{
					HWND hWnd = (HWND) hParentWnd.ToInt32();

					//If there are no windows defined then lets start the simulation, otherwise just add the window to the 
					//currently running simulation.
					if(m_lpSim)
					{
						m_lpSim->WindowMgr()->AddSimulationWindow(m_lpSim, "", "Basic", true, hWnd, Util::StringToStd(sWindowXml));
						return false;
					}
					else 
					{
						CreateSimulation();
						m_lpSim->WindowMgr()->AddSimulationWindow(m_lpSim, "", "Basic", false, hWnd, Util::StringToStd(sWindowXml));
						Simulate(true);
						return true;
					}

				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to add a window to the simulation.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to add a window to the simulation.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			void RemoveWindow(IntPtr hParentWnd)
			{
				try
				{
					if(m_lpSim)
					{
						HWND hWnd = (HWND) hParentWnd.ToInt32();
						m_lpSim->WindowMgr()->RemoveSimulationWindow(m_lpSim, hWnd);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to remove a window from the simulation.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to  remove a window from the simulation.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			void ShutdownSimulation()
			{
				try
				{
					if(m_lpSim) 
						m_lpSim->ShutdownSimulation();

#ifdef _DEBUG
					if(m_newThread)
						m_newThread->Join();
#else
					if(m_newThread)
						m_newThread->Join(5000);
#endif
					m_newThread = nullptr;
					m_bIsLoaded = false;
					m_bSimOpen = false;

					if(m_lpSim)
					{
						delete m_lpSim; 
						m_lpSim = NULL;
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to shutdown the simulation.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to shutdown the simulation.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			bool StartSimulation()
			{
				try
				{
					bool bVal = false;

					if(!m_lpSim) 
						CreateAndRunSimulation(true);

					bVal = m_lpSim->StartSimulation();

					return bVal;
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to start the simulation.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to start the simulation.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			bool PauseSimulation()
			{
				try
				{
					if(!m_lpSim) 
						CreateAndRunSimulation(true);

					return m_lpSim->PauseSimulation();
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to pause the simulation.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to pause the simulation.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			void StopSimulation()
			{
				try
				{
					if(m_lpSim)
					{
						m_lpSim->PauseSimulation();

						if(m_lpSim->WaitForSimulationBlock())
						{
							m_lpSim->ResetSimulation();
							m_lpSim->UnblockSimulation();
						}
						else
							throw gcnew System::Exception("Timed out while trying to stop the simulation.");
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to pause the simulation.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to pause the simulation.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			System::String ^ErrorMessage()
			{return m_strErrorMessage;}

			void TrackCamera(System::Boolean bTrackCamera, String ^sLookAtStructureID, String ^sLookAtBodyID)
			{
				try
				{
					if(m_lpSim) 
					{
						BOOL bVal = bTrackCamera;
						string strLookAtStructureID = Util::StringToStd(sLookAtStructureID);
						string strLookAtBodyID = Util::StringToStd(sLookAtBodyID);

						//m_lpSim->SetupTrackCamera(bTrackCamera, strLookAtStructureID, strLookAtBodyID);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to call TrackCamera.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to call TrackCamera.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

#pragma endregion

#pragma region VideoPlayback

			String ^AddKeyFrame(String ^strType, long lStartMillisecond, long lEndMillisecond)
			{
				try
				{
					if(m_lpSim) 
					{
						string strSType = Util::StringToStd(strType);
						long lStart = m_lpSim->MillisecondToSlice(lStartMillisecond);
						long lEnd = m_lpSim->MillisecondToSlice(lEndMillisecond);

						return gcnew String(m_lpSim->AddKeyFrame(strSType, lStart, lEnd).c_str());
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to add the keyframe.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
					return "";
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to add a keyframe.";
					throw gcnew System::Exception(m_strErrorMessage);
					return "";
				}
				return "";
			}

			void RemoveKeyFrame(String ^strID)
			{
				try
				{
					if(m_lpSim) 
					{
						string strSID = Util::StringToStd(strID);

						m_lpSim->RemoveKeyFrame(strSID);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to add the keyframe.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to remove a keyframe.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			String ^MoveKeyFrame(String ^strID, long lStartMillisecond, long lEndMillisecond)
			{
				try
				{
					if(m_lpSim) 
					{
						string strSID = Util::StringToStd(strID);
						long lStart = m_lpSim->MillisecondToSlice(lStartMillisecond);
						long lEnd = m_lpSim->MillisecondToSlice(lEndMillisecond);

						return gcnew String(m_lpSim->MoveKeyFrame(strSID, lStart, lEnd).c_str());
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to add the keyframe.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to move a keyframe.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
				return "";
			}

			void EnableVideoPlayback(String ^strID)
			{
				try
				{
					if(m_lpSim) 
					{
						string strSID = Util::StringToStd(strID);
						m_lpSim->EnableVideoPlayback(strSID);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while enabling video playback for the keyframe.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while enabling video playback for the video for the keyframe.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			void DisableVideoPlayback()
			{
				try
				{
					if(m_lpSim) 
						m_lpSim->DisableVideoPlayback();
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to disable video playback for the keyframe.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to disable video playback for the keyframe.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			void StartVideoPlayback()
			{
				try
				{
					if(m_lpSim) 
						m_lpSim->StartVideoPlayback();
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to play the video for the keyframe.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to play the video for the keyframe.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			void StopVideoPlayback()
			{
				try
				{
					if(m_lpSim) 
						m_lpSim->StopVideoPlayback();
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to stop playing the video for the keyframe.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to stop playing the video for the keyframe.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			void StepVideoPlayback(int iFrameCount)
			{
				try
				{
					if(m_lpSim) 
						m_lpSim->StepVideoPlayback(iFrameCount);
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to step the playback of the video for the keyframe.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to step the playback of the video for the keyframe.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			void MoveSimulationToKeyFrame(String ^strID)
			{
				try
				{
					if(m_lpSim) 
					{
						string strSID = Util::StringToStd(strID);
						m_lpSim->MoveSimulationToKeyFrame(strSID);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to add the keyframe.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to move simulation to a keyframe.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			void SaveVideo(String ^strPath)
			{
				try
				{
					if(m_lpSim) 
					{
						string strSPath = Util::StringToStd(strPath);
						m_lpSim->SaveVideo(strSPath);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to add the keyframe.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to save video.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

#pragma endregion


#pragma region DataCharts
/*
			System::Boolean FindDataChart(String ^strKey, System::Boolean bThrowError)
			{
				try
				{
					if(m_lpSim) 
					{
						string strSKey = Util::StringToStd(strKey);
						
						AnimatSim::Charting::DataChartMgr *lpChartMgr = m_lpSim->DataChartMgr();
						if(lpChartMgr->Find(strSKey, FALSE))
							return true;
						else
							return false;
					}
					else
						return false;
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to find a data chart.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to find a data chart.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
				return "";
			}

			void RemoveDataChart(String ^strKey)
			{
				try
				{
					if(m_lpSim) 
					{
						string strSKey = Util::StringToStd(strKey);

						AnimatSim::Charting::DataChartMgr *lpChartMgr = m_lpSim->DataChartMgr();
						lpChartMgr->Remove(m_lpSim, strSKey);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to remove a data chart.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to remove a data chart.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			void ModifyDataChart(String ^strKey, float fltCollectWindow, float fltCollectInterval)
			{
				try
				{
					if(m_lpSim) 
					{
						string strSKey = Util::StringToStd(strKey);

						//Lets calculate the number of slices for the collect interval.
						int iCollectInterval = (int) (fltCollectInterval/m_lpSim->TimeStep());
						if(iCollectInterval<=0) iCollectInterval = 1;

						AnimatSim::Charting::DataChartMgr *lpChartMgr = m_lpSim->DataChartMgr();
						AnimatSim::Charting::DataChart *lpChart = dynamic_cast<AnimatSim::Charting::DataChart *>(lpChartMgr->Find(strSKey));

						lpChart->EndTime(fltCollectWindow);
						lpChart->CollectInterval(iCollectInterval);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to remove a data chart.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to remove a data chart.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			void AddDataChart(String ^sModuleName, String ^sType, String ^sXml)
			{
				AnimatSim::Charting::DataChart *lpChart = NULL;

				try
				{
					if(m_lpSim) 
					{
						string strModuleName = Util::StringToStd(sModuleName);
						string strType = Util::StringToStd(sType);
						string strXml = Util::StringToStd(sXml);

						lpChart = dynamic_cast<AnimatSim::Charting::DataChart *>(m_lpSim->CreateObject(strModuleName, "DataChart", strType));

						CStdXml oXml;
						oXml.Deserialize(strXml);

						lpChart->LoadChart(m_lpSim, oXml);

						AnimatSim::Charting::DataChartMgr *lpChartMgr = m_lpSim->DataChartMgr();
						lpChartMgr->Add(m_lpSim, lpChart);
					}
				}
				catch(CStdErrorInfo oError)
				{
					if(lpChart) delete lpChart;
					string strError = "An error occurred while attempting to add a data chart.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					if(lpChart) delete lpChart;
					m_strErrorMessage = "An unknown error occurred while attempting to add a data chart.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}
*/
			void ReInitializeSimulation()
			{
				try
				{
					if(m_lpSim) 
					{
						AnimatSim::Charting::DataChartMgr *lpChartMgr = m_lpSim->DataChartMgr();
						lpChartMgr->ReInitialize(m_lpSim);

						AnimatSim::ExternalStimuli::ExternalStimuliMgr *lpStimMgr = m_lpSim->ExternalStimuliMgr();
						lpStimMgr->ReInitialize(m_lpSim);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to re-initialize the data charts.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to re-initialize the data charts.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			System::Int32 RetrieveChartData(String ^sChartKey, cli::array<System::Single, 2> ^%aryTimeData, cli::array<System::Single, 2> ^%aryData)
			{
				try
				{
					System::Int32 iRowCount=0;

					if(m_lpSim) 
					{
						string strChartKey = Util::StringToStd(sChartKey);

						AnimatSim::Charting::DataChartMgr *lpChartMgr = m_lpSim->DataChartMgr();
						AnimatSim::Charting::DataChart *lpChart = dynamic_cast<AnimatSim::Charting::DataChart *>(lpChartMgr->Find(strChartKey));

						if(lpChart->Lock())
						{
							float *lpDataBuffer = lpChart->DataBuffer();
							float *lpTimeBuffer = lpChart->TimeBuffer();
							long lColCount = lpChart->ColumnCount();
							long lCurrentRow = lpChart->CurrentRow();
							//if(lCurrentRow > 10000) lCurrentRow = 10000;
							long lBufferSize = lColCount * lCurrentRow;

							if(lCurrentRow > 0)
							{
								//First lets create the arrays.
								aryTimeData = gcnew cli::array<System::Single, 2>(lColCount, lCurrentRow);
								aryData = gcnew cli::array<System::Single, 2>(lColCount, lCurrentRow);

								//Unfortuanetly, I have been completely unable to find a way to use Marshal::Copy with a 2D array.
								//So I will have to just manually copy the stupid thing by hand. I know this is much slower, but I 
								//have been unable to find another alternative.
								long lCol=0;
								if(lpTimeBuffer)
								{
									for(long lRow=0; lRow<lCurrentRow; lRow++)
									{
										for(lCol=0; lCol<lColCount; lCol++)
										{
											aryTimeData[lCol, lRow] = lpTimeBuffer[lRow];
											aryData[lCol, lRow] = lpDataBuffer[(lRow*lColCount) + lCol];
										}
									}
								}
								else
								{
									for(long lRow=0; lRow<lCurrentRow; lRow++)
									{
										for(lCol=0; lCol<lColCount; lCol++)
											aryData[lCol, lRow] = lpDataBuffer[(lRow*lColCount) + lCol];
									}
								}

								//System::Runtime::InteropServices::Marshal::Copy(IntPtr((void *)lpBuffer), aryYData, 0, lBufferSize);

								//Reset the current row to the first one.
								lpChart->CurrentRow(0);
								iRowCount = lCurrentRow;
							}

							lpChart->Unlock();
						}
					}

					return iRowCount;
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to re-initialize the data charts.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to re-initialize the data charts.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}
/*
			System::Boolean FindDataColumn(String ^sChartKey, String ^sColumnName, System::Boolean bThrowError)
			{
				try
				{
					if(m_lpSim) 
					{
						string strChartKey = Util::StringToStd(sChartKey);
						string strColumnName = Util::StringToStd(sColumnName);
						
						AnimatSim::Charting::DataChartMgr *lpChartMgr = m_lpSim->DataChartMgr();
						if(lpChartMgr->FindDataColumn(strChartKey, strColumnName, FALSE))
							return true;
						else
							return false;
					}
					else 
						return false;
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to find a data column.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to find a data column.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
				return "";
			}

			void RemoveDataColumn(String ^sChartKey, String ^sColumnName)
			{
				try
				{
					if(m_lpSim) 
					{
						string strChartKey = Util::StringToStd(sChartKey);
						string strColumnName = Util::StringToStd(sColumnName);

						AnimatSim::Charting::DataChartMgr *lpChartMgr = m_lpSim->DataChartMgr();
						lpChartMgr->RemoveDataColumn(strChartKey, strColumnName);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to remove a data chart.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to remove a data chart.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			void AddDataColumn(String ^sChartKey, String ^sColumnName, String ^sModuleName, String ^sType, String ^sXml)
			{
				AnimatSim::Charting::DataColumn *lpColumn = NULL;

				try
				{
					if(m_lpSim) 
					{
						string strChartKey = Util::StringToStd(sChartKey);
						string strColumnName = Util::StringToStd(sColumnName);
						string strModuleName = Util::StringToStd(sModuleName);
						string strType = Util::StringToStd(sType);
						string strXml = Util::StringToStd(sXml);

						lpColumn = dynamic_cast<AnimatSim::Charting::DataColumn *>(m_lpSim->CreateObject(strModuleName, "DataColumn", strType));
						
						CStdXml oXml;
						oXml.Deserialize(strXml);
						oXml.FindElement("Data");
						oXml.FindChildElement("DataColumn");

						lpColumn->Load(m_lpSim, oXml);

						AnimatSim::Charting::DataChartMgr *lpChartMgr = m_lpSim->DataChartMgr();
						lpChartMgr->AddDataColumn(strChartKey, lpColumn);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to add a data chart.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					if(lpColumn) delete lpColumn;
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to add a data chart.";
					if(lpColumn) delete lpColumn;
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			void ModifyDataColumn(String ^sChartKey, String ^sColumnName, String ^sDataType)
			{
				try
				{
					if(m_lpSim) 
					{
						string strChartKey = Util::StringToStd(sChartKey);
						string strColumnName = Util::StringToStd(sColumnName);
						string strDataType = Util::StringToStd(sDataType);

						AnimatSim::Charting::DataChartMgr *lpChartMgr = m_lpSim->DataChartMgr();
						lpChartMgr->ModifyDataColumn(strChartKey, strColumnName, strDataType);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to modify a data chart.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to modify a data chart.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			void SetDataColumnIndex(String ^sChartKey, String ^sColumnName, int iIndex)
			{
				try
				{
					if(m_lpSim) 
					{
						string strChartKey = Util::StringToStd(sChartKey);
						string strColumnName = Util::StringToStd(sColumnName);

						AnimatSim::Charting::DataChartMgr *lpChartMgr = m_lpSim->DataChartMgr();
						lpChartMgr->SetDataColumnIndex(strChartKey, strColumnName, iIndex);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to set the index of a data column.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to set the index of a data column.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}
*/
#pragma endregion
/*
#pragma region Stimuli

			System::Boolean FindStimulus(String ^sKey, System::Boolean bThrowError)
			{
				try
				{
					if(m_lpSim) 
					{
						string strKey = Util::StringToStd(sKey);
					
						AnimatSim::ExternalStimuli::ExternalStimuliMgr *lpStimMgr = m_lpSim->ExternalStimuliMgr();
						if(lpStimMgr->Find(strKey, FALSE))
							return true;
						else
							return false;
					}
					else
						return false;
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to find a stimulus.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to find a stimulus.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
				return false;
			}

			void RemoveStimulus(String ^sKey)
			{
				try
				{
					if(m_lpSim) 
					{
						string strKey = Util::StringToStd(sKey);

						AnimatSim::ExternalStimuli::ExternalStimuliMgr *lpStimMgr = m_lpSim->ExternalStimuliMgr();
						lpStimMgr->Remove(m_lpSim, strKey);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to remove a stimulus.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to remove a stimulus.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			void AddStimulus(String ^sStimulusKey, String ^sStimulusName, String ^sModuleName, String ^sType, String ^sXml)
			{
				AnimatSim::ExternalStimuli::ExternalStimulus *lpStimulus = NULL;

				try
				{
					if(m_lpSim) 
					{
						string strStimulusKey = Util::StringToStd(sStimulusKey);
						string strStimulusName = Util::StringToStd(sStimulusName);
						string strModuleName = Util::StringToStd(sModuleName);
						string strType = Util::StringToStd(sType);
						string strXml = Util::StringToStd(sXml);

						lpStimulus = dynamic_cast<AnimatSim::ExternalStimuli::ExternalStimulus *>(m_lpSim->CreateObject(strModuleName, "ExternalStimulus", strType));
						
						CStdXml oXml;
						oXml.Deserialize(strXml);
						oXml.FindElement("Root");
						oXml.FindChildElement("Stimulus");

						lpStimulus->Load(m_lpSim, oXml);

						AnimatSim::ExternalStimuli::ExternalStimuliMgr *lpStimMgr = m_lpSim->ExternalStimuliMgr();
						lpStimMgr->Add(m_lpSim, lpStimulus);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to add a stimulus.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					if(lpStimulus) delete lpStimulus;
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to add a stimulus.";
					if(lpStimulus) delete lpStimulus;
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

			void ModifyStimulus(String ^sStimulusKey, String ^sStimulusName, String ^sModuleName, String ^sType, System::Boolean bEnabled, String ^sXml)
			{
				AnimatSim::ExternalStimuli::ExternalStimulus *lpStimulus = NULL;
				AnimatSim::ExternalStimuli::ExternalStimulus *lpOldStimulus = NULL;

				try
				{
					if(m_lpSim) 
					{
						string strStimulusKey = Util::StringToStd(sStimulusKey);
						string strStimulusName = Util::StringToStd(sStimulusName);
						string strModuleName = Util::StringToStd(sModuleName);
						string strType = Util::StringToStd(sType);
						string strXml = Util::StringToStd(sXml);

						AnimatSim::ExternalStimuli::ExternalStimuliMgr *lpStimMgr = m_lpSim->ExternalStimuliMgr();
						lpOldStimulus = dynamic_cast<AnimatSim::ExternalStimuli::ExternalStimulus *>(lpStimMgr->Find(strStimulusKey, FALSE));

						if(bEnabled)
						{
							lpStimulus = dynamic_cast<AnimatSim::ExternalStimuli::ExternalStimulus *>(m_lpSim->CreateObject(strModuleName, "ExternalStimulus", strType));
							
							CStdXml oXml;
							oXml.Deserialize(strXml);
							oXml.FindElement("Root");
							oXml.FindChildElement("Stimulus");

							lpStimulus->Load(m_lpSim, oXml);

							if(lpOldStimulus)
							{
								lpStimulus->Modify(m_lpSim, lpOldStimulus);

								//Remove the old stimulus
								lpStimMgr->Remove(m_lpSim, strStimulusKey);
							}

							//Add the new stimulus
							lpStimMgr->Add(m_lpSim, lpStimulus);
						}
						else if(lpOldStimulus)
							lpStimMgr->Remove(m_lpSim, strStimulusKey);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to add a stimulus.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					if(lpStimulus) delete lpStimulus;
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to add a stimulus.";
					if(lpStimulus) delete lpStimulus;
					throw gcnew System::Exception(m_strErrorMessage);
				}
			}

#pragma endregion
*/
#pragma region DataAccess

			System::Boolean AddItem(String ^sParentID, String ^sItemType, String ^sXml, System::Boolean bThrowError)
			{
				try
				{
					if(m_lpSim) 
					{
						string strParentID = Std_Trim(Std_ToUpper(Util::StringToStd(sParentID)));
						string strItemType = Std_Trim(Std_ToUpper(Util::StringToStd(sItemType)));
						string strXml = Util::StringToStd(sXml);

						AnimatBase *lpParent = m_lpSim->FindByID(strParentID, FALSE);
						if(lpParent)
						{
							if(m_lpSim->WaitForSimulationBlock())
							{
								BOOL bVal = lpParent->AddItem(strItemType, strXml, bThrowError);

								m_lpSim->UnblockSimulation();

								return bVal;
							}
							else if(bThrowError)
								throw gcnew PropertyUpdateException("Unable to block simulation.");
							else
								return false;
						}
						else
						{
							if(bThrowError)
								throw gcnew PropertyUpdateException("Unable to find the parent item with ID: " + sParentID);
							return false;
						}
					}
					else
						return false;
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to add a data item.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew PropertyUpdateException(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to add a data item.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
				return false;
			}

			System::Boolean RemoveItem(String ^sParentID, String ^sItemType, String ^sID, System::Boolean bThrowError)
			{
				try
				{
					if(m_lpSim) 
					{
						string strParentID = Std_Trim(Std_ToUpper(Util::StringToStd(sParentID)));
						string strItemType = Std_Trim(Std_ToUpper(Util::StringToStd(sItemType)));
						string strID = Std_Trim(Std_ToUpper(Util::StringToStd(sID)));

						AnimatBase *lpParent = m_lpSim->FindByID(strParentID, FALSE);
						if(lpParent)
						{
							if(m_lpSim->WaitForSimulationBlock())
							{
								BOOL bVal = lpParent->RemoveItem(strItemType, strID, bThrowError);

								m_lpSim->UnblockSimulation();

								return bVal;
							}
							else if(bThrowError)
								throw gcnew PropertyUpdateException("Unable to block simulation.");
							else
								return false;
						}
						else
						{
							if(bThrowError)
								throw gcnew PropertyUpdateException("Unable to find the parent item with ID: " + sParentID);
							return false;
						}
					}
					else
						return false;
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to remove a data item.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew PropertyUpdateException(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to remove a data item.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
				return false;
			}

			System::Boolean SetData(String ^sID, String ^sDataType, String ^sValue, System::Boolean bThrowError)
			{
				try
				{
					if(m_lpSim) 
					{
						string strID = Std_Trim(Std_ToUpper(Util::StringToStd(sID)));
						string strDataType = Std_Trim(Std_ToUpper(Util::StringToStd(sDataType)));
						string strValue = Util::StringToStd(sValue);

						AnimatBase *lpItem = m_lpSim->FindByID(strID, FALSE);
						if(lpItem)
						{
							if(m_lpSim->WaitForSimulationBlock())
							{
								BOOL bVal = lpItem->SetData(strDataType, strValue, bThrowError);
								m_lpSim->UnblockSimulation();
								return bVal;
							}
							else
								throw gcnew System::Exception("Timed out while trying to stop the simulation.");

						}
						else
						{
							if(bThrowError)
								throw gcnew PropertyUpdateException("Unable to find the item with ID: " + sID);
							return false;
						}
					}
					else
						return false;
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to set a data value.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew PropertyUpdateException(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to set a data value.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
				return false;
			}

			
			System::Boolean FindItem(String ^sID, System::Boolean bThrowError)
			{
				try
				{
					if(m_lpSim) 
					{
						string strID = Std_Trim(Std_ToUpper(Util::StringToStd(sID)));

						AnimatBase *lpItem = m_lpSim->FindByID(strID, FALSE);
						if(lpItem)
						{
							return true;
						}
						else
						{
							if(bThrowError)
								throw gcnew PropertyUpdateException("Unable to find the item with ID: " + sID);
							return false;
						}
					}
					else
						return false;
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to find an item.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew PropertyUpdateException(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to find an item.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
				return false;
			}
#pragma endregion

		protected: 
			AnimatSim::Simulator *m_lpSim;
			bool m_bPaused;
			System::Boolean m_bIsLoaded;
			//This tells if the simulation window has been started or not. It is always true when the window is open, even if the sim is paused.
			System::Boolean m_bSimOpen; 
			System::String ^m_strProjectFile;
			System::Threading::Thread ^m_newThread;
			System::String ^m_strErrorMessage;
			AnimatGUI::Interfaces::Logger ^m_lpLogger;

#pragma region SimulationThreading


			void RunSimulator()
			{
				int iHandle = 0;

				try
				{
					LogMsg(Logger->enumLogLevel::Info, "Starting RunSimulator");

					if(!m_lpSim)
						throw gcnew System::Exception("You must first create a simulation before you can run it.");

					//m_lpSim = AnimatSim::Simulator::CreateSimulator(Util::StringToStd(m_strProjectFile));

					//System::Windows::Forms::MessageBox::Show(this, "Test", "test", System::Windows::Forms::MessageBoxButtons::YesNo,
					//								System::Windows::Forms::MessageBoxIcon::Question, System::Windows::Forms::MessageBoxDefaultButton::Button1, 
					//								System::Windows::Forms::MessageBoxOptions::RightAlign);

//#pragma push_macro("MessageBox")
//#undef MessageBox     
//System::Windows::Forms::MessageBox::Show("About to Initialize.");     
//#pragma pop_macro("MessageBox")

					LogMsg(Logger->enumLogLevel::Debug, "About to initialize the simulator");

					//HWND hWnd = (HWND) m_hParentWnd.ToInt32();
					//m_lpSim->WindowMgr()->AddSimulationWindow(m_lpSim, "", "Basic", FALSE, hWnd,  Util::StringToStd(m_strWindowXml));

					m_lpSim->Paused(m_bPaused);
					m_lpSim->Initialize(0, NULL);

					LogMsg(Logger->enumLogLevel::Debug, "Finished initializing the simulator");

//#pragma push_macro("MessageBox")
//#undef MessageBox     
//System::Windows::Forms::MessageBox::Show("Finished Initializing.");     
//#pragma pop_macro("MessageBox")

					//We need to get the handle for this instance of the simulation wrapper and pass it
					//to the simulator object so it can pass this back in callbacks. We will cast this
					//into a gc pointer for our wrapper and then fire the appropriate events to communicate
					//information about the simulator back up to the forms.
					//iHandle = (GCHandle::op_Explicit(GCHandle::Alloc(this))).ToInt32();
					GCHandle gcHndl = GCHandle::Alloc(this);
					iHandle = GCHandle::ToIntPtr(gcHndl).ToInt32();


					m_lpSim->ManagedInstance( (void *) iHandle);					
					m_lpSim->UpdateDataCallback( (ManagedCallbackPtr) UpdateDataCallback);
					m_lpSim->StartSimulationCallback( (ManagedCallbackPtr) StartSimulationCallback);
					m_lpSim->PauseSimulationCallback( (ManagedCallbackPtr) PauseSimulationCallback);
					m_lpSim->EndingSimulationCallback( (ManagedCallbackPtr) EndingSimulationCallback);

//#pragma push_macro("MessageBox")
//#undef MessageBox     
//System::Windows::Forms::MessageBox::Show("Set Callbacks.");     
//#pragma pop_macro("MessageBox")

					LogMsg(Logger->enumLogLevel::Debug, "About to start simulation processing loop");

					m_bSimOpen = true;
					SimulationRunning();

					m_lpSim->Simulate();

					LogMsg(Logger->enumLogLevel::Debug, "Finished simulation processing loop");

					//#pragma push_macro("MessageBox")
//#undef MessageBox     
//System::Windows::Forms::MessageBox::Show("Simulated.");     
//#pragma pop_macro("MessageBox")

					if(iHandle)
					{
						IntPtr iptr(iHandle);
						GCHandle::FromIntPtr(iptr).Free();
						//GCHandle::op_Explicit(iHandle).Free(); 
						iHandle = NULL;
					}
					if(m_lpSim) 
					{
						delete m_lpSim; 
						m_lpSim = NULL;
					}

					LogMsg(Logger->enumLogLevel::Info, "Finished RunSimulator");

					m_bSimOpen = false;
				}
				catch(CStdErrorInfo oError)
				{
					try
					{
						string strError = "An error occurred while attempting to start the simulation.\nError: " + oError.m_strError;
						m_strErrorMessage = gcnew String(strError.c_str());
						FireSimulationErrorEvent();
						if(iHandle)
						{
							IntPtr iptr(iHandle);
							GCHandle::FromIntPtr(iptr).Free();
							//GCHandle::op_Explicit(iHandle).Free(); 
							iHandle = NULL;
						}
						if(m_lpSim) delete m_lpSim;
						m_lpSim = NULL;
						m_bSimOpen = false;
					}
					catch(...)
					{
						m_lpSim = NULL;
						m_bSimOpen = false;
					}
				}
				catch(...)
				{
					try
					{
						//int iError = GetLastError()
						//m_strErrorMessage = "An unknown error occurred while attempting to start the simulation.";
						//FireSimulationErrorEvent();
						//if(iHandle) GCHandle::op_Explicit(iHandle).Free();
						//if(m_lpSim) delete m_lpSim;
						m_lpSim = NULL;
						m_bSimOpen = false;
					}
					catch(...)
					{
						m_lpSim = NULL;
						m_bSimOpen = false;
					}
				}
			}

			void LogMsg(AnimatGUI::Interfaces::Logger::enumLogLevel eLevel, System::String ^sMessage)
			{
				if(m_lpLogger)
					m_lpLogger->LogMsg(eLevel, sMessage);
			}

			//void Dispose(Boolean disposing)
			//{
			//	if (disposing && components)
			//	{
			//		components->Dispose();
			//	}
			//	__super::Dispose(disposing);
			//}
			/*~SimulatorInterface()
			{
				if (components)
				{
					delete components;
				}
			}*/

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