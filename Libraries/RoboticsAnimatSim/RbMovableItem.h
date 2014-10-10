
#pragma once

namespace RoboticsAnimatSim
{
    class RbSimulator;

	namespace Environment
	{

		class ROBOTICS_PORT RbMovableItem : public AnimatSim::Environment::IPhysicsMovableItem
		{
		protected:
			AnimatBase *m_lpThisAB;
			MovableItem *m_lpThisMI;
			RbMovableItem *m_lpThisRbMI;
			RbMovableItem *m_lpParentRbMI;
            RbSimulator *m_lpRbSim;

            /// Used to report back nulls.
            float m_fltReportNull;

			virtual void SetThisPointers();

		public:
			RbMovableItem();
			virtual ~RbMovableItem();

            virtual RbSimulator *GetRbSimulator();
			virtual RbMovableItem *RbParent();

			virtual void Physics_SetParent(MovableItem *lpParent)
			{
				m_lpParentRbMI = dynamic_cast<RbMovableItem *>(lpParent);
			};
			virtual void Physics_SetChild(MovableItem *lpParent) {};

			virtual std::string Physics_ID();
			virtual void Physics_UpdateMatrix();
			virtual void Physics_ResetGraphicsAndPhysics();
			virtual void Physics_PositionChanged();
			virtual void Physics_RotationChanged();
			virtual void Physics_UpdateAbsolutePosition();
			virtual void Physics_Selected(bool bValue, bool bSelectMultiple); 
			virtual float Physics_GetBoundingRadius();
			virtual BoundingBox Physics_GetBoundingBox();
			virtual void Physics_SetColor() {};
			virtual void Physics_TextureChanged() {};
			virtual void Physics_CollectData();
            virtual void Physics_CollectExtraData() {};
			virtual void Physics_ResetSimulation();
			virtual void Physics_AfterResetSimulation() {};
			virtual float *Physics_GetDataPointer(const std::string &strDataType);
			virtual void Physics_OrientNewPart(float fltXPos, float fltYPos, float fltZPos, float fltXNorm, float fltYNorm, float fltZNorm);
			virtual void Physics_SelectedVertex(float fltXPos, float fltYPos, float fltZPos) {};
			virtual bool Physics_CalculateLocalPosForWorldPos(float fltWorldX, float fltWorldY, float fltWorldZ, CStdFPoint &vLocalPos);
			virtual void Physics_LoadLocalTransformMatrix(CStdXml &oXml);
			virtual void Physics_SaveLocalTransformMatrix(CStdXml &oXml);
			virtual std::string Physics_GetLocalTransformMatrixString();
			virtual void Physics_ResizeDragHandler(float fltRadius);
            virtual void Physics_Resize(void) {};

            virtual void SetVisible(bool) {};
            virtual void SetAlpha() {};

            virtual void CreateItem();
        };

	}			// Environment
}				//RoboticsAnimatSim


