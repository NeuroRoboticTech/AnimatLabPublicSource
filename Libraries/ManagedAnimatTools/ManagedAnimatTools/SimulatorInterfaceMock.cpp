#include "stdafx.h"
#include "Util.h"
#include "Logger.h"
#include "PropertyUpdateException.h"
#include "ISimulatorInterface.h"
#include "SimulatorInterfaceMock.h"
#include "SimGUICallback.h"

namespace AnimatGUI
{
	namespace Interfaces
	{

		SimulatorInterfaceMock::SimulatorInterfaceMock(void)
		{
		/*	m_lpSim = NULL;
			m_lpSimGUICallback = NULL;
			m_bPaused = true;
			m_bIsLoaded = false;
			m_bSimOpen = false;*/
		}

		SimulatorInterfaceMock::~SimulatorInterfaceMock()
		{
			//if(m_lpSim) 
			//{
			//	delete m_lpSim;
			//	m_lpSim = NULL;
			//}
		}


#pragma region EventSystems

			void SimulatorInterfaceMock::FireNeedToStopSimulationEvent()    
			{
				try
				{
					NeedToStopSimulation();
				}
				catch(...)
				{
					//If we get an error here just eat it.
				}
			}

			void SimulatorInterfaceMock::FireHandleNonCriticalErrorEvent(System::String ^strError)    
			{
				try
				{
					HandleNonCriticalError(strError);
				}
				catch(...)
				{
					//If we get an error here just eat it.
				}
			}

			void SimulatorInterfaceMock::FireHandleCriticalErrorEvent(System::String ^strError)    
			{
				try
				{
					HandleCriticalError(strError);
				}
				catch(...)
				{
					//If we get an error here just eat it.
				}
			}

#pragma endregion

#pragma region Properties

			Simulator *SimulatorInterfaceMock::Sim() 
			{
				return NULL;
				//return m_lpSim;
			};

			long SimulatorInterfaceMock::CurrentMillisecond()
			{
				return 0;
				//if(m_lpSim) 
				//	return m_lpSim->Millisecond();
				//else
				//	return 0;
			}

			bool SimulatorInterfaceMock::Paused()
			{
				return true;
				//if(m_lpSim)
				//	return m_lpSim->Paused();
				//else
				//	return true;
			}

			bool SimulatorInterfaceMock::SimRunning()
			{
				return true;
				//if(m_lpSim)
				//	return m_lpSim->SimRunning();
				//else
				//	return false;
			}

			System::Boolean SimulatorInterfaceMock::Loaded()
			{
				return true;
				//return m_bIsLoaded;
			}

			System::Boolean SimulatorInterfaceMock::SimOpen()
			{
				return true;
				//return m_bSimOpen;
			}

			void SimulatorInterfaceMock::SetLogger(AnimatGUI::Interfaces::Logger ^lpLog)
			{
				//m_lpLogger = lpLog;
			}

#pragma endregion


#pragma region SimulationControl

			void SimulatorInterfaceMock::SetProjectPath(System::String ^strPath)
			{
				//m_strProjectPath = strPath;
			}

			void SimulatorInterfaceMock::CreateAndRunSimulation(System::Boolean bPaused)
			{
				////If the sim is already running then do not attempt to start it again.
				//if(m_lpSim) return;

				//CreateSimulation();
				//Simulate(bPaused);
			}

			void SimulatorInterfaceMock::CreateSimulation()
			{
				////If the sim is already running then do not attempt to start it again.
				//if(m_lpSim) return;

				//System::String ^sSimXml = "";
				//OnSimulationCreate(sSimXml);

				//if(sSimXml->Length == 0)
				//	throw gcnew System::Exception("No simulation XML was generated during creation of simulation.");

				//CreateSimulation(sSimXml);
			}

