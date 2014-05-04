// RbFirmataHingeServo.h: interface for the RbFirmataHingeServo class.
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

class ROBOTICS_PORT RbFirmataHingeServo : public RbFirmataPart
{
protected:
    RbHinge *m_lpHinge;

public:
	RbFirmataHingeServo();
	virtual ~RbFirmataHingeServo();

	virtual void SetupIO();
	virtual void StepIO();

	virtual void Initialize();
    virtual void StepSimulation();
};

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

