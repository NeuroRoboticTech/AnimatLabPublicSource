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

			class VORTEX_PORT VsSpring : public VsLine, public AnimatSim::Environment::Bodies::Spring     
			{
			protected:
				Vx::VxSpring *m_vxSpring;

			public:
				VsSpring();
				virtual ~VsSpring();

				virtual void Enabled(BOOL bVal);

				virtual void Physics_CollectData();

				virtual void CreateParts();
				virtual void CreateJoints();
				virtual void ResetSimulation();
				virtual void AfterResetSimulation();
				virtual void StepSimulation();
			};

		}		//Joints
	}			// Environment
}				//VortexAnimatSim

#endif // !defined(AFX_VSSPRINGJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
