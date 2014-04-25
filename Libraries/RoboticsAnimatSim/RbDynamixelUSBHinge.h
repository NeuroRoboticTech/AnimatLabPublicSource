// RbDynamixelCM5USBUARTHingeController.h: interface for the RbDynamixelCM5USBUARTHingeController class.
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

class ROBOTICS_PORT RbDynamixelUSBHinge : public AnimatSim::Robotics::RobotPartInterface
{
protected:
    RbHinge *m_lpHinge;
    float m_fltPos;
    int m_iCounter;
    int m_iSign;

public:
	RbDynamixelUSBHinge();
	virtual ~RbDynamixelUSBHinge();

    virtual void StepSimulation();
};

			}	//DynamixelUSB
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

