// RbFirmataDynamixelHinge.h: interface for the RbFirmataDynamixelHinge class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{
			namespace Firmata
			{

class ROBOTICS_PORT RbFirmataDynamixelHinge : public RbFirmataDynamixelServo
{
protected:
    Hinge *m_lpHinge;

public:
	RbFirmataDynamixelHinge();
	virtual ~RbFirmataDynamixelHinge();

	virtual void SetupIO();
	virtual void Initialize();
};

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

