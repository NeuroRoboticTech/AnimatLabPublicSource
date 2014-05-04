// RbFirmataPrismaticServo.h: interface for the RbFirmataPrismaticServo class.
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

class ROBOTICS_PORT RbFirmataPrismaticServo : public RbFirmataPart
{
protected:
    RbHinge *m_lpHinge;

public:
	RbFirmataPrismaticServo();
	virtual ~RbFirmataPrismaticServo();

	virtual void SetupIO();
	virtual void StepIO();

	virtual void Initialize();
    virtual void StepSimulation();
};

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

