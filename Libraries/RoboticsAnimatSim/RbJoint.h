// RbJoint.h: interface for the RbJoint class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		class RbRigidBody;

		/*! \brief 
			A common class for all joint data specific to vortex.

			\remarks
			This is a common class for the joint objects that 
			specifically holds vortex data and methods. The 
			reasoning behind this class is the same as that for
			the RbRigidBody. Please see that class for an explanation
			of why this class is needed.

			\sa
			RigidBody, CVsAlJoint
		*/

		class ROBOTICS_PORT RbJoint : public RbBody
		{
		protected:
            RbSimulator *m_lpRbSim;
            float m_fltPrevBtJointPos;
            float m_fltPrevJointPos;

			Joint *m_lpThisJoint;
            RbRigidBody *m_lpRbParent;
            RbRigidBody *m_lpRbChild;

			virtual void SetThisPointers();
			virtual RbSimulator *GetRbSimulator();

		public:
			RbJoint();
			virtual ~RbJoint();

            virtual RbRigidBody *GetRbParent() {return m_lpRbParent;};
            virtual RbRigidBody *GetRbChild() {return m_lpRbChild;};

            virtual bool Physics_IsDefined() {return true;};
            virtual bool Physics_IsGeometryDefined() {return true;};
            virtual void Physics_EnableCollision(AnimatSim::Environment::RigidBody *lpBody) {};
            virtual void Physics_DisableCollision(AnimatSim::Environment::RigidBody *lpBody) {};
            virtual void Physics_AddBodyForceAtLocalPos(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits) {};
            virtual void Physics_AddBodyForceAtWorldPos(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits) {};
            virtual void Physics_AddBodyTorque(float fltTx, float fltTy, float fltTz, bool bScaleUnits) {};
            virtual CStdFPoint Physics_GetVelocityAtPoint(float x, float y, float z) {CStdFPoint vVel; return vVel;};

            virtual void Physics_ResetSimulation();
            virtual void Physics_CollectData();
            virtual void SetConstraintFriction() {};
		};

	}			// Environment
}				//RoboticsAnimatSim
