/**
\file	BlFluidPlane.h

\brief	Declares the vortex fluid plane class.
**/

#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class BULLET_PORT BlFluidPlane : public AnimatSim::Environment::Bodies::FluidPlane, public BlRigidBody
			{
			protected:
                //FIX PHYSICS
				//VxPlanarFluidState *m_vxFluidPlane;

				virtual void SetupPhysics();
				virtual void DeletePhysics(bool bIncludeChildren);

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void ResizePhysicsGeometry();
				virtual void UpdateFluidPlaneHeight();

				virtual void SetGravity();

			public:
				BlFluidPlane();
				virtual ~BlFluidPlane();

				virtual void Position(CStdFPoint &oPoint, bool bUseScaling = true, bool bFireChangeEvent = false, bool bUpdateMatrix = true);
				virtual void Velocity(CStdFPoint &oPoint, bool bUseScaling = true);

				virtual void CreateParts();
				virtual void Physics_SetDensity(float fltVal);
				virtual void Physics_FluidDataChanged();
				virtual void Physics_PositionChanged();
			};

		}		//Bodies
	}			// Environment
}				//BulletAnimatSim
