// RbFirmataDigitalInput.h: interface for the RbFirmataDigitalInput class.
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

class ROBOTICS_PORT RbFirmataDigitalInput : public RbFirmataPart
{
protected:

public:
	RbFirmataDigitalInput();
	virtual ~RbFirmataDigitalInput();

	virtual void SetupIO();
	virtual void StepIO();

	virtual void StepSimulation();
};

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

