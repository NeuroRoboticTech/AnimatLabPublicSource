// RbLine.h: interface for the RbLine class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{

		class ROBOTICS_PORT RbLine : public RbRigidBody
		{
		protected:
			virtual void SetThisPointers();

		public:
			RbLine();
			virtual ~RbLine();

			virtual void Initialize() {};
			virtual void ResetSimulation();
			virtual void AfterResetSimulation();
			virtual void StepSimulation(float fltTension);
			virtual void CreateParts();
		};

	}			// Visualization
}				//RoboticsAnimatSim