			void SimulatorInterfaceMock::CreateSimulation(System::String ^sXml)
			{
				/*try
				{					
					if(m_lpSim)
						return;

					LogMsg(AnimatGUI::Interfaces::Logger::enumLogLevel::Info, "Starting CreateSimulation");

					CStdXml oXml;
					oXml.Deserialize(Util::StringToStd(sXml));
					
					if(m_newThread)
						throw gcnew System::Exception("A thread is already running. You can not create a new simulation while one is currently running.");

					LogMsg(AnimatGUI::Interfaces::Logger::enumLogLevel::Debug, "About to create the simulation.");	
	
					m_lpSim = AnimatSim::Simulator::CreateSimulator(oXml);					

					LogMsg(AnimatGUI::Interfaces::Logger::enumLogLevel::Debug, "About to load the simulation.");
					string strProjectPath = Util::StringToStd(m_strProjectPath);
					m_lpSim->ProjectPath(strProjectPath);
					m_lpSim->Load(oXml);
					m_lpSim->Paused(true);
					m_bIsLoaded = true;
					
					m_lpSimGUICallback = new AnimatGUI::Interfaces::SimGUICallback(this);
					m_lpSim->SimCallBack(m_lpSimGUICallback);					

					LogMsg(AnimatGUI::Interfaces::Logger::enumLogLevel::Info, "Finished CreateSimulation");
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
				}*/
			}

			void SimulatorInterfaceMock::Simulate(bool bPaused)
			{
				//try
				//{
				//	LogMsg(AnimatGUI::Interfaces::Logger::enumLogLevel::Info, "Starting Simulate");

				//	//if(strProjectFile->Length() == 0)
				//	//	throw new System::Exception("No project file was specified for the simulator to run.");
				//	if(!m_lpSim)
				//		throw gcnew System::Exception("You must first create a simulation before you can run it.");

				//	m_bPaused = bPaused;
				//	System::Threading::ThreadStart ^startThread = gcnew System::Threading::ThreadStart(this, &SimulatorInterfaceMock::RunSimulator);
				//	m_newThread = gcnew System::Threading::Thread(startThread);
				//	m_newThread->Start();

				//	//Lets block here until the the m_bSimOpen = True. This will mean that the sim has been created, loaded, and initialized.
				//	int iCount = 0;
				//	while(!m_bSimOpen)
				//	{
				//		Sleep(10);
				//		if(iCount > 1e6)
				//			throw gcnew System::Exception("The simulation failed to start correctly");
				//		iCount++;
				//	}

				//	LogMsg(AnimatGUI::Interfaces::Logger::enumLogLevel::Info, "Finished Simulate");
				//}
				//catch(CStdErrorInfo oError)
				//{
				//	string strError = "An error occurred while attempting to start the simulation.\nError: " + oError.m_strError;
				//	m_strErrorMessage = gcnew String(strError.c_str());
				//	throw gcnew System::Exception(m_strErrorMessage);
				//}
				//catch(System::Exception ^ex)
				//{throw ex;}
				//catch(...)
				//{
				//	m_strErrorMessage = "An unknown error occurred while attempting to start the simulation.";
				//	throw gcnew System::Exception(m_strErrorMessage);
				//}

			}

			void SimulatorInterfaceMock::AddSimWindow(System::String ^sWindowXml, BOOL bInit, HWND hWnd)
			{
				/*try
				{
					if(m_lpSim->WaitForSimulationBlock())
					{
						m_lpSim->WindowMgr()->AddSimulationWindow("", "Basic", bInit, hWnd, Util::StringToStd(sWindowXml));
						m_lpSim->UnblockSimulation();
					}
					else
						throw gcnew System::Exception("Timed out while trying to stop the simulation.");
				}
				catch(CStdErrorInfo oError)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					string strError = "An error occurred while attempting to add a simulation window.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					throw ex;
				}
				catch(...)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					m_strErrorMessage = "An unknown error occurred while attempting to add a simulation window.";
					throw gcnew System::Exception(m_strErrorMessage);
				}*/
			}

