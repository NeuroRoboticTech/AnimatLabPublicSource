#pragma once

namespace AnimatSim
{
	namespace Environment
	{


		class ANIMAT_PORT IPhysicsBody : public IPhysicsBase
		{
		protected:

		public:
			IPhysicsBody(void);
			virtual ~IPhysicsBody(void);

			virtual void SetFreeze(BOOL bVal) = 0;
			virtual void SetDensity(float fltVal) = 0;

			virtual void Physics_UpdateNode() = 0;
			virtual void Physics_EnableCollision(RigidBody *lpBody) = 0;
			virtual void Physics_DisableCollision(RigidBody *lpBody) = 0;
			virtual void Physics_AddBodyForce(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, BOOL bScaleUnits) = 0;
			virtual void Physics_AddBodyTorque(float fltTx, float fltTy, float fltTz, BOOL bScaleUnits) = 0;
			virtual CStdFPoint Physics_GetVelocityAtPoint(float x, float y, float z) = 0;
			virtual float Physics_GetMass() = 0;
			virtual void Physics_TextureChanged() = 0;
		};

	}
}