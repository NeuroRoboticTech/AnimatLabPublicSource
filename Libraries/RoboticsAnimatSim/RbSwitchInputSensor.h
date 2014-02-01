// RbSwitchInputSensor.h: interface for the RbSwitchInputSensor class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace InputSensorSystems
		{

class ROBOTICS_PORT RbSwitchInputSensor : public AnimatSim::Robotics::RobotPartInterface
{
protected:
    RigidBody *m_lpBody;

	virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify);

public:
	RbSwitchInputSensor();
	virtual ~RbSwitchInputSensor();

    virtual void StepSimulation();
};

		}		//MotorControlSystems
	}			// Robotics
}				//RoboticsAnimatSim

