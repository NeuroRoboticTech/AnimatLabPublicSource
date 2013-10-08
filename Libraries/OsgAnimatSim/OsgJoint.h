// OsgJoint.h: interface for the OsgJoint class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace OsgAnimatSim
{
	namespace Environment
	{

		/*! \brief 
			A common class for all joint data specific to vortex.

			\remarks
			This is a common class for the joint objects that 
			specifically holds vortex data and methods. The 
			reasoning behind this class is the same as that for
			the VsRigidBody. Please see that class for an explanation
			of why this class is needed.

			\sa
			RigidBody, CVsAlJoint
		*/

		class ANIMAT_OSG_PORT OsgJoint : public OsgBody
		{
		protected:

#pragma region DefaultBallGraphicsItems

			//Graphics objects for the default joint drawing code
			/// The osg default ball geometry.
			osg::ref_ptr<osg::Geometry> m_osgDefaultBall;

			/// The osg default ball matrix transform.
			osg::ref_ptr<osg::MatrixTransform> m_osgDefaultBallMT;

			/// The osg default ball material.
			osg::ref_ptr<osg::Material> m_osgDefaultBallMat;

			/// The osg default ball state set.
			osg::ref_ptr<osg::StateSet> m_osgDefaultBallSS;

			/// The osg joint matrix transform.
			osg::ref_ptr<osg::MatrixTransform> m_osgJointMT;

            ///Rotational offset needed to make the joint graphics match the physics.
            CStdFPoint m_vJointGraphicsRotOffset;

#pragma endregion

			Joint *m_lpThisJoint;
			OsgBody *m_lpVsParent;
			OsgBody *m_lpVsChild;

			virtual void SetThisPointers();

            virtual void ResetDraggerOnResize();
            virtual void DeleteJointGraphics();
            virtual void CreateJointGraphics();
			virtual osg::Vec3d NormalizeAxis(CStdFPoint vLocalRot);
			virtual void UpdatePositionAndRotationFromMatrix();

		public:
			OsgJoint();
			virtual ~OsgJoint();

			virtual osg::MatrixTransform *ParentOSG();
			virtual osg::MatrixTransform *ChildOSG();
            virtual osg::Matrix GetChildWorldMatrix();

			virtual void SetAlpha();

			virtual void SetupGraphics();
            virtual void DeleteGraphics();
			virtual void SetupPhysics();

            virtual void StartGripDrag();
            virtual void EndGripDrag();

			virtual void Initialize();
			virtual bool Physics_SetData(const std::string &strDataType, const std::string &strValue);
			virtual void Physics_QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);

			//Methods not used by joints.
            virtual bool Physics_IsGeometryDefined() {return true;};
			virtual void Physics_Resize();
			virtual void Physics_ResetSimulation();
			virtual void Physics_SetParent(MovableItem *lpParent);
			virtual void Physics_SetChild(MovableItem *lpChild);
			virtual void Physics_UpdateMatrix();
			virtual void Physics_PositionChanged();
			virtual void Physics_RotationChanged();
			virtual void BuildLocalMatrix();
			virtual void BuildLocalMatrix(CStdFPoint localPos, CStdFPoint localRot, std::string strName);
			virtual void Physics_EnableCollision(RigidBody *lpBody) {};
			virtual void Physics_DisableCollision(RigidBody *lpBody) {};
			virtual void Physics_AddBodyForce(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits) {};
			virtual void Physics_AddBodyTorque(float fltTx, float fltTy, float fltTz, bool bScaleUnits) {};
			virtual CStdFPoint Physics_GetVelocityAtPoint(float x, float y, float z) {CStdFPoint v; return v;};
			virtual float Physics_GetMass() {return 0;};
			//virtual bool Physics_CalculateLocalPosForWorldPos(float fltWorldX, float fltWorldY, float fltWorldZ, CStdFPoint &vLocalPos);

		};

	}			// Environment
}				//OsgAnimatSim
