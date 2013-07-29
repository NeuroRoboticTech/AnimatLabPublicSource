// VsAttachment.h: interface for the VsAttachment class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

class VORTEX_PORT VsAttachment  : public AnimatSim::Environment::Bodies::Attachment, public VsRigidBody
{
protected:

	virtual void CreateGraphicsGeometry();
	virtual void CreatePhysicsGeometry();
	virtual void ResizePhysicsGeometry();

public:
	VsAttachment();
	virtual ~VsAttachment();

	virtual void CreateParts();
};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

