// RbFirmataDigitalOutput.h: interface for the RbFirmataDigitalOutput class.
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

class ROBOTICS_PORT RbFirmataDigitalOutput : public RbFirmataPart
{
protected:

public:
	RbFirmataDigitalOutput();
	virtual ~RbFirmataDigitalOutput();

	virtual void SetupIO();
	virtual void StepIO(int iPartIdx);

	virtual void StepSimulation();
};

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

