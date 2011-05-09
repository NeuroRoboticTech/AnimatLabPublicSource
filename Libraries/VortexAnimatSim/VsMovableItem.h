
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		BOOL VORTEX_PORT OsgMatricesEqual(osg::Matrix v1, osg::Matrix v2);
		osg::Quat VORTEX_PORT EulerToQuaternion(float fX, float fY, float fZ);
		CStdFPoint VORTEX_PORT QuaterionToEuler(osg::Quat vQuat);
		osg::Matrix VORTEX_PORT SetupMatrix(CStdFPoint &localPos, CStdFPoint &localRot);
		osg::Matrix VORTEX_PORT SetupMatrix(CStdFPoint &localPos, osg::Quat qRot);
		osg::Geometry VORTEX_PORT *CreateBoxGeometry(float xsize, float ysize, float zsize, float fltXSegWidth, float fltYSegWidth, float fltZSegWidth);
		osg::Geometry VORTEX_PORT *CreateConeGeometry(float height, float topradius, float botradius, int sides, bool doSide, bool doTop, bool doBottom);
		osg::Geometry VORTEX_PORT *CreateSphereGeometry(int latres, int longres, float radius);
		osg::Geometry VORTEX_PORT *CreateEllipsoidGeometry(int latres, int longres, float rSemiMajorAxis, float rSemiMinorAxis);
		osg::Geometry VORTEX_PORT *CreatePlaneGeometry(float fltCornerX, float fltCornerY, float fltXSize, float fltYSize, float fltXGrid, float fltYGrid);
		osg::MatrixTransform VORTEX_PORT *CreateLinearAxis(float fltGripScale, CStdFPoint vRotAxis);
		osg::Geode VORTEX_PORT *CreateCircle( int plane, int approx, float radius, float width );
		osg::Vec3Array VORTEX_PORT *CreateCircleVerts( int plane, int approx, float radius );
		osg::Geometry VORTEX_PORT *CreateTorusGeometry(float innerRadius, float outerRadius, int sides, int rings);


		class VORTEX_PORT VsMovableItem : public AnimatSim::Environment::IPhysicsMovableItem
		{
		protected:
			AnimatBase *m_lpThisAB;
			MovableItem *m_lpThisMI;

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
			osg::ref_ptr<VsDragger> m_osgDragger;

			osg::Matrix m_osgLocalMatrix;		

			//Some parts, like joints, have an additional offset matrix. This
			//Final Matrix is the combination of the local and offset matrix. 
			osg::Matrix m_osgFinalMatrix;		
			BOOL m_bCullBackfaces;
			osg::StateAttribute::GLMode m_eTextureMode;

			virtual void SetThisPointers();
			virtual void LocalMatrix(osg::Matrix osgLocalMT);

			virtual void SetupGraphics();
			virtual void SetupPhysics() = 0;
			virtual void DeleteGraphics();
			virtual void DeletePhysics() {};
			virtual void CreateSelectedGraphics(string strName);
			virtual void CreateDragger(string strName);
			virtual void AttachedPartMovedOrRotated(string strID);
			virtual void UpdatePositionAndRotationFromMatrix();
			virtual void UpdateAbsolutePosition();

		public:
			VsMovableItem();
			virtual ~VsMovableItem();

			virtual osg::Group *ParentOSG() = 0;
			virtual osg::Group *RootGroup() {return m_osgRoot.get();};
			virtual osg::Group *NodeGroup() {return m_osgNodeGroup.get();};
			virtual osg::Matrix LocalMatrix() {return m_osgLocalMatrix;};
			virtual osg::Matrix FinalMatrix() {return m_osgFinalMatrix;};

			virtual CStdFPoint GetOSGWorldCoords(osg::MatrixTransform *osgMT);
			virtual CStdFPoint GetOSGWorldCoords();
			virtual osg::Matrix GetOSGWorldMatrix();
			virtual osg::Matrix GetOSGWorldMatrix(osg::MatrixTransform *osgMT);

			virtual void EndGripDrag();

			virtual string Physics_ID();
			virtual void Physics_UpdateMatrix();
			virtual void Physics_ResetGraphicsAndPhysics();
			virtual void Physics_PositionChanged();
			virtual void Physics_RotationChanged();
			virtual void Physics_Selected(BOOL bValue, BOOL bSelectMultiple); 
			virtual float Physics_GetBoundingRadius();
			virtual BoundingBox Physics_GetBoundingBox();
			virtual void Physics_SetColor() {};
			virtual void Physics_TextureChanged() {};
			virtual void Physics_CollectData();
			virtual void Physics_ResetSimulation();
			virtual void Physics_AfterResetSimulation() {};
			virtual float *Physics_GetDataPointer(string strDataType);

			virtual void SetTexture(string strTexture);
			virtual void SetCulling();
			virtual void SetColor(CStdColor &vAmbient, CStdColor &vDiffuse, CStdColor &vSpecular, float fltShininess);
			virtual void SetAlpha();
			virtual void SetMaterialAlpha(osg::Material *osgMat, osg::StateSet *ss, float fltAlpha);
			virtual void SetVisible(BOOL bVisible);
			virtual void SetVisible(osg::Node *osgNode, BOOL bVisible);

			//virtual void Initialize() = 0;
			virtual void CreateItem();

			virtual osg::MatrixTransform* GetMatrixTransform();
			virtual void BuildLocalMatrix();
			virtual void BuildLocalMatrix(CStdFPoint localPos, CStdFPoint localRot, string strName);
			virtual void WorldToBodyCoords(VxReal3 vWorldPos, StdVector3 &vLocalPos);
		};

	}			// Environment
}				//VortexAnimatSim


