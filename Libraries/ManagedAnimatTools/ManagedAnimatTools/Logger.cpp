#include "stdafx.h"
//#include "ILogger.h"
#include "Util.h"
#include "Logger.h"

namespace AnimatGUI
{
	namespace Interfaces
	{

		System::String ^Logger::GetLogPrefix()
		{
			try
			{
				std::string strPrefix = Std_GetLogFilePrefix();
				System::String ^strVal = gcnew System::String(strPrefix.c_str());
				return strVal;
			}
			catch(CStdErrorInfo oError)
			{
				std::string strError = "An error occurred while get_LogPrefix.\nError: " + oError.m_strError;
				throw gcnew System::Exception(gcnew System::String(strError.c_str()));
			}
			catch(...)
			{throw gcnew System::Exception("An unknown error occurred while attempting to get_LogPrefix.");}
		}

		void Logger::SetLogPrefix(System::String ^sLogFile)
		{
			try
			{ 
				std::string strLogFile = Util::StringToStd(sLogFile);
	 
				Std_TraceMsg(0, ("LogFile: " + strLogFile), __FILE__, __LINE__, STD_TRACE_TO_FILE, true);
						 
				Std_SetLogFilePrefix(strLogFile);
			}
			catch(CStdErrorInfo oError)
			{
				std::string strError = "An error occurred while SetLogPrefix.\nError: " + oError.m_strError;
				throw gcnew System::Exception(gcnew System::String(strError.c_str()));
			}
			catch(...)
			{throw gcnew System::Exception("An unknown error occurred while attempting to SetLogPrefix.");}
		}

		ManagedAnimatInterfaces::ILogger::enumLogLevel Logger::GetLogLevel()
		{
			try
			{
				//return ManagedAnimatInterfaces::ILogger::enumLogLevel::None;
				return (Logger::enumLogLevel) Std_GetTraceLevel();
			}
			catch(CStdErrorInfo oError)
			{
				std::string strError = "An error occurred while Std_SetTraceLevel.\nError: " + oError.m_strError;
				throw gcnew System::Exception(gcnew System::String(strError.c_str()));
			}
			catch(...)
			{throw gcnew System::Exception("An unknown error occurred while attempting to Std_SetTraceLevel."); return ManagedAnimatInterfaces::ILogger::enumLogLevel::None;}
		}

		void Logger::SetLogLevel(ManagedAnimatInterfaces::ILogger::enumLogLevel eLevel)
		{
			try
			{
				Std_SetTraceLevel((int) eLevel);
			}
			catch(CStdErrorInfo oError)
			{
				std::string strError = "An error occurred while TraceLevel.\nError: " + oError.m_strError;
				throw gcnew System::Exception(gcnew System::String(strError.c_str()));
			}
			catch(...)
			{throw gcnew System::Exception("An unknown error occurred while attempting to TraceLevel.");}
		}

		void Logger::LogMsg(ManagedAnimatInterfaces::ILogger::enumLogLevel eLevel, System::String ^sMessage)
		{
			try
			{
				std::string strMessage = Util::StringToStd(sMessage);
				Std_LogMsg((int)eLevel, strMessage, "", -1, true);
			}
			catch(CStdErrorInfo oError)
			{
				std::string strError = "An error occurred while LogMsg.\nError: " + oError.m_strError;
				throw gcnew System::Exception(gcnew System::String(strError.c_str()));
			}
			catch(...)
			{throw gcnew System::Exception("An unknown error occurred while attempting to LogMsg.");}

		}


	}
}