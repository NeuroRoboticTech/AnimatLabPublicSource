/**
\file	VsPlaneTest.h

\brief	Declares the vortex plane class.
**/

#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class VORTEX_PORT VsPlaneTest : public AnimatSim::Environment::Bodies::Plane  
			{
			protected:
				Vx::VxPart* part;
				osg::ref_ptr<osg::MatrixTransform> node;

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void ResizePhysicsGeometry();

			public:
				VsPlaneTest();
				virtual ~VsPlaneTest();

				virtual void CreateParts();
				virtual void Physics_FluidDataChanged();
			};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
