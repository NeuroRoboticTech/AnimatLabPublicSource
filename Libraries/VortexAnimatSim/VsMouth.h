/**
\file	VsMouth.h

\brief	Declares the vortex mouth class.
**/

#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

class VORTEX_PORT VsMouth : public AnimatSim::Environment::Bodies::Mouth, public VortexAnimatSim::Environment::VsRigidBody
{
protected:
	virtual void CreateGraphicsGeometry();
	virtual void CreatePhysicsGeometry();
	virtual void ResizePhysicsGeometry();

public:
	VsMouth();
	virtual ~VsMouth();

	virtual void CreateParts();
};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
