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
		public ref class Logger : public ManagedAnimatInterfaces::ILogger
		{
		public:
			Logger(void) {}
 
			virtual property System::String^ LogPrefix
			{
				System::String^ get() 
				{
					return GetLogPrefix();
				}

				void set(System::String ^sLogFile)
				{
					SetLogPrefix(sLogFile);
				}
			};			

			virtual property ManagedAnimatInterfaces::ILogger::enumLogLevel TraceLevel				
			{
				ManagedAnimatInterfaces::ILogger::enumLogLevel get()
				{
					return GetLogLevel();
				}

				void set(ManagedAnimatInterfaces::ILogger::enumLogLevel eLevel)
				{
					SetLogLevel(eLevel);
				}
			};			

			virtual void LogMsg(ManagedAnimatInterfaces::ILogger::enumLogLevel eLevel, System::String ^sMessage);

		protected: 
			
			~Logger() {}
		
			System::String ^GetLogPrefix();
			void SetLogPrefix(System::String ^sLogFile);

			ManagedAnimatInterfaces::ILogger::enumLogLevel GetLogLevel();
			void SetLogLevel(ManagedAnimatInterfaces::ILogger::enumLogLevel eLevel);
		};

	}
}