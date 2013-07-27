/**
\file	OsgOrganism.h

\brief	Declares the vortex organism class.
**/

#pragma once


namespace OsgAnimatSim
{
	namespace Environment
	{
		/**
		\brief	Vortex Organism implementation. 
		
		\author	dcofer
		\date	8/27/2011
		**/
		class ANIMAT_OSG_PORT OsgOrganism : public AnimatSim::Environment::Organism,  public OsgMovableItem      
		{
		protected:
			Structure *m_lpThisST;
			Organism *m_lpThisOG;
			OsgMovableItem *m_lpOsgBody;

			virtual void SetThisPointers();
			virtual void SetupPhysics();
			virtual void DeletePhysics() {};
			virtual void UpdatePositionAndRotationFromMatrix();

		public:
			OsgOrganism();
			virtual ~OsgOrganism();

			virtual void Body(RigidBody *lpBody);

			virtual osg::Group *ParentOSG();
			virtual void Create();
			virtual void ResetSimulation();
			virtual void Physics_Resize() {};

		};

	}			// Environment
}				//OsgAnimatSim
