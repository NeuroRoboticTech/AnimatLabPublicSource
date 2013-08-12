// VsSpring.h: interface for the VsSpring class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

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

			class VORTEX_PORT VsSpring : public AnimatSim::Environment::Bodies::Spring, public VsLine     
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

				virtual void Enabled(bool bVal);
				virtual void NaturalLength(float fltVal, bool bUseScaling = true);
				virtual void Stiffness(float fltVal, bool bUseScaling = true);
				virtual void Damping(float fltVal, bool bUseScaling = true);

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
