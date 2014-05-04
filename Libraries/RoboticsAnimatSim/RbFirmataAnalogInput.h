// RbFirmataAnalogInput.h: interface for the RbFirmataAnalogInput class.
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

class ROBOTICS_PORT RbFirmataAnalogInput : public RbFirmataPart
{
protected:

public:
	RbFirmataAnalogInput();
	virtual ~RbFirmataAnalogInput();

	virtual void SetupIO();
	virtual void StepIO();

    virtual void StepSimulation();
};

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

