/**
\file	BlPlane.h

\brief	Declares the vortex plane class.
**/

#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class BULLET_PORT BlPlane : public AnimatSim::Environment::Bodies::Plane, public BlRigidBody  
			{
			protected:
				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void ResizePhysicsGeometry();

                virtual void CreateDynamicPart();

            public:
				BlPlane();
				virtual ~BlPlane();

				virtual void CreateParts();
				virtual void Physics_FluidDataChanged();
			};

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
