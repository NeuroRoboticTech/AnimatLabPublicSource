// BlSpring.h: interface for the BlSpring class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class BULLET_PORT AnimatVxSpring : public Vx::VxSpring
			{
			public:
				AnimatVxSpring() : Vx::VxSpring() {};
				AnimatVxSpring(Vx::VxPart* part1, Vx::VxPart* part2, Vx::VxReal naturalLength, Vx::VxReal stiffness, Vx::VxReal damping)
					 : Vx::VxSpring(part1, part2, naturalLength,stiffness, damping) {};
				virtual ~AnimatVxSpring() {};
				
				void EnableBodies()
				{Vx::VxSpring::enableBodies();}
			};

			class BULLET_PORT BlSpring : public AnimatSim::Environment::Bodies::Spring, public BlLine     
			{
			protected:
				//Vx::VxSpring *m_vxSpring;
				AnimatVxSpring *m_vxSpring;

				virtual void SetupPhysics();
				virtual void DeletePhysics();
				virtual void InitializeAttachments();

			public:
				BlSpring();
				virtual ~BlSpring();

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
}				//BulletAnimatSim
