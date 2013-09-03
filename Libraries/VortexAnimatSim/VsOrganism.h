/**
\file	VsOrganism.h

\brief	Declares the vortex organism class.
**/

#pragma once


namespace VortexAnimatSim
{
	namespace Environment
	{
		/**
		\brief	Vortex Organism implementation. 
		
		\author	dcofer
		\date	8/27/2011
		**/
		class VORTEX_PORT VsOrganism : public AnimatSim::Environment::Organism,  public VsMovableItem      
		{
		protected:
			Structure *m_lpThisST;
			Organism *m_lpThisOG;
			VsRigidBody *m_lpVsBody;
			Vx::VxAssembly *m_lpAssembly;

			virtual void SetThisPointers();
			virtual void SetupPhysics();
			virtual void DeletePhysics() {};
			virtual void UpdatePositionAndRotationFromMatrix();

		public:
			VsOrganism();
			virtual ~VsOrganism();

			virtual void Body(RigidBody *lpBody);
			virtual void *Assembly() {return (void *)m_lpAssembly;};

			virtual osg::Group *ParentOSG();
			virtual void Create();
			virtual void ResetSimulation();
			virtual void Physics_Resize() {};

		};

	}			// Environment
}				//VortexAnimatSim
