/**
\file	VsOdorSensor.h

\brief	Declares the vortex odor sensor class.
**/

#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

class VORTEX_PORT VsOdorSensor : public AnimatSim::Environment::Bodies::OdorSensor, public VortexAnimatSim::Environment::VsRigidBody
{
protected:
	virtual void CreateGraphicsGeometry();
	virtual void CreatePhysicsGeometry();
	virtual void ResizePhysicsGeometry();

public:
	VsOdorSensor();
	virtual ~VsOdorSensor();

	virtual void CreateParts();
};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
