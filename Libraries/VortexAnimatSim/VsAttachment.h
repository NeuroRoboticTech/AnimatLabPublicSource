// VsAttachment.h: interface for the VsAttachment class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSMUSCLE_ATTACHMENT_H__8438B067_2454_459B_8092_E74ABF23B265__INCLUDED_)
#define AFX_VSMUSCLE_ATTACHMENT_H__8438B067_2454_459B_8092_E74ABF23B265__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

class VORTEX_PORT VsAttachment  : public Attachment, public VsRigidBody
{
protected:

public:
	VsAttachment();
	virtual ~VsAttachment();

	virtual void Initialize(Simulator *lpSim, Structure *lpStructure) {};
	virtual void CreateParts(Simulator *lpSim, Structure *lpStructure);
	//virtual void ResetSimulation(Simulator *lpSim, Structure *lpStructure);
	//virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
	//virtual float *GetDataPointer(string strDataType);
};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

#endif // !defined(AFX_VSMUSCLE_ATTACHMENT_H__8438B067_2454_459B_8092_E74ABF23B265__INCLUDED_)
