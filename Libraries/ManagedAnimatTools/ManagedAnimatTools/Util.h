#pragma once

namespace AnimatGUI
{
	namespace Interfaces
	{
		class Util
		{
		public:
			Util(void);
			virtual ~Util(void);

			static std::string StringToStd( System::String ^s);
			static std::string StringToStd( System::String ^s, ManagedAnimatInterfaces::ILogger ^lpLogger);
		};
	}
}

