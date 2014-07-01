// RbFirmataPWMOutput.h: interface for the RbFirmataPWMOutput class.
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

class ROBOTICS_PORT RbFirmataPWMOutput : public RbFirmataPart
{
protected:

public:
	RbFirmataPWMOutput();
	virtual ~RbFirmataPWMOutput();

	virtual void SetupIO();
	virtual void StepIO(int iPartIdx);

	virtual void StepSimulation();
};

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

