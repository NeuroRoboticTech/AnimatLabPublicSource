/**
\file	RbOrganism.h

\brief	Declares the vortex organism class.
**/

#pragma once


namespace RoboticsAnimatSim
{
	namespace Environment
	{
		/**
		\brief	Vortex Organism implementation. 
		
		\author	dcofer
		\date	8/27/2011
		**/
		class ROBOTICS_PORT RbOrganism : public AnimatSim::Environment::Organism,  public RbMovableItem      
		{
		protected:
			Structure *m_lpThisST;
			Organism *m_lpThisOG;
			RbMovableItem *m_lpRbBody;

			virtual void SetThisPointers();

		public:
			RbOrganism();
			virtual ~RbOrganism();

			virtual void Body(RigidBody *lpBody);

			virtual void Create();
			virtual void ResetSimulation();
		};

	}			// Environment
}				//RoboticsAnimatSim
