// RbSpring.h: interface for the RbSpring class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class ROBOTICS_PORT RbSpring : public AnimatSim::Environment::Bodies::Spring, public RbLine     
			{
			protected:

			public:
				RbSpring();
				virtual ~RbSpring();

				virtual void CreateJoints();
				virtual void ResetSimulation();
				virtual void AfterResetSimulation();
				virtual void StepSimulation();
			};

		}		//Joints
	}			// Environment
}				//RoboticsAnimatSim
