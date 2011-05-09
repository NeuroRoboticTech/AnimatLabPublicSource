// VsLinearHillMuscle.h: interface for the VsMuscle class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSLINEAR_HILL_MUSCLE_H__8438B067_2454_459B_8092_E74ABF23B265__INCLUDED_)
#define AFX_VSLINEAR_HILL_MUSCLE_H__8438B067_2454_459B_8092_E74ABF23B265__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

class VORTEX_PORT VsLinearHillMuscle : public AnimatSim::Environment::Bodies::LinearHillMuscle, public VsLine
{
protected:	
	virtual void SetThisPointers();

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

#endif // !defined(AFX_VSLINEAR_HILL_MUSCLE_H__8438B067_2454_459B_8092_E74ABF23B265__INCLUDED_)
