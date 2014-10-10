// BlSphere.h: interface for the BlSphere class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class BULLET_PORT BlSphere : public AnimatSim::Environment::Bodies::Sphere, public BlRigidBody
			{
			protected:

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
                virtual void CalculateVolumeAndAreas();

			public:
				BlSphere();
				virtual ~BlSphere();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
