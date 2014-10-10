// RbConstraintRelaxation.h: interface for the RbConstraintRelaxation class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{

		class ROBOTICS_PORT RbConstraintRelaxation : public AnimatSim::Environment::ConstraintRelaxation
		{
		protected:
			virtual void SetRelaxationProperties();

		public:
			RbConstraintRelaxation();
			virtual ~RbConstraintRelaxation();

            virtual void Initialize();
		};

	}			// Visualization
}				//RoboticsAnimatSim
