/**
\file	BlPlaneTest.h

\brief	Declares the vortex plane class.
**/

#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class BULLET_PORT BlPlaneTest : public AnimatSim::Environment::Bodies::Plane  
			{
			protected:
				Vx::VxPart* part;
				osg::ref_ptr<osg::MatrixTransform> node;

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void ResizePhysicsGeometry();

			public:
				BlPlaneTest();
				virtual ~BlPlaneTest();

				virtual void CreateParts();
				virtual void Physics_FluidDataChanged();
			};

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
