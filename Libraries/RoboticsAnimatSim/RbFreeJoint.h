/**
\file	RbFreeJoint.h

\brief	Declares the vs universal class.
**/

#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

		    class ROBOTICS_PORT RbFreeJoint : public RbJoint, public BallSocket     
		    {
		    protected:

		    public:
			    RbFreeJoint();
			    virtual ~RbFreeJoint();

    #pragma region DataAccesMethods

#pragma endregion

			    virtual void CreateJoint();
		    };

    	}			// Joints
	}			// Environment
}				//VortexAnimatSim