			//Returns a bool telling whether it had to start the sim or not.
			bool SimulatorInterfaceMock::AddWindow(IntPtr hParentWnd, System::String ^sWindowXml)
			{
				return true;
				//try
				//{
				//	HWND hWnd = (HWND) hParentWnd.ToInt32();

				//	//If there are no windows defined then lets start the simulation, otherwise just add the window to the 
				//	//currently running simulation.
				//	if(m_lpSim)
				//	{
				//		AddSimWindow(sWindowXml, true, hWnd);
				//		return false;
				//	}
				//	else 
				//	{
				//		CreateSimulation();
				//		m_lpSim->WindowMgr()->AddSimulationWindow("", "Basic", false, hWnd, Util::StringToStd(sWindowXml));
				//		Simulate(true);
				//		return true;
				//	}

				//}
				//catch(CStdErrorInfo oError)
				//{
				//	string strError = "An error occurred while attempting to add a window to the simulation.\nError: " + oError.m_strError;
				//	m_strErrorMessage = gcnew String(strError.c_str());
				//	throw gcnew System::Exception(m_strErrorMessage);
				//}
				//catch(System::Exception ^ex)
				//{throw ex;}
				//catch(...)
				//{
				//	m_strErrorMessage = "An unknown error occurred while attempting to add a window to the simulation.";
				//	throw gcnew System::Exception(m_strErrorMessage);
				//}
			}

			void SimulatorInterfaceMock::RemoveSimWindow(HWND hWnd)
			{
				/*try
				{
					if(m_lpSim->WaitForSimulationBlock())
					{
						m_lpSim->WindowMgr()->RemoveSimulationWindow(hWnd);
						m_lpSim->UnblockSimulation();
					}
					else
						throw gcnew System::Exception("Timed out while trying to stop the simulation.");
				}
				catch(CStdErrorInfo oError)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					string strError = "An error occurred while attempting to remove a simulation window.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					throw ex;
				}
				catch(...)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					m_strErrorMessage = "An unknown error occurred while attempting to remove a simulation window.";
					throw gcnew System::Exception(m_strErrorMessage);
				}*/
			}

			void SimulatorInterfaceMock::RemoveWindow(IntPtr hParentWnd)
			{
				/*try
				{
					if(m_lpSim)
					{
						HWND hWnd = (HWND) hParentWnd.ToInt32();
						RemoveSimWindow(hWnd);
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
				}*/
			}

			void SimulatorInterfaceMock::OnWindowGetFocus(System::String ^sID)
			{
				/*try
				{
					if(m_lpSim) 
					{
						string strID = Std_Trim(Std_ToUpper(Util::StringToStd(sID)));

						SimulationWindow *lpWin = dynamic_cast<SimulationWindow *>(m_lpSim->FindByID(strID, FALSE));
						if(lpWin)
						{
							if(m_lpSim->WaitForSimulationBlock())
							{
								lpWin->OnGetFocus();

								m_lpSim->UnblockSimulation();
							}
							else
								throw gcnew System::Exception("Unable to block simulation.");
						}
						else
							throw gcnew System::Exception("Unable to find the item with ID: " + sID);
					}
				}
				catch(CStdErrorInfo oError)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					string strError = "An error occurred while attempting to set the window focus.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					throw ex;
				}
				catch(...)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					m_strErrorMessage = "An unknown error occurred while attempting to add a data item.";
					throw gcnew System::Exception(m_strErrorMessage);
				}*/
			}

			void SimulatorInterfaceMock::OnWindowLoseFocus(System::String ^sID)
			{
				/*try
				{
					if(m_lpSim) 
					{
						string strID = Std_Trim(Std_ToUpper(Util::StringToStd(sID)));

						SimulationWindow *lpWin = dynamic_cast<SimulationWindow *>(m_lpSim->FindByID(strID, FALSE));
						if(lpWin)
						{
							if(m_lpSim->WaitForSimulationBlock())
							{
								lpWin->OnLoseFocus();

								m_lpSim->UnblockSimulation();
							}
							else
								throw gcnew System::Exception("Unable to block simulation.");
						}
						else
							throw gcnew System::Exception("Unable to find the item with ID: " + sID);
					}
				}
				catch(CStdErrorInfo oError)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					string strError = "An error occurred while attempting to set the window focus.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					throw ex;
				}
				catch(...)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					m_strErrorMessage = "An unknown error occurred while attempting to add a data item.";
					throw gcnew System::Exception(m_strErrorMessage);
				}*/
			}

			void SimulatorInterfaceMock::ShutdownSimulation()
			{
				/*try
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
				}*/
			}

			bool SimulatorInterfaceMock::StartSimulation()
			{
				return true;
				/*try
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
				}*/
			}

			bool SimulatorInterfaceMock::PauseSimulation()
			{
				return true;
				/*try
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
				}*/
			}

