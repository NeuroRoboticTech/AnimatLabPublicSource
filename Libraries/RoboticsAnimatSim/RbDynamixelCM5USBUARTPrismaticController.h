// RbDynamixelCM5USBUARTPrismaticController.h: interface for the RbDynamixelCM5USBUARTPrismaticController class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace MotorControlSystems
		{

class ROBOTICS_PORT RbDynamixelCM5USBUARTPrismaticController : public AnimatSim::Robotics::RobotPartInterface
{
protected:
    RbPrismatic *m_lpPrismatic;

	virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify);

public:
	RbDynamixelCM5USBUARTPrismaticController();
	virtual ~RbDynamixelCM5USBUARTPrismaticController();

    virtual void StepSimulation();
};

		}		//MotorControlSystems
	}			// Robotics
}				//RoboticsAnimatSim

