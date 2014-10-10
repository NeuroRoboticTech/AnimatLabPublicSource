/**
\file	RbEllipsoid.h

\brief	Declares the vortex ellipsoid class.
**/

#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class ROBOTICS_PORT RbEllipsoid : public AnimatSim::Environment::Bodies::Ellipsoid, public RbRigidBody
			{
			protected:

			public:
				RbEllipsoid();
				virtual ~RbEllipsoid();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
