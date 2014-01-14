// BlJoint.h: interface for the BlJoint class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		class BlRigidBody;

		/*! \brief 
			A common class for all joint data specific to vortex.

			\remarks
			This is a common class for the joint objects that 
			specifically holds vortex data and methods. The 
			reasoning behind this class is the same as that for
			the BlRigidBody. Please see that class for an explanation
			of why this class is needed.

			\sa
			RigidBody, CVsAlJoint
		*/

		class BULLET_PORT BlJoint : public OsgJoint
		{
		protected:
			btTypedConstraint *m_btJoint;
            BlSimulator *m_lpVsSim;
            float m_fltPrevBtJointPos;
            float m_fltPrevJointPos;

            BlRigidBody *m_lpBlParent;
            BlRigidBody *m_lpBlChild;

            btRigidBody *m_btParent;
            btRigidBody *m_btChild;

			virtual BlSimulator *GetBlSimulator();

            virtual void CalculateRelativeJointMatrices(btTransform &mtJointRelToParent, btTransform &mtJointRelToChild);
            virtual void CalculateRelativeJointMatrices(CStdFPoint vAdditionalRot, btTransform &mtJointRelToParent, btTransform &mtJointRelToChild);

		public:
			BlJoint();
			virtual ~BlJoint();

            virtual BlRigidBody *GetBlParent() {return m_lpBlParent;};
            virtual BlRigidBody *GetBlChild() {return m_lpBlChild;};

            virtual btRigidBody *GetBtParent() {return m_btParent;};
            virtual btRigidBody *GetBtChild() {return m_btChild;};

			virtual void DeletePhysics(bool bIncludeChildren);

            virtual bool Physics_IsDefined();
			virtual void Physics_ResetSimulation();
            virtual void Physics_CollectData();
            virtual void SetConstraintFriction() {};
            
            virtual float GetCurrentBtPosition() {return 0;};

			virtual btTypedConstraint* Constraint() {return m_btJoint;};
		};

	}			// Environment
}				//BulletAnimatSim