			void SimulatorInterfaceMock::StopSimulation()
			{
				/*try
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
					if(m_lpSim) m_lpSim->UnblockSimulation();
					string strError = "An error occurred while attempting to pause the simulation.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					throw ex;
				}
				catch(...)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					m_strErrorMessage = "An unknown error occurred while attempting to pause the simulation.";
					throw gcnew System::Exception(m_strErrorMessage);
				}*/
			}

			System::String ^SimulatorInterfaceMock::ErrorMessage()
			{
				return "";
				//return m_strErrorMessage;
			}

			void SimulatorInterfaceMock::SaveSimulationFile(String ^sFile)
			{
				/*try
				{
					if(m_lpSim) 
					{
						string strFile = Util::StringToStd(sFile);

						m_lpSim->Save(strFile);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to call SaveSimulationFile.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to call SaveSimulationFile.";
					throw gcnew System::Exception(m_strErrorMessage);
				}*/
			}

			void SimulatorInterfaceMock::TrackCamera(System::Boolean bTrackCamera, String ^sLookAtStructureID, String ^sLookAtBodyID)
			{
				//try
				//{
				//	if(m_lpSim) 
				//	{
				//		BOOL bVal = bTrackCamera;
				//		string strLookAtStructureID = Util::StringToStd(sLookAtStructureID);
				//		string strLookAtBodyID = Util::StringToStd(sLookAtBodyID);

				//		//m_lpSim->SetupTrackCamera(bTrackCamera, strLookAtStructureID, strLookAtBodyID);
				//	}
				//}
				//catch(CStdErrorInfo oError)
				//{
				//	string strError = "An error occurred while attempting to call TrackCamera.\nError: " + oError.m_strError;
				//	m_strErrorMessage = gcnew String(strError.c_str());
				//	throw gcnew System::Exception(m_strErrorMessage);
				//}
				//catch(System::Exception ^ex)
				//{throw ex;}
				//catch(...)
				//{
				//	m_strErrorMessage = "An unknown error occurred while attempting to call TrackCamera.";
				//	throw gcnew System::Exception(m_strErrorMessage);
				//}
			}

#pragma endregion


#pragma region VideoPlayback

			String ^SimulatorInterfaceMock::AddKeyFrame(String ^strType, long lStartMillisecond, long lEndMillisecond)
			{
				/*try
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
				}*/
				return "";
			}

			void SimulatorInterfaceMock::RemoveKeyFrame(String ^strID)
			{
				/*try
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
				*/
			}

			String ^SimulatorInterfaceMock::MoveKeyFrame(String ^strID, long lStartMillisecond, long lEndMillisecond)
			{
				/*try
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
				}*/
				return "";
			}

			void SimulatorInterfaceMock::EnableVideoPlayback(String ^strID)
			{
				/*try
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
				}*/
			}

			void SimulatorInterfaceMock::DisableVideoPlayback()
			{
				/*try
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
				}*/
			}

			void SimulatorInterfaceMock::StartVideoPlayback()
			{
				/*try
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
				}*/
			}

			void SimulatorInterfaceMock::StopVideoPlayback()
			{
				/*try
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
				}*/
			}

			void SimulatorInterfaceMock::StepVideoPlayback(int iFrameCount)
			{
				/*try
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
				}*/
			}

			void SimulatorInterfaceMock::MoveSimulationToKeyFrame(String ^strID)
			{
				/*try
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
				}*/
			}

			void SimulatorInterfaceMock::SaveVideo(String ^strPath)
			{
				/*try
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
				}*/
			}

#pragma endregion

#pragma region HelperMethods

			void SimulatorInterfaceMock::ReInitializeSimulation()
			{
				/*try
				{
					if(m_lpSim) 
					{
						AnimatSim::Charting::DataChartMgr *lpChartMgr = m_lpSim->DataChartMgr();
						lpChartMgr->ReInitialize();

						AnimatSim::ExternalStimuli::ExternalStimuliMgr *lpStimMgr = m_lpSim->ExternalStimuliMgr();
						lpStimMgr->ReInitialize();
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
				}*/
			}

