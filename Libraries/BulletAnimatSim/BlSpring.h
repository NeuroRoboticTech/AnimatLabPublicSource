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

			class BULLET_PORT BlSpring : public AnimatSim::Environment::Bodies::Spring, public BlLine     
			{
			protected:

			public:
				BlSpring();
				virtual ~BlSpring();

				virtual void CreateJoints();
				virtual void ResetSimulation();
				virtual void AfterResetSimulation();
				virtual void StepSimulation();
			};

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
