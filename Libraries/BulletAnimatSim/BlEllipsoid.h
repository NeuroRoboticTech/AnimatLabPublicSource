/**
\file	BlEllipsoid.h

\brief	Declares the vortex ellipsoid class.
**/

#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class BULLET_PORT BlEllipsoid : public AnimatSim::Environment::Bodies::Ellipsoid, public BlRigidBody
			{
			protected:

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
                virtual void CalculateVolumeAndAreas();

			public:
				BlEllipsoid();
				virtual ~BlEllipsoid();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
