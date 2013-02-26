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

			class VORTEX_PORT AnimatVxSpring : public Vx::VxSpring
			{
			public:
				AnimatVxSpring() : Vx::VxSpring() {};
				AnimatVxSpring(Vx::VxPart* part1, Vx::VxPart* part2, Vx::VxReal naturalLength, Vx::VxReal stiffness, Vx::VxReal damping)
					 : Vx::VxSpring(part1, part2, naturalLength,stiffness, damping) {};
				virtual ~AnimatVxSpring() {};
				
				void EnableBodies()
				{Vx::VxSpring::enableBodies();}
			};

			class VORTEX_PORT VsSpring : public VsLine, public AnimatSim::Environment::Bodies::Spring     
			{
			protected:
				//Vx::VxSpring *m_vxSpring;
				AnimatVxSpring *m_vxSpring;

				virtual void SetupPhysics();
				virtual void DeletePhysics();
				virtual void InitializeAttachments();

			public:
				VsSpring();
				virtual ~VsSpring();

				virtual void Enabled(BOOL bVal);
				virtual void NaturalLength(float fltVal, BOOL bUseScaling = TRUE);
				virtual void Stiffness(float fltVal, BOOL bUseScaling = TRUE);
				virtual void Damping(float fltVal, BOOL bUseScaling = TRUE);

				virtual void Physics_CollectData();
				virtual void Physics_Resize();

				virtual void CreateJoints();
				virtual void ResetSimulation();
				virtual void AfterResetSimulation();
				virtual void StepSimulation();
			};

		}		//Joints
	}			// Environment
}				//VortexAnimatSim

#endif // !defined(AFX_VSSPRINGJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
