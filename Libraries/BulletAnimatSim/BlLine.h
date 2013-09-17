// BlLine.h: interface for the BlLine class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{

		class BULLET_PORT BlLine : public BlRigidBody, public OsgLine
		{
		protected:
			//Remove the texture and culling options for the line.
			virtual void SetTexture(string strTexture) {};
			virtual void SetCulling() {};
			virtual void CreateGraphicsGeometry();
			virtual void CreatePhysicsGeometry();
			virtual void SetThisPointers();
			virtual void DeleteGraphics();

		public:
			BlLine();
			virtual ~BlLine();

			virtual void SetupGraphics();

			virtual void Initialize() {};
			virtual void ResetSimulation();
			virtual void AfterResetSimulation();
			virtual void StepSimulation(float fltTension);
			virtual void CreateParts();
		};

	}			// Visualization
}				//BulletAnimatSim
