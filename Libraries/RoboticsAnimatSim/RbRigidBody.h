// RbRigidBody.h: interface for the RbRigidBody class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

#include "RbMaterialType.h"

namespace RoboticsAnimatSim
{
	namespace Environment
	{
	
		/*! \brief 
			A common class for all rigid body data specific to vortex.

			\remarks
			This is a common class for the rigid body objects that 
			specifically holds vortex data and methods. I had hoped to
			not have this class. However, it proved necessary. The reason
			is that rigid bodies like the box and cylinder all had common
			data items asociated with them like the m_iBodyID that need
			to be used in various places in order to get things done. 
			When we are in one of the overridden virtual functions we
			need a way to get at these data members for a rigid body.
			If we have this class then we can just do a dynamic cast
			to convert it into a RbRigidBody and access those items that
			way. If we do not have this common class and instead just duplicated
			these items in each of the different RbBox, RbCylinder, etc. 
			classes then we have a problem. If we put virtual accessor 
			functions for those items in the RigidBody class then that
			is not appropriate because the animat library is not supposed 
			to know anything about data elements specific to vortex. 
			What happens if you move to a different physics engine? Do
			you put data elements for that one in RigidBody also. 
			Clearly that will not work as a solution. The only other
			way would be to try and determine the actual type of object
			you have and do a specific cast to that one. So if we are
			looking at the parent of the current rigid body and we have
			it as a RigidBody pointer then we would have to find 
			some way to determine that it was say a RbBox type of object.
			And then we would have to dynamic cast it to RbBox. Again,
			this would be horribly messy with switches and other stuff.
			This was the cleanest solution that I could see. With this
			we just always cast the RigidBody to a RbRigidBody to
			get access to the vortex specific data elements.

			\sa
			RigidBody, RbBox, RbPlane, RbCylinder
		*/

		class ROBOTICS_PORT RbRigidBody : public RbBody
		{
		protected:
            RbSimulator *m_lpRbSim;

			/// The pointer to the material for this body
			RbMaterialType *m_lpMaterial;

			CStdFPoint Physics_GetCurrentPosition();

        public:
			RbRigidBody();
			virtual ~RbRigidBody();

			RbMaterialType *Material() {return m_lpMaterial;};

			virtual RbSimulator *GetRbSimulator();
			
            virtual void Initialize() {};
            virtual bool Physics_IsDefined();
            virtual bool Physics_IsGeometryDefined();
			virtual void Physics_ResetSimulation();
			virtual void Physics_EnableCollision(RigidBody *lpBody);
			virtual void Physics_DisableCollision(RigidBody *lpBody);
			virtual void Physics_CollectData();
            virtual void Physics_CollectExtraData();
			virtual void Physics_SetFreeze(bool bVal);
			virtual void Physics_SetMass(float fltVal);
			virtual void Physics_SetMaterialID(std::string strID);
			virtual void Physics_SetVelocityDamping(float fltLinear, float fltAngular);
			virtual void Physics_SetCenterOfMass(float fltTx, float fltTy, float fltTz);
			virtual void Physics_UpdateNode();
			virtual void Physics_FluidDataChanged();
            virtual void Physics_WakeDynamics();
            virtual void Physics_ContactSensorAdded(ContactSensor *lpSensor);
            virtual void Physics_ContactSensorRemoved();
            virtual void Physics_ChildBodyAdded(RigidBody *lpChild);
            virtual void Physics_ChildBodyRemoved(bool bHasStaticJoint);

			virtual void Physics_AddBodyForce(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits);
			virtual void Physics_AddBodyTorque(float fltTx, float fltTy, float fltTz, bool bScaleUnits);
			virtual CStdFPoint Physics_GetVelocityAtPoint(float x, float y, float z);
			virtual float Physics_GetMass();
			virtual float Physics_GetDensity();
			virtual bool Physics_HasCollisionGeometry();
            virtual void Physics_StepHydrodynamicSimulation();
			virtual float *Physics_GetDataPointer(const std::string &strDataType);

            friend class RbJoint;
		};

	}			// Environment
}				//RoboticsAnimatSim
