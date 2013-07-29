// VsLinearHillMuscle.h: interface for the VsMuscle class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

class VORTEX_PORT VsLinearHillMuscle : public AnimatSim::Environment::Bodies::LinearHillMuscle, public VsLine
{
protected:	

public:
	VsLinearHillMuscle();
	virtual ~VsLinearHillMuscle();

	virtual void CreateParts();
	virtual void CreateJoints();
	virtual void ResetSimulation();
	virtual void AfterResetSimulation();
	virtual void StepSimulation();
};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
