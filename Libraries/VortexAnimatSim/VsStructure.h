/**
\file	VsStructure.h

\brief	Declares the vortex structure class.
**/

#pragma once

namespace VortexAnimatSim
{

	/**
	\namespace	VortexAnimatSim::Environment

	\brief	Classes for the virtual world simulation that use the vortex physics engine. 
	**/
	namespace Environment
	{
		/**
		\brief	Vortex physical structure implementation. 
		
		\author	dcofer
		\date	4/25/2011
		**/
		class VORTEX_PORT VsStructure : public AnimatSim::Environment::Structure,  public VsMovableItem   
		{
		protected:
			Structure *m_lpThisST;
			Vx::VxAssembly *m_lpAssembly;

			virtual void SetThisPointers();
			virtual void SetupPhysics();
			virtual void UpdatePositionAndRotationFromMatrix();

		public:
			VsStructure();
			virtual ~VsStructure();

			virtual void *Assembly() {return (void *)m_lpAssembly;};

			virtual osg::Group *ParentOSG();
			virtual void Create();
			virtual void ResetSimulation();
		};

	}			// Environment
}				//VortexAnimatSim
