#pragma once

namespace AnimatSim
{
	namespace Environment
	{


		class ANIMAT_PORT IPhysicsBase
		{
		protected:

		public:
			IPhysicsBase(void);
			virtual ~IPhysicsBase(void);

			virtual void SetVisible(BOOL bVisible) = 0;
			virtual void SetAlpha() = 0;

			virtual void Physics_UpdateMatrix() = 0;
			virtual void Physics_CollectData() = 0;
			virtual void Physics_ResetSimulation() = 0;
			virtual void Physics_AfterResetSimulation() = 0;
			virtual float *Physics_GetDataPointer(string strDataType) = 0;
			virtual void Physics_Selected(BOOL bValue, BOOL bSelectMultiple) = 0;  
			virtual float Physics_GetBoundingRadius() = 0;
			virtual BoundingBox Physics_GetBoundingBox() = 0;
			virtual void Physics_SetColor() = 0;
		};

	}
}