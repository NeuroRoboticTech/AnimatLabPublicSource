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
            float m_fltPrevJointPos;

			void UpdatePosition();

			virtual BlSimulator *GetBlSimulator();

		public:
			BlJoint();
			virtual ~BlJoint();

			virtual void DeletePhysics();

            virtual bool Physics_IsDefined();
			virtual void Physics_ResetSimulation();
            virtual void Physics_CollectData();
            
            virtual float GetCurrentBtPosition() {return 0;};

			virtual btTypedConstraint* Constraint() {return m_btJoint;};
		};

	}			// Environment
}				//BulletAnimatSim
