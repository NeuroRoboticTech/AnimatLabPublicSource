#include "StdAfx.h"
#include "Util.h"

using namespace System;

namespace ManagedAnimatSim
{
	Util::Util(void)
	{
	}

	Util::~Util(void)
	{
	}

	std::string Util::StringToStd( System::String ^s)
	{
		return StringToStd(s, nullptr);
	}

	std::string Util::StringToStd( System::String ^s, ManagedAnimatInterfaces::ILogger ^lpLogger)
	{
		if(lpLogger != nullptr) lpLogger->LogMsg(ManagedAnimatInterfaces::ILogger::enumLogLevel::Detail, "About to call StringToStd." + s);     

		//string strMsg = "Starting Util::StringToStd";
		//StdUtils::Std_LogMsg(40, strMsg, "", -1, false);

		//TRACE_DETAIL(strMsg);

		if(s == nullptr)
			throw gcnew System::Exception("Cannot convert a string that is null to a valid type!");

		using namespace System::Runtime::InteropServices;

		if(lpLogger != nullptr) lpLogger->LogMsg(ManagedAnimatInterfaces::ILogger::enumLogLevel::Detail, "StringToHGlobalAnsi");     

		const char* chars = 
			(const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
		std::string os = chars;

		if(lpLogger != nullptr) lpLogger->LogMsg(ManagedAnimatInterfaces::ILogger::enumLogLevel::Detail, "FreeHGlobal");     

		Marshal::FreeHGlobal(System::IntPtr((void*)chars));

		if(lpLogger != nullptr) lpLogger->LogMsg(ManagedAnimatInterfaces::ILogger::enumLogLevel::Detail, "Finished StringToStd.");     

		//TRACE_DETAIL("Finished Util::StringToStd: " + os);

		return os;
	}
}

