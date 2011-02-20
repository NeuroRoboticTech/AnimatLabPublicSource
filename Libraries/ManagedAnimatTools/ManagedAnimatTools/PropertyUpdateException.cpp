
#include "PropertyUpdateException.h"

namespace AnimatGUI
{
	namespace Interfaces
	{

PropertyUpdateException::PropertyUpdateException(String ^strMessage) : System::Exception(strMessage)
{
}

PropertyUpdateException::~PropertyUpdateException(void)
{
}


	}
}