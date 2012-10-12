
#pragma once


namespace VortexAnimatSim
{
	namespace Environment
	{

		/**
		\namespace	VortexAnimatSim::Environment::Bodies

		\brief	Body part classes that use the vortex physics engine. 
		**/
		namespace Bodies
		{

			class VORTEX_PORT VsBoxTest : public AnimatSim::Environment::Bodies::Box
			{
			protected:
				Vx::VxPart* part;
				osg::ref_ptr<osg::MatrixTransform> node;

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void ResizePhysicsGeometry();

			public:
				VsBoxTest();
				virtual ~VsBoxTest();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
