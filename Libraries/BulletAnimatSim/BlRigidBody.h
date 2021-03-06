// BlRigidBody.h: interface for the BlRigidBody class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

#include "BlMaterialType.h"

namespace BulletAnimatSim
{
	namespace Environment
	{
		

            class BULLET_PORT BlContactPoint
            {
            public:
                btManifoldPoint *m_lpCP;
                BlRigidBody *m_lpContacted;
                bool m_bIsBodyA;

                BlContactPoint(btManifoldPoint *lpCP, BlRigidBody *lpContacted, bool bIsBodyA)
                {
                    m_lpCP = lpCP;
                    m_lpContacted = lpContacted;
                    m_bIsBodyA = bIsBodyA;
                }

                virtual ~BlContactPoint() {};

            };

            class BULLET_PORT BlBulletData
            {
            public:
                BlRigidBody *m_lpBody;
                bool m_bExclusionProcessing;

                BlBulletData(BlRigidBody *lpBody, bool bExclusionProcessing)
                {
                    m_lpBody = lpBody;
                    m_bExclusionProcessing = bExclusionProcessing;
                }

                virtual ~BlBulletData() {};

            };

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
			to convert it into a BlRigidBody and access those items that
			way. If we do not have this common class and instead just duplicated
			these items in each of the different BlBox, BlCylinder, etc. 
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
			some way to determine that it was say a BlBox type of object.
			And then we would have to dynamic cast it to BlBox. Again,
			this would be horribly messy with switches and other stuff.
			This was the cleanest solution that I could see. With this
			we just always cast the RigidBody to a BlRigidBody to
			get access to the vortex specific data elements.

			\sa
			RigidBody, BlBox, BlPlane, BlCylinder
		*/

		class BULLET_PORT BlRigidBody : public OsgRigidBody
		{
		protected:
            btCollisionShape *m_btCollisionShape;
            btCollisionObject *m_btCollisionObject;
            btCompoundShape *m_btCompoundShape;
            CStdPtrArray<btCollisionShape> m_aryCompoundChildShapes;

			btAnimatGeneric6DofConstraint *m_btStickyLock;
			btAnimatGeneric6DofConstraint *m_btStickyLock2;

            btRigidBody *m_btPart;
            osgbDynamics::MotionState *m_osgbMotion;
            BroadphaseNativeTypes m_eBodyType;

            float m_fltStaticMasses;

			///The area of this rigid body in the each axis direction. This is used to calculate the
			///drag force in this direction.
			CStdFPoint m_vArea;

            /// The area for this part after being rotated like the part is. This basically tells the 
            ///  area that is in the direction that the part is moving.
            CStdFPoint m_vRotatedArea;

            /// The buoyancy force applied to this part
            float m_fltBuoyancy;

            /// The buoyancy force reported to the GUI
            float m_fltReportBuoyancy;
            
			///This is the drag forces applied to this body.
			float m_vLinearDragForce[3];

			///This is the drag forces applied to this body.
			float m_vAngularDragTorque[3];

            //This is the data that is provided to the bullet body part user pointer.
            //We can access this when all we have is a bullet pointer to a part.
            BlBulletData *m_lpBulletData;

            BlSimulator *m_lpVsSim;

			/// The pointer to the material for this body
			BlMaterialType *m_lpMaterial;

			virtual void ProcessContacts();

            virtual void DeleteDynamicPart();
            virtual void DeleteSensorPart();
            virtual void DeleteCollisionGeometry();
            virtual void DeletePhysics(bool bIncludeChildren);

			virtual void CreateSensorPart();
            virtual void CreateStaticChildren(const CStdFPoint &vCom);
			virtual void CreateDynamicPart();
			virtual void CreateStickyLock();
            virtual void SetupOffsetCOM(const CStdFPoint &vCom);
			virtual btAnimatGeneric6DofConstraint *AddDynamicJoint(BlRigidBody *lpParent, BlRigidBody *lpChild);

            virtual void AddStaticGeometry(BlRigidBody *lpChild, btCompoundShape *btCompound, const CStdFPoint &vCom);
            virtual void RemoveStaticGeometry(BlRigidBody *lpChild, btCompoundShape *btCompound);

			CStdFPoint Physics_GetCurrentPosition();
			virtual void GetBaseValues();

			virtual void ResetStaticCollisionGeom();
			virtual void ResetSensorCollisionGeom();
			virtual void ResetDynamicCollisionGeom();

            virtual void DeleteChildPhysics();
            virtual void DeleteAttachedJointPhysics();
            virtual void RecreateAttachedJointPhysics();
			virtual void ResizePhysicsGeometry();

            virtual void CalculateVolumeAndAreas() {};
            virtual void CalculateRotatedAreas();

        public:
			BlRigidBody();
			virtual ~BlRigidBody();

            CStdPtrArray<BlContactPoint> m_aryContactPoints;

            btCompoundShape *CompoundShape() {return m_btCompoundShape;};
            btCollisionShape *CollisionShape() {return m_btCollisionShape;};
            btRigidBody *Part() {return m_btPart;};
            osgbDynamics::MotionState *MotionState() {return m_osgbMotion;};
			BlMaterialType *Material() {return m_lpMaterial;};

			virtual BlSimulator *GetBlSimulator();
			
            virtual bool NeedCollision(BlRigidBody *lpTest);
			virtual void SetSurfaceContactCount();

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
			virtual void Physics_DeleteStickyLock();

            virtual void Physics_AddBodyForceAtLocalPos(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits);
            virtual void Physics_AddBodyForceAtWorldPos(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, bool bScaleUnits);
			virtual void Physics_AddBodyTorque(float fltTx, float fltTy, float fltTz, bool bScaleUnits);
			virtual CStdFPoint Physics_GetVelocityAtPoint(float x, float y, float z);
			virtual float Physics_GetMass();
			virtual float Physics_GetDensity();
			virtual bool Physics_HasCollisionGeometry();
            virtual void Physics_StepHydrodynamicSimulation();
			virtual float *Physics_GetDataPointer(const std::string &strDataType);

            virtual osg::Matrix GetPhysicsWorldMatrix();

			virtual void MaterialTypeModified();

            friend class BlJoint;
		};

	}			// Environment
}				//BulletAnimatSim
