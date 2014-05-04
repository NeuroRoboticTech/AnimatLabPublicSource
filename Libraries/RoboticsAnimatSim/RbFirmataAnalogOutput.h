// RbFirmataAnalogOutput.h: interface for the RbFirmataAnalogOutput class.
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

class ROBOTICS_PORT RbFirmataAnalogOutput : public RbFirmataPart
{
protected:

public:
	RbFirmataAnalogOutput();
	virtual ~RbFirmataAnalogOutput();

	virtual void SetupIO();
	virtual void StepIO();

	virtual void StepSimulation();
};

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

