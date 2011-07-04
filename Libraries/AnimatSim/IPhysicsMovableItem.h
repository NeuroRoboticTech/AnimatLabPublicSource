#pragma once

namespace AnimatSim
{
	namespace Environment
	{


		class ANIMAT_PORT IPhysicsMovableItem
		{
		protected:

		public:
			IPhysicsMovableItem(void);
			virtual ~IPhysicsMovableItem(void);

			virtual void SetVisible(BOOL bVisible) = 0;
			virtual void SetAlpha() = 0;

			virtual void Physics_UpdateMatrix() = 0;
			virtual void Physics_ResetGraphicsAndPhysics() = 0;
			virtual void Physics_PositionChanged() = 0;
			virtual void Physics_RotationChanged() = 0;
			virtual void Physics_UpdateAbsolutePosition() = 0;
			virtual void Physics_CollectData() = 0;
			virtual void Physics_ResetSimulation() = 0;
			virtual void Physics_AfterResetSimulation() = 0;
			virtual float *Physics_GetDataPointer(string strDataType) = 0;
			virtual void Physics_Selected(BOOL bValue, BOOL bSelectMultiple) = 0;  
			virtual float Physics_GetBoundingRadius() = 0;
			virtual BoundingBox Physics_GetBoundingBox() = 0;
			virtual void Physics_SetColor() = 0;
			virtual void Physics_TextureChanged() = 0;
			virtual void Physics_OrientNewPart(float fltXPos, float fltYPos, float fltZPos, float fltXNorm, float fltYNorm, float fltZNorm) = 0;
			virtual void Physics_SelectedVertex(float fltXPos, float fltYPos, float fltZPos) = 0;
			virtual BOOL Physics_CalculateLocalPosForWorldPos(float fltWorldX, float fltWorldY, float fltWorldZ, CStdFPoint &vLocalPos) = 0;
		};

	}
}