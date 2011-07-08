/**
\file	VsLight.h

\brief	Declares the vortex Light class.
**/

#pragma once

namespace VortexAnimatSim
{

	/**
	\namespace	VortexAnimatSim::Environment

	\brief	Implements the light object within osg. 
	**/
	namespace Environment
	{
		/**
		\brief	Vortex physical structure implementation. 
		
		\author	dcofer
		\date	4/25/2011
		**/
		class VORTEX_PORT VsLight : public AnimatSim::Environment::Light,  public VsMovableItem   
		{
		protected:
			AnimatSim::Environment::Light *m_lpThisLI;

			virtual void SetThisPointers();

		public:
			VsLight();
			virtual ~VsLight();

			virtual void Create();
			virtual void ResetSimulation();
		};

	}			// Environment
}				//VortexAnimatSim
