// VsLinearHillStretchReceptor.h: interface for the VsMuscle class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSLINEAR_HILL_STRETCH_RECEPTOR_H__8438B067_2454_459B_8092_E74ABF23B265__INCLUDED_)
#define AFX_VSLINEAR_HILL_STRETCH_RECEPTOR_H__8438B067_2454_459B_8092_E74ABF23B265__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

class VORTEX_PORT VsLinearHillStretchReceptor : public LinearHillStretchReceptor, public VsLine
{
protected:	

public:
	VsLinearHillStretchReceptor();
	virtual ~VsLinearHillStretchReceptor();

	virtual void CreateParts(Simulator *lpSim, Structure *lpStructure);
	virtual void CreateJoints(Simulator *lpSim, Structure *lpStructure);
	virtual void ResetSimulation(Simulator *lpSim, Structure *lpStructure);
	virtual void AfterResetSimulation(Simulator *lpSim, Structure *lpStructure);
	virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

#endif // !defined(AFX_VSLINEAR_HILL_STRETCH_RECEPTOR_H__8438B067_2454_459B_8092_E74ABF23B265__INCLUDED_)
