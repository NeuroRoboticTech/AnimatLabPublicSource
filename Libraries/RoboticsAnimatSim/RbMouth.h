/**
\file	RbMouth.h

\brief	Declares the vortex mouth class.
**/

#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

class ROBOTICS_PORT RbMouth : public AnimatSim::Environment::Bodies::Mouth, public RbRigidBody
{
protected:

public:
	RbMouth();
	virtual ~RbMouth();

	virtual void CreateParts();
};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
