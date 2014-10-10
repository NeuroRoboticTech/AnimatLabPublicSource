/**
\file	RbTorus.h

\brief	Declares the vortex Torus class.
**/

#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class ROBOTICS_PORT RbTorus : public AnimatSim::Environment::Bodies::Torus, public RbRigidBody
			{
			protected:

			public:
				RbTorus();
				virtual ~RbTorus();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
