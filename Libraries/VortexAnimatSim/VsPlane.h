// VsPlane.h: interface for the VsPlane class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSPLANE_H__76C24FAB_0AFC_4822_BCA0_D3055E824E6E__INCLUDED_)
#define AFX_VSPLANE_H__76C24FAB_0AFC_4822_BCA0_D3055E824E6E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class VORTEX_PORT VsPlane : public Plane, public VsRigidBody  
			{
			protected:

			public:
				VsPlane();
				virtual ~VsPlane();

				//virtual void Selected(BOOL bValue, BOOL bSelectMultiple); 

				//virtual void EnableCollision(Simulator *lpSim, RigidBody *lpBody);
				//virtual void DisableCollision(Simulator *lpSim, RigidBody *lpBody);

				virtual void CreateParts(Simulator *lpSim, Structure *lpStructure);
				//virtual void ResetSimulation(Simulator *lpSim, Structure *lpStructure);
			};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

#endif // !defined(AFX_VSPLANE_H__76C24FAB_0AFC_4822_BCA0_D3055E824E6E__INCLUDED_)
