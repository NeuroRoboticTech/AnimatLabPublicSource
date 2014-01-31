// RbLinearHillMuscle.h: interface for the VsMuscle class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

class ROBOTICS_PORT RbLinearHillMuscle : public AnimatSim::Environment::Bodies::LinearHillMuscle, public RbLine
{
protected:	

public:
	RbLinearHillMuscle();
	virtual ~RbLinearHillMuscle();

	virtual void CreateParts();
	virtual void CreateJoints();
	virtual void ResetSimulation();
	virtual void AfterResetSimulation();
	virtual void StepSimulation();
};

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
