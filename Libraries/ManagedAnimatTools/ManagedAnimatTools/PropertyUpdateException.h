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


public ref class PropertyUpdateException : public System::Exception
{
public:
	PropertyUpdateException(System::String ^strMessage);
	virtual ~PropertyUpdateException(void);
};


	}
}