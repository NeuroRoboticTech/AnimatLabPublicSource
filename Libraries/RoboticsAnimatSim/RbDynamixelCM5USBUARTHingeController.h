// RbDynamixelCM5USBUARTHingeController.h: interface for the RbDynamixelCM5USBUARTHingeController class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace MotorControlSystems
		{

class ROBOTICS_PORT RbDynamixelCM5USBUARTHingeController : public AnimatSim::Robotics::RobotPartInterface
{
protected:
    RbHinge *m_lpHinge;
    float m_fltPos;
    int m_iCounter;
    int m_iSign;

	virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify);

public:
	RbDynamixelCM5USBUARTHingeController();
	virtual ~RbDynamixelCM5USBUARTHingeController();

    virtual void StepSimulation();
};

		}		//MotorControlSystems
	}			// Robotics
}				//RoboticsAnimatSim

