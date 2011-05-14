/**
\file	VsPlane.h

\brief	Declares the vortex plane class.
**/

#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class VORTEX_PORT VsPlane : public AnimatSim::Environment::Bodies::Plane, public VsRigidBody  
			{
			protected:
				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void ResizePhysicsGeometry();

			public:
				VsPlane();
				virtual ~VsPlane();

				virtual void CreateParts();
			};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
