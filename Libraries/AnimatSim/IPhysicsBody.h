#pragma once

namespace AnimatSim
{
	namespace Environment
	{


		class ANIMAT_PORT IPhysicsBody
		{
		protected:

		public:
			IPhysicsBody(void);
			virtual ~IPhysicsBody(void);

            virtual BOOL Physics_IsDefined() = 0;
            virtual BOOL Physics_IsGeometryDefined() = 0;
			virtual void Physics_SetFreeze(BOOL bVal) = 0;
			virtual void Physics_SetDensity(float fltVal) = 0;
			virtual void Physics_SetMaterialID(string strID) = 0;
			virtual void Physics_SetVelocityDamping(float fltLinear, float fltAngular) = 0;
			virtual void Physics_SetCenterOfMass(float fltTx, float fltTy, float fltTz) = 0;

			virtual void Physics_UpdateNode() = 0;
			virtual void Physics_EnableCollision(RigidBody *lpBody) = 0;
			virtual void Physics_DisableCollision(RigidBody *lpBody) = 0;
			virtual void Physics_AddBodyForce(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, BOOL bScaleUnits) = 0;
			virtual void Physics_AddBodyTorque(float fltTx, float fltTy, float fltTz, BOOL bScaleUnits) = 0;
			virtual CStdFPoint Physics_GetVelocityAtPoint(float x, float y, float z) = 0;
			virtual float Physics_GetMass() = 0;
			virtual void Physics_ResizeSelectedReceptiveFieldVertex() = 0;
			virtual void Physics_FluidDataChanged() = 0;
			virtual BOOL Physics_HasCollisionGeometry() = 0;
		};

	}
}