			System::Int32 SimulatorInterfaceMock::RetrieveChartData(String ^sChartKey, cli::array<System::Single, 2> ^%aryTimeData, cli::array<System::Single, 2> ^%aryData)
			{
				return 0;
				//try
				//{
				//	System::Int32 iRowCount=0;

				//	if(m_lpSim) 
				//	{
				//		string strChartKey = Util::StringToStd(sChartKey);

				//		AnimatSim::Charting::DataChartMgr *lpChartMgr = m_lpSim->DataChartMgr();
				//		AnimatSim::Charting::DataChart *lpChart = dynamic_cast<AnimatSim::Charting::DataChart *>(lpChartMgr->Find(strChartKey));

				//		if(lpChart->Lock())
				//		{
				//			float *lpDataBuffer = lpChart->DataBuffer();
				//			float *lpTimeBuffer = lpChart->TimeBuffer();
				//			long lColCount = lpChart->ColumnCount();
				//			long lCurrentRow = lpChart->CurrentRow();
				//			//if(lCurrentRow > 10000) lCurrentRow = 10000;
				//			long lBufferSize = lColCount * lCurrentRow;

				//			if(lCurrentRow > 0)
				//			{
				//				//First lets create the arrays.
				//				aryTimeData = gcnew cli::array<System::Single, 2>(lColCount, lCurrentRow);
				//				aryData = gcnew cli::array<System::Single, 2>(lColCount, lCurrentRow);

				//				//Unfortuanetly, I have been completely unable to find a way to use Marshal::Copy with a 2D array.
				//				//So I will have to just manually copy the stupid thing by hand. I know this is much slower, but I 
				//				//have been unable to find another alternative.
				//				long lCol=0;
				//				if(lpTimeBuffer)
				//				{
				//					for(long lRow=0; lRow<lCurrentRow; lRow++)
				//					{
				//						for(lCol=0; lCol<lColCount; lCol++)
				//						{
				//							aryTimeData[lCol, lRow] = lpTimeBuffer[lRow];
				//							aryData[lCol, lRow] = lpDataBuffer[(lRow*lColCount) + lCol];
				//						}
				//					}
				//				}
				//				else
				//				{
				//					for(long lRow=0; lRow<lCurrentRow; lRow++)
				//					{
				//						for(lCol=0; lCol<lColCount; lCol++)
				//							aryData[lCol, lRow] = lpDataBuffer[(lRow*lColCount) + lCol];
				//					}
				//				}

				//				//System::Runtime::InteropServices::Marshal::Copy(IntPtr((void *)lpBuffer), aryYData, 0, lBufferSize);

				//				//Reset the current row to the first one.
				//				lpChart->CurrentRow(0);
				//				iRowCount = lCurrentRow;
				//			}

				//			lpChart->Unlock();
				//		}
				//	}

				//	return iRowCount;
				//}
				//catch(CStdErrorInfo oError)
				//{
				//	string strError = "An error occurred while attempting to re-initialize the data charts.\nError: " + oError.m_strError;
				//	m_strErrorMessage = gcnew String(strError.c_str());
				//	throw gcnew System::Exception(m_strErrorMessage);
				//}
				//catch(System::Exception ^ex)
				//{throw ex;}
				//catch(...)
				//{
				//	m_strErrorMessage = "An unknown error occurred while attempting to re-initialize the data charts.";
				//	throw gcnew System::Exception(m_strErrorMessage);
				//}
			}

			void SimulatorInterfaceMock::GenerateCollisionMeshFile(String ^sOriginalMeshFile, String ^sCollisionMeshFile)
			{
				/*try
				{
					if(m_lpSim) 
					{
						string strOriginalMeshFile = Util::StringToStd(sOriginalMeshFile);
						string strCollisionMeshFile = Util::StringToStd(sCollisionMeshFile);

						m_lpSim->GenerateCollisionMeshFile(strOriginalMeshFile, strCollisionMeshFile);
					}
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while attempting to generate a collision mesh file.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew System::Exception(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{throw ex;}
				catch(...)
				{
					m_strErrorMessage = "An unknown error occurred while attempting to generate a collision mesh file.";
					throw gcnew System::Exception(m_strErrorMessage);
				}*/
			}

#pragma endregion

#pragma region DataAccess

