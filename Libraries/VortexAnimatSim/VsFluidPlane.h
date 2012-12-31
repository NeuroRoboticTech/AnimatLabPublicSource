/**
\file	VsFluidPlane.h

\brief	Declares the vortex fluid plane class.
**/

#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			class VORTEX_PORT VsFluidPlane : public AnimatSim::Environment::Bodies::FluidPlane, public VsRigidBody
			{
			protected:
				VxPlanarFluidState *m_vxFluidPlane;

				virtual void SetupPhysics();
				virtual void DeletePhysics();

				virtual void CreateGraphicsGeometry();
				virtual void CreatePhysicsGeometry();
				virtual void ResizePhysicsGeometry();
				virtual void UpdateFluidPlaneHeight();

				virtual void SetGravity();

			public:
				VsFluidPlane();
				virtual ~VsFluidPlane();

				virtual void Position(CStdFPoint &oPoint, BOOL bUseScaling = TRUE, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);
				virtual void Velocity(CStdFPoint &oPoint, BOOL bUseScaling = TRUE);

				virtual void CreateParts();
				virtual void Physics_SetDensity(float fltVal);
				virtual void Physics_FluidDataChanged();
				virtual void Physics_PositionChanged();
			};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
