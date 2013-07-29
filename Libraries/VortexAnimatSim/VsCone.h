// VsCone.h: interface for the VsCone class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

 
namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class VORTEX_PORT VsCone : public AnimatSim::Environment::Bodies::Cone, public VsRigidBody
			{
			protected:

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void ResizePhysicsGeometry();

			public:
				VsCone();
				virtual ~VsCone();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
