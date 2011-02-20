// VsSpring.h: interface for the VsSpring class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSSPRINGJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
#define AFX_VSSPRINGJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class VORTEX_PORT VsSpring : public VsLine, public Spring     
			{
			protected:
				Vx::VxSpring *m_vxSpring;

				virtual void CollectBodyData(Simulator *lpSim);

			public:
				VsSpring();
				virtual ~VsSpring();

				virtual void Enabled(BOOL bVal);

				virtual void CreateParts(Simulator *lpSim, Structure *lpStructure);
				virtual void CreateJoints(Simulator *lpSim, Structure *lpStructure);
				virtual float *GetDataPointer(string strDataType);
				virtual void ResetSimulation(Simulator *lpSim, Structure *lpStructure);
				virtual void AfterResetSimulation(Simulator *lpSim, Structure *lpStructure);
				virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
			};

		}		//Joints
	}			// Environment
}				//VortexAnimatSim

#endif // !defined(AFX_VSSPRINGJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
