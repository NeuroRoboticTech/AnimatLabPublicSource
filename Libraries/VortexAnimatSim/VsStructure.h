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
			VsRigidBody *m_lpVsBody;
			Vx::VxAssembly *m_lpAssembly;

			virtual void SetThisPointers();
			virtual void SetupPhysics();
			virtual void DeletePhysics() {};
			virtual void UpdatePositionAndRotationFromMatrix();

		public:
			VsStructure();
			virtual ~VsStructure();

			virtual void Body(RigidBody *lpBody);
			virtual void *Assembly() {return (void *)m_lpAssembly;};

			virtual osg::Group *ParentOSG();
			virtual void Create();
			virtual void ResetSimulation();
			virtual void Physics_Resize() {};
		};

	}			// Environment
}				//VortexAnimatSim
