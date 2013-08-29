
#pragma once


namespace BulletAnimatSim
{
	namespace Environment
	{

		/**
		\namespace	BulletAnimatSim::Environment::Bodies

		\brief	Body part classes that use the vortex physics engine. 
		**/
		namespace Bodies
		{

			class BULLET_PORT BlBox : public AnimatSim::Environment::Bodies::Box, public BlRigidBody
			{
			protected:

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void ResizePhysicsGeometry();

                virtual void CreateDynamicPart();

			public:
				BlBox();
				virtual ~BlBox();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
