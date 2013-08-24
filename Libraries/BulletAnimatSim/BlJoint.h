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
            //FIX PHYSICS
			//Vx::VxConstraint *m_vxJoint;
			//Vx::VxConstraint::CoordinateID m_iCoordID;
            BlSimulator *m_lpVsSim;

			void UpdatePosition();

			virtual BlSimulator *GetBlSimulator();

		public:
			BlJoint();
			virtual ~BlJoint();

            virtual bool Physics_IsDefined();
			virtual void Physics_ResetSimulation();
			virtual void Physics_CollectData();

            //FIX PHYSICS
			//virtual Vx::VxConstraint* Constraint() {return m_vxJoint;};
			//virtual Vx::VxConstraint::CoordinateID CoordinateID() {return m_iCoordID;};
		};

	}			// Environment
}				//BulletAnimatSim
