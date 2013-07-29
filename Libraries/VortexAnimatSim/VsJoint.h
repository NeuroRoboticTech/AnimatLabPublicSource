// VsJoint.h: interface for the VsJoint class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		class VsRigidBody;

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

		class VORTEX_PORT VsJoint : public OsgJoint
		{
		protected:
			Vx::VxConstraint *m_vxJoint;
			Vx::VxConstraint::CoordinateID m_iCoordID;

			void UpdatePosition();

			virtual VsSimulator *GetVsSimulator();
            virtual void UpdatePositionAndRotationFromMatrix(osg::Matrix osgMT);

		public:
			VsJoint();
			virtual ~VsJoint();

            virtual BOOL Physics_IsDefined();
			virtual void Physics_ResetSimulation();
			virtual void Physics_CollectData();

			virtual Vx::VxConstraint* Constraint() {return m_vxJoint;};
			virtual Vx::VxConstraint::CoordinateID CoordinateID() {return m_iCoordID;};
		};

	}			// Environment
}				//VortexAnimatSim
