// VsRigidBody.h: interface for the VsRigidBody class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSRIGIDBODY_H__BE00E72D_B205_450A_9A20_58752ED37EED__INCLUDED_)
#define AFX_VSRIGIDBODY_H__BE00E72D_B205_450A_9A20_58752ED37EED__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatSim
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
			to convert it into a VsRigidBody and access those items that
			way. If we do not have this common class and instead just duplicated
			these items in each of the different VsBox, VsCylinder, etc. 
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
			some way to determine that it was say a VsBox type of object.
			And then we would have to dynamic cast it to VsBox. Again,
			this would be horribly messy with switches and other stuff.
			This was the cleanest solution that I could see. With this
			we just always cast the RigidBody to a VsRigidBody to
			get access to the vortex specific data elements.

			\sa
			RigidBody, VsBox, VsPlane, VsCylinder
		*/

		class VORTEX_PORT VsRigidBody : public VsBody
		{
		protected:
			//The physics part that this body is
			VxCollisionSensor *m_vxSensor;
			VxPart *m_vxPart;
			VxGeometry *m_vxGeometry;
			VxCollisionGeometry *m_vxCollisionGeometry;
			VxNode m_vxGraphicNode;

			RigidBody *m_lpThisRB;

			//Keeps track of the bouyancy force applied to this rigid body at each time step.
			//We need the report variable because it must be rescaled back for display.
			//float m_fltBuoyancy;
			//float m_fltReportBuoyancy; 

			//We need these arrays to store body data that could potentially be charted.
			//this may be scaled so we need to store it in here instead of just using the
			//body data directly from the physics engine.
			BOOL m_bCollectExtraData;
			CStdFPoint m_vPos;

			//float m_vPosition[3];
			//float m_vRotation[3];
			float m_vLinearVelocity[3];
			float m_vAngularVelocity[3];
			float m_vLinearAcceleration[3];
			float m_vAngularAcceleration[3];
			float m_vForce[3];
			float m_vTorque[3];
			
			float m_fltMass;
			float m_fltReportMass;
			float m_fltReportVolume;

			virtual void SetThisPointers();

			virtual void ProcessContacts();

			virtual void DeletePhysics();
			virtual void CreateSensorPart();
			virtual void CreateStaticPart();
			virtual void CreateDynamicPart();
			virtual void RemoveStaticPart();

			CStdFPoint Physics_GetCurrentPosition();
			virtual void GetBaseValues();
			virtual void UpdatePositionAndRotationFromMatrix();
			virtual void ResetStaticCollisionGeom();
			virtual void ResetSensorCollisionGeom();
			virtual void SetFollowEntity(VsRigidBody *lpEntity);

			virtual void ShowSelectedVertex();
			virtual void HideSelectedVertex();

		public:
			VsRigidBody();
			virtual ~VsRigidBody();

			Vx::VxCollisionSensor* Sensor();	
			Vx::VxPart* Part();	
			Vx::VxNode GraphicsNode();
			Vx::VxCollisionGeometry *CollisionGeometry();
			virtual void CollisionGeometry(Vx::VxCollisionGeometry *vxGeometry);

			virtual int GetPartIndex(VxPart *vxP0, VxPart *vxP1);

			virtual osg::Group *ParentOSG();
			virtual void SetupPhysics();

			virtual void SetBody();
			
			virtual void Initialize();
			virtual void BuildLocalMatrix();
			virtual void Physics_ResetSimulation();
			virtual void Physics_EnableCollision(RigidBody *lpBody);
			virtual void Physics_DisableCollision(RigidBody *lpBody);
			virtual void Physics_CollectData();
			virtual float *Physics_GetDataPointer(const string &strDataType);
			virtual void Physics_UpdateMatrix();
			virtual void Physics_SetFreeze(BOOL bVal);
			virtual void Physics_SetDensity(float fltVal);
			virtual void Physics_SetMaterialID(string strID);
			virtual void Physics_SetVelocityDamping(float fltLinear, float fltAngular);
			virtual void Physics_SetCenterOfMass(float fltTx, float fltTy, float fltTz);
			virtual void Physics_SetColor();
			virtual void Physics_TextureChanged();
			virtual void Physics_UpdateNode();
			virtual void Physics_Resize();
			virtual void Physics_SelectedVertex(float fltXPos, float fltYPos, float fltZPos);
			virtual void Physics_ResizeSelectedReceptiveFieldVertex();
			virtual void Physics_FluidDataChanged();

			virtual void Physics_AddBodyForce(float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, BOOL bScaleUnits);
			virtual void Physics_AddBodyTorque(float fltTx, float fltTy, float fltTz, BOOL bScaleUnits);
			virtual CStdFPoint Physics_GetVelocityAtPoint(float x, float y, float z);
			virtual float Physics_GetMass();
			virtual BOOL Physics_HasCollisionGeometry();
		};

	}			// Environment
}				//VortexAnimatSim

#endif // !defined(AFX_VSRIGIDBODY_H__BE00E72D_B205_450A_9A20_58752ED37EED__INCLUDED_)
