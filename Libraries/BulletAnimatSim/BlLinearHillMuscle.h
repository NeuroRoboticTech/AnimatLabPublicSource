// BlLinearHillMuscle.h: interface for the VsMuscle class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

class BULLET_PORT BlLinearHillMuscle : public AnimatSim::Environment::Bodies::LinearHillMuscle, public BlLine
{
protected:	

public:
	BlLinearHillMuscle();
	virtual ~BlLinearHillMuscle();

	virtual void CreateParts();
	virtual void CreateJoints();
	virtual void ResetSimulation();
	virtual void AfterResetSimulation();
	virtual void StepSimulation();
};

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
