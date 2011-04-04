// VsJoint.h: interface for the VsJoint class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSJOINT_H__93EDBBFE_2FA0_467C_970F_1775454FE94E__INCLUDED_)
#define AFX_VSJOINT_H__93EDBBFE_2FA0_467C_970F_1775454FE94E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
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

		class VORTEX_PORT VsJoint : public VsBody
		{
		protected:
			Vx::VxConstraint *m_vxJoint;
			Vx::VxConstraint::CoordinateID m_iCoordID;

			//This is the offset from the child to the parent. We must add the osg graphics to the parent osg node
			//because if we do not then the joint will move when the child moves, and that is not correct. However, 
			//the joint is really attached relative to the child. So I am adding this extra matrix transform that is
			//the same as the child body to make up for the fact that we have to attach to the parent osg node.
			osg::Matrix m_osgChildOffsetMatrix;
			osg::ref_ptr< osg::MatrixTransform> m_osgChildOffsetMT;

			Joint *m_lpThisJoint;

			void UpdatePosition();

			virtual void SetupGraphics();
			virtual void SetupPhysics();
			virtual void DeletePhysics() {};
			virtual VxVector3 NormalizeAxis(CStdFPoint vLocalRot);
			virtual void UpdatePositionAndRotationFromMatrix();

			virtual void LocalMatrix(osg::Matrix osgLocalMT);
			virtual void ChildOffsetMatrix(osg::Matrix osgMT);

		public:
			VsJoint();
			virtual ~VsJoint();

			virtual osg::Group *ParentOSG();
			virtual osg::Group *ChildOSG();

			virtual void Initialize();
			virtual void SetBody();
			virtual void Physics_ResetSimulation();
			virtual void Physics_CollectBodyData();
			virtual float *Physics_GetDataPointer(string strDataType);

			virtual Vx::VxConstraint* Constraint() {return m_vxJoint;};
			virtual Vx::VxConstraint::CoordinateID CoordinateID() {return m_iCoordID;};

			//Methods not used by joints.
			virtual void Physics_UpdateMatrix();
			virtual void BuildLocalMatrix(CStdFPoint localPos, CStdFPoint localRot, string strName);
			virtual void Physics_EnableCollision(RigidBody *lpBody) {};
			virtual void Physics_DisableCollision(RigidBody *lpBody) {};
			virtual void Physics_AddBodyForce(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, BOOL bScaleUnits) {};
			virtual void Physics_AddBodyTorque(float fltTx, float fltTy, float fltTz, BOOL bScaleUnits) {};
			virtual CStdFPoint Physics_GetVelocityAtPoint(float x, float y, float z) {CStdFPoint v; return v;};
			virtual float Physics_GetMass() {return 0;};

		};

	}			// Environment
}				//VortexAnimatSim

#endif // !defined(AFX_VSJOINT_H__93EDBBFE_2FA0_467C_970F_1775454FE94E__INCLUDED_)
