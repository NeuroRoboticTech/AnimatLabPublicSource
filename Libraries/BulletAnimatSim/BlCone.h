// BlCone.h: interface for the BlCone class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

 
namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class BULLET_PORT BlCone : public AnimatSim::Environment::Bodies::Cone, public BlRigidBody
			{
			protected:

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
                virtual void CalculateVolumeAndAreas();

			public:
				BlCone();
				virtual ~BlCone();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
