// RbCone.h: interface for the RbCone class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

 
namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class ROBOTICS_PORT RbCone : public AnimatSim::Environment::Bodies::Cone, public RbRigidBody
			{
			protected:

			public:
				RbCone();
				virtual ~RbCone();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
