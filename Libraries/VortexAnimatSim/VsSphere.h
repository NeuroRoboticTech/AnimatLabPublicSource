// VsSphere.h: interface for the VsSphere class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class VORTEX_PORT VsSphere : public AnimatSim::Environment::Bodies::Sphere, public VsRigidBody
			{
			protected:

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void ResizePhysicsGeometry();

			public:
				VsSphere();
				virtual ~VsSphere();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
