// RbCylinder.h: interface for the RbCylinder class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class ROBOTICS_PORT RbCylinder : public AnimatSim::Environment::Bodies::Cylinder, public RbRigidBody
			{
			protected:

			public:
				RbCylinder();
				virtual ~RbCylinder();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
