/**
\file	VsEllipsoid.h

\brief	Declares the vortex ellipsoid class.
**/

#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class VORTEX_PORT VsEllipsoid : public AnimatSim::Environment::Bodies::Ellipsoid, public VsRigidBody
			{
			protected:

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void ResizePhysicsGeometry();

			public:
				VsEllipsoid();
				virtual ~VsEllipsoid();

				virtual void CreateParts();
				virtual void CreateJoints();
			};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
