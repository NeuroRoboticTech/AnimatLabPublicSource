// BlLinearHillStretchReceptor.h: interface for the VsMuscle class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

class BULLET_PORT BlLinearHillStretchReceptor : public AnimatSim::Environment::Bodies::LinearHillStretchReceptor, public BlLine
{
protected:	

public:
	BlLinearHillStretchReceptor();
	virtual ~BlLinearHillStretchReceptor();

	virtual void CreateParts();
	virtual void CreateJoints();
	virtual void ResetSimulation();
	virtual void AfterResetSimulation();
	virtual void StepSimulation();
};

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
