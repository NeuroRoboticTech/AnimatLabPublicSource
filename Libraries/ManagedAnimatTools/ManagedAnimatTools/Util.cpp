#include "StdAfx.h"
#include "Util.h"

#using <mscorlib.dll>
using namespace System;

namespace AnimatGUI
{
	namespace Interfaces
	{
		Util::Util(void)
		{
		}

		Util::~Util(void)
		{
		}

		std::string Util::StringToStd( System::String ^s)
		{
			if(s == nullptr)
				throw gcnew System::Exception("Cannot convert a string that is null to a valid type!");

			using namespace System::Runtime::InteropServices;
			const char* chars = 
				(const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
			std::string os = chars;
			Marshal::FreeHGlobal(System::IntPtr((void*)chars));
			return os;
		}
	}
}

