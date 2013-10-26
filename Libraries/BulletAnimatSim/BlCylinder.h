// BlCylinder.h: interface for the BlCylinder class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class BULLET_PORT BlCylinder : public AnimatSim::Environment::Bodies::Cylinder, public BlRigidBody
			{
			protected:

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
                virtual void CalculateVolumeAndAreas();

			public:
				BlCylinder();
				virtual ~BlCylinder();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