			System::Boolean SimulatorInterfaceMock::AddItem(String ^sParentID, String ^sItemType, String ^sID, String ^sXml, System::Boolean bThrowError)
			{
				return true;
				//try
				//{
				//	if(m_lpSim) 
				//	{
				//		string strID = Std_Trim(Std_ToUpper(Util::StringToStd(sID)));
				//		string strParentID = Std_Trim(Std_ToUpper(Util::StringToStd(sParentID)));
				//		string strItemType = Std_Trim(Std_ToUpper(Util::StringToStd(sItemType)));
				//		string strXml = Util::StringToStd(sXml);

				//		//First check to see if this item already exists. If it does then do not attempt to add it again.
				//		AnimatBase *lpItem = m_lpSim->FindByID(strID, FALSE);

				//		if(lpItem != NULL)
				//			return TRUE;

				//		AnimatBase *lpParent = m_lpSim->FindByID(strParentID, FALSE);
				//		if(lpParent)
				//		{
				//			if(m_lpSim->WaitForSimulationBlock())
				//			{
				//				BOOL bVal = lpParent->AddItem(strItemType, strXml, bThrowError);

				//				m_lpSim->UnblockSimulation();

				//				return bVal;
				//			}
				//			else if(bThrowError)
				//				throw gcnew PropertyUpdateException("Unable to block simulation.");
				//			else
				//				return false;
				//		}
				//		else
				//		{
				//			if(bThrowError)
				//				throw gcnew PropertyUpdateException("Unable to find the parent item with ID: " + sParentID);
				//			return false;
				//		}
				//	}
				//	else
				//		return false;
				//}
				//catch(CStdErrorInfo oError)
				//{
				//	if(m_lpSim) m_lpSim->UnblockSimulation();
				//	string strError = "An error occurred while attempting to add a data item.\nError: " + oError.m_strError;
				//	m_strErrorMessage = gcnew String(strError.c_str());
				//	throw gcnew PropertyUpdateException(m_strErrorMessage);
				//}
				//catch(System::Exception ^ex)
				//{
				//	if(m_lpSim) m_lpSim->UnblockSimulation();
				//	throw ex;
				//}
				//catch(...)
				//{
				//	if(m_lpSim) m_lpSim->UnblockSimulation();
				//	m_strErrorMessage = "An unknown error occurred while attempting to add a data item.";
				//	throw gcnew System::Exception(m_strErrorMessage);
				//}
				//return false;
			}

			System::Boolean SimulatorInterfaceMock::RemoveItem(String ^sParentID, String ^sItemType, String ^sID, System::Boolean bThrowError)
			{
				return true;
				/*try
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
					if(m_lpSim) m_lpSim->UnblockSimulation();
					string strError = "An error occurred while attempting to remove a data item.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew PropertyUpdateException(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					throw ex;
				}
				catch(...)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					m_strErrorMessage = "An unknown error occurred while attempting to remove a data item.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
				return false;*/
			}

			System::Boolean SimulatorInterfaceMock::SetData(String ^sID, String ^sDataType, String ^sValue, System::Boolean bThrowError)
			{
				return true;
				/*try
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
					if(m_lpSim) m_lpSim->UnblockSimulation();
					string strError = "An error occurred while attempting to set a data value.\nError: " + oError.m_strError;
					m_strErrorMessage = gcnew String(strError.c_str());
					throw gcnew PropertyUpdateException(m_strErrorMessage);
				}
				catch(System::Exception ^ex)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					throw ex;
				}
				catch(...)
				{
					if(m_lpSim) m_lpSim->UnblockSimulation();
					m_strErrorMessage = "An unknown error occurred while attempting to set a data value.";
					throw gcnew System::Exception(m_strErrorMessage);
				}
				return false;*/
			}

			
			System::Boolean SimulatorInterfaceMock::FindItem(String ^sID, System::Boolean bThrowError)
			{
				return true;
				/*try
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
				return false;*/
			}

#pragma endregion

			
#pragma region SimulationThreading

