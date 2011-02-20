// VsBox.h: interface for the VsBox class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSBOX_H__F74F0855_9701_4D03_82C4_EA3E5755910A__INCLUDED_)
#define AFX_VSBOX_H__F74F0855_9701_4D03_82C4_EA3E5755910A__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class VORTEX_PORT VsBox : public Box, public VsRigidBody
			{
			protected:

			public:
				VsBox();
				virtual ~VsBox();

				//virtual void Selected(BOOL bValue, BOOL bSelectMultiple); 

				virtual void CreateParts(Simulator *lpSim, Structure *lpStructure);
				virtual void CreateJoints(Simulator *lpSim, Structure *lpStructure);
				virtual void Resize();

				//virtual void ResetSimulation(Simulator *lpSim, Structure *lpStructure);
				//virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
				//virtual float *GetDataPointer(string strDataType);

				//virtual void EnableCollision(Simulator *lpSim, RigidBody *lpBody);
				//virtual void DisableCollision(Simulator *lpSim, RigidBody *lpBody);

				//virtual void AddForce(Simulator *lpSim, float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, BOOL bScaleUnits);
				//virtual void AddTorque(Simulator *lpSim, float fltTx, float fltTy, float fltTz, BOOL bScaleUnits);
				//virtual CStdFPoint GetVelocityAtPoint(float x, float y, float z);
				//virtual float GetMass();
			};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

#endif // !defined(AFX_VSBOX_H__F74F0855_9701_4D03_82C4_EA3E5755910A__INCLUDED_)
