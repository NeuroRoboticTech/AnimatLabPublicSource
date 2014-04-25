// RbDynamixelCM5USBUARTPrismaticController.h: interface for the RbDynamixelCM5USBUARTPrismaticController class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{
			namespace DynamixelUSB
			{

class ROBOTICS_PORT RbDynamixelUSBPrismatic : public AnimatSim::Robotics::RobotPartInterface
{
protected:
    RbPrismatic *m_lpPrismatic;

public:
	RbDynamixelUSBPrismatic();
	virtual ~RbDynamixelUSBPrismatic();

    virtual void StepSimulation();
};

			}	//DynamixelUSB
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

