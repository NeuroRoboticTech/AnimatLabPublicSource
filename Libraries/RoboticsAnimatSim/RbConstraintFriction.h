// RbConstraintFriction.h: interface for the RbConstraintFriction class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{

		class ROBOTICS_PORT RbConstraintFriction : public AnimatSim::Environment::ConstraintFriction
		{
		protected:
            virtual void SetFrictionProperties();

		public:
			RbConstraintFriction();
			virtual ~RbConstraintFriction();

            virtual void Initialize();
		};

	}			// Visualization
}				//RoboticsAnimatSim
