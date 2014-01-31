// RbLinearHillStretchReceptor.h: interface for the VsMuscle class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

class ROBOTICS_PORT RbLinearHillStretchReceptor : public AnimatSim::Environment::Bodies::LinearHillStretchReceptor, public RbLine
{
protected:	

public:
	RbLinearHillStretchReceptor();
	virtual ~RbLinearHillStretchReceptor();

	virtual void CreateParts();
	virtual void CreateJoints();
	virtual void ResetSimulation();
	virtual void AfterResetSimulation();
	virtual void StepSimulation();
};

		}		//Bodies
	}			// Environment
}				//RoboticsAnimatSim
