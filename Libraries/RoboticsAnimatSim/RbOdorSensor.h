/**
\file	RbOdorSensor.h

\brief	Declares the vortex odor sensor class.
**/

#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

class ROBOTICS_PORT RbOdorSensor : public AnimatSim::Environment::Bodies::OdorSensor, public RbRigidBody
{
protected:

public:
	RbOdorSensor();
	virtual ~RbOdorSensor();
    
	virtual void CreateParts();
};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
