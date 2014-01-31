/**
\file	RbStructure.h

\brief	Declares the vortex structure class.
**/

#pragma once

namespace RoboticsAnimatSim
{

	/**
	\namespace	RoboticsAnimatSim::Environment

	\brief	Classes for the virtual world simulation that use the vortex physics engine. 
	**/
	namespace Environment
	{
		/**
		\brief	Vortex physical structure implementation. 
		
		\author	dcofer
		\date	4/25/2011
		**/
		class ROBOTICS_PORT RbStructure : public AnimatSim::Environment::Structure,  public RbMovableItem   
		{
		protected:
			Structure *m_lpThisST;
			RbMovableItem *m_lpRbBody;

			virtual void SetThisPointers();

		public:
			RbStructure();
			virtual ~RbStructure();

			virtual void Body(RigidBody *lpBody);

            virtual void Create();
			virtual void ResetSimulation();
        };

	}			// Environment
}				//RoboticsAnimatSim
