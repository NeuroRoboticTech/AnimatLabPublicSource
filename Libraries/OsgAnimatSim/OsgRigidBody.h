// OsgRigidBody.h: interface for the OsgRigidBody class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

namespace OsgAnimatSim
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
			to convert it into a OsgRigidBody and access those items that
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
			we just always cast the RigidBody to a OsgRigidBody to
			get access to the vortex specific data elements.

			\sa
			RigidBody, VsBox, VsPlane, VsCylinder
		*/

		class ANIMAT_OSG_PORT OsgRigidBody : public OsgBody
		{
		protected:
			RigidBody *m_lpThisRB;

			//We need these arrays to store body data that could potentially be charted.
			//this may be scaled so we need to store it in here instead of just using the
			//body data directly from the physics engine.
			bool m_bCollectExtraData;
			CStdFPoint m_vPos;

			//float m_vPosition[3];
			//float m_vRotation[3];
			float m_vLinearVelocity[3];
			float m_vAngularVelocity[3];
			float m_vLinearAcceleration[3];
			float m_vAngularAcceleration[3];
			float m_vForce[3];
			float m_vTorque[3];

			virtual void SetThisPointers();

			virtual void ProcessContacts() = 0;
            
			virtual void DeletePhysics() = 0;
			virtual void CreateSensorPart() = 0;
			virtual void CreateStaticPart() = 0;
			virtual void CreateDynamicPart() = 0;
			virtual void RemoveStaticPart() = 0;

			virtual void UpdatePositionAndRotationFromMatrix();

			virtual void ShowSelectedVertex();
			virtual void HideSelectedVertex();
            virtual void GetBaseValues() = 0;

		public:
			OsgRigidBody();
			virtual ~OsgRigidBody();

			virtual osg::MatrixTransform *ParentOSG();

			virtual void SetupPhysics();
            virtual bool AddOsgNodeToParent();

			virtual void Initialize();
			virtual void BuildLocalMatrix();
			virtual float *Physics_GetDataPointer(const string &strDataType);
			virtual void Physics_UpdateMatrix();
			virtual void Physics_SetColor();
			virtual void Physics_TextureChanged();
			virtual void Physics_Resize();
			virtual void Physics_SelectedVertex(float fltXPos, float fltYPos, float fltZPos);
			virtual void Physics_ResizeSelectedReceptiveFieldVertex();
		};

	}			// Environment
}				//OsgAnimatSim
