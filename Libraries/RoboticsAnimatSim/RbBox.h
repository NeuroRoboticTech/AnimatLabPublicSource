
#pragma once


namespace RoboticsAnimatSim
{
	namespace Environment
	{

		/**
		\namespace	RoboticsAnimatSim::Environment::Bodies

		\brief	Body part classes that use the vortex physics engine. 
		**/
		namespace Bodies
		{

			class ROBOTICS_PORT RbBox : public AnimatSim::Environment::Bodies::Box, public RbRigidBody
			{
			protected:

			public:
				RbBox();
				virtual ~RbBox();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
