#pragma once

namespace AnimatSim
{


	class ANIMAT_PORT ISimGUICallback
	{
	protected:

	public:
		ISimGUICallback(void);
		virtual ~ISimGUICallback(void);

		virtual void NeedToStopSimulation() = 0;
	};
}