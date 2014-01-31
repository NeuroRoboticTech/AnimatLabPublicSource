// RbAttachment.h: interface for the RbAttachment class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

class ROBOTICS_PORT RbAttachment  : public AnimatSim::Environment::Bodies::Attachment, public RbRigidBody
{
protected:

public:
	RbAttachment();
	virtual ~RbAttachment();

    virtual bool AddRbNodeToParent() {return true;};
    virtual bool Physics_IsGeometryDefined() {return false;};
};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

