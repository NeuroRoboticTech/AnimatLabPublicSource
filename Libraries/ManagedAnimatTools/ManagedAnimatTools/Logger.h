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
		/// Summary for Logger
		/// </summary>
		public ref class Logger :  public System::ComponentModel::Component
		{
		public:
			Logger(void)
			{
				InitializeComponent();
			}
			Logger(System::ComponentModel::IContainer ^container) : components(nullptr)
			{
				/// <summary>
				/// Required for Windows.Forms Class Composition Designer support
				/// </summary>

				container->Add(this);
				InitializeComponent();
			}
 
			enum class enumLogLevel
				{
					None = 0, // no trace
					Error = 10, // only trace error
					Info = 20, // some extra info
					Debug = 30, // debugging info
					Detail = 40 // detailed debugging info
				};

			property System::String^ LogPrefix
			{
				System::String^ get()
				{
					try
					{
						string strPrefix = Std_GetLogFilePrefix();
						System::String ^strVal = gcnew System::String(strPrefix.c_str());
						return strVal;
					}
					catch(CStdErrorInfo oError)
					{
						string strError = "An error occurred while get_LogPrefix.\nError: " + oError.m_strError;
						throw gcnew System::Exception(gcnew String(strError.c_str()));
					}
					catch(...)
					{throw gcnew System::Exception("An unknown error occurred while attempting to get_LogPrefix.");}
				}

				void set(System::String ^sLogFile)
				{
					try
					{ 
						//string strLogFile = Util::StringToStd(sLogFile);
	 
						//Std_TraceMsg(0, ("LogFile: " + strLogFile), __FILE__, __LINE__, STD_TRACE_TO_FILE, true);
						// 
						//Std_SetLogFilePrefix("test.txt"); //strLogFile);
					}
					catch(CStdErrorInfo oError)
					{
						string strError = "An error occurred while SetLogPrefix.\nError: " + oError.m_strError;
						throw gcnew System::Exception(gcnew String(strError.c_str()));
					}
					catch(...)
					{throw gcnew System::Exception("An unknown error occurred while attempting to SetLogPrefix.");}
				}
			}			

			property enumLogLevel TraceLevel				
			{
				enumLogLevel get()
				{
					try
					{
						 return (enumLogLevel) Std_GetTraceLevel();
					}
					catch(CStdErrorInfo oError)
					{
						string strError = "An error occurred while Std_SetTraceLevel.\nError: " + oError.m_strError;
						throw gcnew System::Exception(gcnew String(strError.c_str()));
					}
					catch(...)
					{throw gcnew System::Exception("An unknown error occurred while attempting to Std_SetTraceLevel."); return enumLogLevel::None;}
				}

				void set(enumLogLevel eLevel)
				{
					try
					{
						Std_SetTraceLevel((int)eLevel);
					}
					catch(CStdErrorInfo oError)
					{
						string strError = "An error occurred while TraceLevel.\nError: " + oError.m_strError;
						throw gcnew System::Exception(gcnew String(strError.c_str()));
					}
					catch(...)
					{throw gcnew System::Exception("An unknown error occurred while attempting to TraceLevel.");}
					}
			}			

			void LogMsg(enumLogLevel eLevel, System::String ^sMessage)
			{
				try
				{
					//string strMessage = Util::StringToStd(sMessage);
					//Std_LogMsg((int)eLevel, strMessage, "", -1, FALSE);
				}
				catch(CStdErrorInfo oError)
				{
					string strError = "An error occurred while LogMsg.\nError: " + oError.m_strError;
					throw gcnew System::Exception(gcnew String(strError.c_str()));
				}
				catch(...)
				{throw gcnew System::Exception("An unknown error occurred while attempting to LogMsg.");}

			}

		protected: 
			/*void Dispose(Boolean disposing)
			{
				if (disposing && components)
				{
					components->Dispose();
				}
				__super::Dispose(disposing);
			}*/
			~Logger()
			{
				if (components)
				{
					delete components;
				}
			}
					
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