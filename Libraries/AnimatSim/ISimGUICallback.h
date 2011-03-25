#pragma once

namespace AnimatSim
{
	/**
	\brief	Callback methods for the simulation object.
	
	\author	dcofer
	\date	3/25/2011
	**/
	class ANIMAT_PORT ISimGUICallback
	{
	protected:

	public:
		ISimGUICallback(void);
		virtual ~ISimGUICallback(void);

		/**
		\brief	Callback to inform that GUI that the currently running simulation needs to be stopped.

		\details This is used when the EndSimTime property is set to allow the simulation to automatically
		stop after a set amount of time. When the simulation reaches that point it pauses the simulation and 
		fires this event. The GUI then stops the simulation.
		
		\author	dcofer
		\date	3/25/2011
		**/
		virtual void NeedToStopSimulation() = 0;
	};
}