			void SimulatorInterfaceMock::RunSimulator()
			{
//				int iHandle = 0;
//
//				try
//				{
//					LogMsg(AnimatGUI::Interfaces::Logger::enumLogLevel::Info, "Starting RunSimulator");
//
//					if(!m_lpSim)
//						throw gcnew System::Exception("You must first create a simulation before you can run it.");
//
//					//m_lpSim = AnimatSim::Simulator::CreateSimulator(Util::StringToStd(m_strProjectFile));
//
//					//System::Windows::Forms::MessageBox::Show(this, "Test", "test", System::Windows::Forms::MessageBoxButtons::YesNo,
//					//								System::Windows::Forms::MessageBoxIcon::Question, System::Windows::Forms::MessageBoxDefaultButton::Button1, 
//					//								System::Windows::Forms::MessageBoxOptions::RightAlign);
//
////#pragma push_macro("MessageBox")
////#undef MessageBox     
////System::Windows::Forms::MessageBox::Show("About to Initialize.");     
////#pragma pop_macro("MessageBox")
//
//					LogMsg(AnimatGUI::Interfaces::Logger::enumLogLevel::Debug, "About to initialize the simulator");
//
//					//HWND hWnd = (HWND) m_hParentWnd.ToInt32();
//					//m_lpSim->WindowMgr()->AddSimulationWindow(m_lpSim, "", "Basic", FALSE, hWnd,  Util::StringToStd(m_strWindowXml));
//
//					m_lpSim->Paused(m_bPaused);
//					m_lpSim->Initialize(0, NULL);
//
//					LogMsg(AnimatGUI::Interfaces::Logger::enumLogLevel::Debug, "Finished initializing the simulator");
//
////#pragma push_macro("MessageBox")
////#undef MessageBox     
////System::Windows::Forms::MessageBox::Show("Finished Initializing.");     
////#pragma pop_macro("MessageBox")
//
//
////#pragma push_macro("MessageBox")
////#undef MessageBox     
////System::Windows::Forms::MessageBox::Show("Set Callbacks.");     
////#pragma pop_macro("MessageBox")
//
//					LogMsg(AnimatGUI::Interfaces::Logger::enumLogLevel::Debug, "About to start simulation processing loop");
//
//					m_bSimOpen = true;
//					SimulationRunning();
//
//					m_lpSim->Simulate();
//
//					LogMsg(AnimatGUI::Interfaces::Logger::enumLogLevel::Debug, "Finished simulation processing loop");
//
//					//#pragma push_macro("MessageBox")
////#undef MessageBox     
////System::Windows::Forms::MessageBox::Show("Simulated.");     
////#pragma pop_macro("MessageBox")
//
//					if(iHandle)
//					{
//						IntPtr iptr(iHandle);
//						GCHandle::FromIntPtr(iptr).Free();
//						//GCHandle::op_Explicit(iHandle).Free(); 
//						iHandle = NULL;
//					}
//					//if(m_lpSim) 
//					//{
//					//	delete m_lpSim; 
//					//	m_lpSim = NULL;
//					//}
//
//					LogMsg(AnimatGUI::Interfaces::Logger::enumLogLevel::Info, "Finished RunSimulator");
//
//					m_bSimOpen = false;
//				}
//				catch(CStdErrorInfo oError)
//				{
//					try
//					{
//						string strError = "A critical error occured while running the simulator. The application is shutting down.\nError: " + oError.m_strError;
//						m_strErrorMessage = gcnew String(strError.c_str());
//						//this->FireHandleCriticalErrorEvent(m_strErrorMessage);
//						if(iHandle)
//						{
//							IntPtr iptr(iHandle);
//							GCHandle::FromIntPtr(iptr).Free();
//							//GCHandle::op_Explicit(iHandle).Free(); 
//							iHandle = NULL;
//						}
//						if(m_lpSim) delete m_lpSim;
//						m_lpSim = NULL;
//						m_bSimOpen = false;
//					}
//					catch(...)
//					{
//						m_lpSim = NULL;
//						m_bSimOpen = false;
//					}
//				}
//				catch(...)
//				{
//					try
//					{
//						this->FireHandleCriticalErrorEvent("A critical error occured while running the simulator. The application is shutting down.");
//						m_lpSim = NULL;
//						m_bSimOpen = false;
//					}
//					catch(...)
//					{
//						m_lpSim = NULL;
//						m_bSimOpen = false;
//					}
//				}
			}

			void SimulatorInterfaceMock::LogMsg(AnimatGUI::Interfaces::Logger::enumLogLevel eLevel, System::String ^sMessage)
			{
			/*	if(m_lpLogger)
					m_lpLogger->LogMsg(eLevel, sMessage);*/
			}

#pragma endregion

	}
}