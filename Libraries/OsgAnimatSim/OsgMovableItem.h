
#pragma once

#include "OsgGeometry.h"

namespace OsgAnimatSim
{
    class OsgSimulator;

	namespace Environment
	{

		class ANIMAT_OSG_PORT OsgMovableItem : public AnimatSim::Environment::IPhysicsMovableItem
		{
		protected:
			AnimatBase *m_lpThisAB;
			MovableItem *m_lpThisMI;
			OsgMovableItem *m_lpThisVsMI;
			OsgMovableItem *m_lpParentVsMI;
            OsgSimulator *m_lpOsgSim;

			osg::Matrix m_osgWorldMatrix;

			osg::ref_ptr<osg::Group> m_osgParent;
			osg::ref_ptr<osgManipulator::Selection> m_osgMT;
			osg::ref_ptr<osg::Geometry> m_osgGeometry;
			osg::ref_ptr<osg::Group> m_osgRoot;
			osg::ref_ptr<osg::Node> m_osgNode;
			osg::ref_ptr<osg::Group> m_osgNodeGroup;

			osg::ref_ptr<osg::CullFace> m_osgCull;
			osg::ref_ptr<osg::Texture2D> m_osgTexture;

			osg::ref_ptr<osg::StateSet> m_osgStateSet;
			osg::ref_ptr<osg::Material> m_osgMaterial;

			osg::ref_ptr<osg::Group> m_osgSelectedGroup;
			osg::ref_ptr<OsgDragger> m_osgDragger;

			osg::Matrix m_osgLocalMatrix;		

			/// Sometimes it is necessary to rotate the geometry that was generated to match the correct
			/// orientation of the physics geometry. If this MT is set then this is added 
			/// BEFORE the local matrix so we can make the graphics and physics geometries match. If
			/// it is not set then it is not used.
			osg::ref_ptr< osg::MatrixTransform> m_osgGeometryRotationMT;

			//Some parts, like joints, have an additional offset matrix. This
			//Final Matrix is the combination of the local and offset matrix. 
			osg::Matrix m_osgFinalMatrix;		
			BOOL m_bCullBackfaces;
			osg::StateAttribute::GLMode m_eTextureMode;

			osg::ref_ptr<osg::MatrixTransform> m_osgSelVertexMT;
			osg::ref_ptr<osg::Geode> m_osgSelVertexNode;

			virtual void SetThisPointers();
			virtual void LocalMatrix(osg::Matrix osgLocalMT);
			virtual void GeometryRotationMatrix(osg::Matrix osgGeometryMT);

			virtual void SetupGraphics();
			virtual void SetupPhysics() = 0;
			virtual void DeleteGraphics();
			virtual void DeletePhysics() = 0;
			virtual void CreateSelectedGraphics(string strName);
			virtual void CreateDragger(string strName);
			virtual void CreateSelectedVertex(string strName);
			virtual void DeleteSelectedVertex();
			virtual void AttachedPartMovedOrRotated(string strID);
			virtual void UpdatePositionAndRotationFromMatrix();
			virtual void UpdatePositionAndRotationFromMatrix(osg::Matrix osgMT) = 0;

			virtual void CreateGraphicsGeometry();
			virtual void CreatePhysicsGeometry();
			virtual void CreateGeometry();
			virtual void ResizePhysicsGeometry();

			virtual void UpdateWorldMatrix();

			virtual void ShowSelectedVertex();
			virtual void HideSelectedVertex();

		public:
			OsgMovableItem();
			virtual ~OsgMovableItem();

            virtual OsgSimulator *GetOsgSimulator() {return m_lpOsgSim;};
			virtual OsgMovableItem *VsParent();
			virtual osg::Group *ParentOSG() = 0;
			virtual osg::Group *RootGroup() {return m_osgRoot.get();};
			virtual osg::Group *NodeGroup() {return m_osgNodeGroup.get();};
			virtual osg::Matrix LocalMatrix() {return m_osgLocalMatrix;};
			virtual osg::Matrix FinalMatrix() {return m_osgFinalMatrix;};

			virtual void Physics_SetParent(MovableItem *lpParent)
			{
				m_lpParentVsMI = dynamic_cast<OsgMovableItem *>(lpParent);
			};
			virtual void Physics_SetChild(MovableItem *lpParent) {};


			//virtual CStdFPoint GetOSGWorldCoords(osg::MatrixTransform *osgMT);
			virtual CStdFPoint GetOSGWorldCoords();
			virtual osg::Matrix GetOSGWorldMatrix(BOOL bUpdate = FALSE);
			//virtual osg::Matrix GetOSGWorldMatrix(osg::MatrixTransform *osgMT);

			virtual void EndGripDrag();

			virtual string Physics_ID();
			virtual void Physics_UpdateMatrix();
			virtual void Physics_ResetGraphicsAndPhysics();
			virtual void Physics_PositionChanged();
			virtual void Physics_RotationChanged();
			virtual void Physics_UpdateAbsolutePosition();
			virtual void Physics_Selected(BOOL bValue, BOOL bSelectMultiple); 
			virtual float Physics_GetBoundingRadius();
			virtual BoundingBox Physics_GetBoundingBox();
			virtual void Physics_SetColor() {};
			virtual void Physics_TextureChanged() {};
			virtual void Physics_CollectData();
			virtual void Physics_ResetSimulation();
			virtual void Physics_AfterResetSimulation() {};
			virtual float *Physics_GetDataPointer(const string &strDataType);
			virtual void Physics_OrientNewPart(float fltXPos, float fltYPos, float fltZPos, float fltXNorm, float fltYNorm, float fltZNorm);
			virtual void Physics_SelectedVertex(float fltXPos, float fltYPos, float fltZPos) {};
			virtual BOOL Physics_CalculateLocalPosForWorldPos(float fltWorldX, float fltWorldY, float fltWorldZ, CStdFPoint &vLocalPos);
			virtual void Physics_LoadTransformMatrix(CStdXml &oXml);
			virtual void Physics_ResizeDragHandler(float fltRadius);

			virtual void SetTexture(string strTexture);
			virtual void SetCulling();
			virtual void SetColor(CStdColor &vAmbient, CStdColor &vDiffuse, CStdColor &vSpecular, float fltShininess);
			virtual void SetAlpha();
			virtual void SetMaterialAlpha(osg::Material *osgMat, osg::StateSet *ss, float fltAlpha);
			virtual void SetVisible(BOOL bVisible);
			virtual void SetVisible(osg::Node *osgNode, BOOL bVisible);
			//virtual osg::Vec3 FindPointOnSurface(osg::Vec3 vDirection);

			//virtual void Initialize() = 0;
			virtual void CreateItem();

			virtual osg::Matrix GetWorldMatrix();
			virtual osg::Matrix GetParentWorldMatrix();

			virtual osg::MatrixTransform* GetMatrixTransform();
			virtual osg::MatrixTransform* GetCameraMatrixTransform();

			virtual void BuildLocalMatrix();
			virtual void BuildLocalMatrix(CStdFPoint localPos, CStdFPoint localRot, string strName);
		};

	}			// Environment
}				//OsgAnimatSim


