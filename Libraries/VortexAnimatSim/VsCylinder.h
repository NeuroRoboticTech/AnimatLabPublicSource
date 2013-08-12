// VsCylinder.h: interface for the VsCylinder class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class VORTEX_PORT VsCylinder : public AnimatSim::Environment::Bodies::Cylinder, public VsRigidBody
			{
			protected:

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void ResizePhysicsGeometry();

			public:
				VsCylinder();
				virtual ~VsCylinder();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
