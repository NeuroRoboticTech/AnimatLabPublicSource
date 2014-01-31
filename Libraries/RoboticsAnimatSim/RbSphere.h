// RbSphere.h: interface for the RbSphere class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class ROBOTICS_PORT RbSphere : public AnimatSim::Environment::Bodies::Sphere, public RbRigidBody
			{
			protected:

			public:
				RbSphere();
				virtual ~RbSphere();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
