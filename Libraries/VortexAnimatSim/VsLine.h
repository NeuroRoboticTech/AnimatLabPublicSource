// VsLine.h: interface for the VsLine class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{

		class VORTEX_PORT VsLine : public VsRigidBody, public OsgLine
		{
		protected:

			virtual void SetupGraphics();

			//Remove the texture and culling options for the line.
			virtual void SetTexture(string strTexture) {};
			virtual void SetCulling() {};
			virtual void CreateGraphicsGeometry();
			virtual void CreatePhysicsGeometry();
			virtual void SetThisPointers();
			virtual void DeleteGraphics();

		public:
			VsLine();
			virtual ~VsLine();

			virtual void Initialize() {};
			virtual void ResetSimulation();
			virtual void AfterResetSimulation();
			virtual void StepSimulation(float fltTension);
			virtual void CreateParts();
		};

	}			// Visualization
}				//VortexAnimatSim
