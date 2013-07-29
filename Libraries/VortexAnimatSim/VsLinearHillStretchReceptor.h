// VsLinearHillStretchReceptor.h: interface for the VsMuscle class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

class VORTEX_PORT VsLinearHillStretchReceptor : public AnimatSim::Environment::Bodies::LinearHillStretchReceptor, public VsLine
{
protected:	

public:
	VsLinearHillStretchReceptor();
	virtual ~VsLinearHillStretchReceptor();

	virtual void CreateParts();
	virtual void CreateJoints();
	virtual void ResetSimulation();
	virtual void AfterResetSimulation();
	virtual void StepSimulation();
};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
