
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

			class BULLET_PORT BlBoxTest : public AnimatSim::Environment::Bodies::Box
			{
			protected:
				Vx::VxPart* part;
				osg::ref_ptr<osg::MatrixTransform> node;

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void ResizePhysicsGeometry();

			public:
				BlBoxTest();
				virtual ~BlBoxTest();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
