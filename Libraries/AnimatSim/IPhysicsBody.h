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

			virtual void SetVisible(BOOL bVisible) = 0;
			virtual void SetAlpha() = 0;
			virtual void SetFreeze(BOOL bVal) = 0;
			virtual void SetDensity(float fltVal) = 0;

			virtual void Physics_UpdateMatrix() = 0;
			virtual void Physics_CollectBodyData() = 0;
			virtual void Physics_ResetSimulation() = 0;
			virtual float *Physics_GetDataPointer(string strDataType) = 0;
			virtual void Physics_Selected(BOOL bValue, BOOL bSelectMultiple) = 0;  
			virtual void Physics_EnableCollision(RigidBody *lpBody) = 0;
			virtual void Physics_DisableCollision(RigidBody *lpBody) = 0;
			virtual void Physics_AddBodyForce(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, BOOL bScaleUnits) = 0;
			virtual void Physics_AddBodyTorque(float fltTx, float fltTy, float fltTz, BOOL bScaleUnits) = 0;
			virtual CStdFPoint Physics_GetVelocityAtPoint(float x, float y, float z) = 0;
			virtual float Physics_GetMass() = 0;
			virtual float Physics_GetBoundingRadius() = 0;
			virtual BoundingBox Physics_GetBoundingBox() = 0;
			virtual void Physics_SetColor() = 0;
			virtual void Physics_TextureChanged() = 0;
		};

	}
}