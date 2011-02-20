// VsSphere.h: interface for the VsSphere class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSSPHERE_H__B6B13C0B_D733_44AF_917D_372FE21A4A2D__INCLUDED_)
#define AFX_VSSPHERE_H__B6B13C0B_D733_44AF_917D_372FE21A4A2D__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class VORTEX_PORT VsSphere : public Sphere, public VsRigidBody
			{
			protected:
				Vx::VxSphere *m_cgSphere;

			public:
				VsSphere();
				virtual ~VsSphere();

				//virtual void Selected(BOOL bValue, BOOL bSelectMultiple); 

				virtual void CreateParts(Simulator *lpSim, Structure *lpStructure);
				virtual void CreateJoints(Simulator *lpSim, Structure *lpStructure);
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

#endif // !defined(AFX_VSSPHERE_H__B6B13C0B_D733_44AF_917D_372FE21A4A2D__INCLUDED_)
