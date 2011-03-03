/**
\file	BodyPart.h

\brief	Declares the body part class. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		/**
		\class	BodyPart
		
		\brief	Base class for all body parts and joints.

		\details This is the base class for all types of body parts, both rigidbody and joints.
		
		\author	dcofer
		\date	3/2/2011
		**/
		class ANIMAT_PORT BodyPart : public Node
		{
		protected:
			///The parent rigid body of this part. If this value is null
			///then it is assumed that this is the root part of a structure.
			RigidBody *m_lpParent;

			///The absolute position of the body part in world coordinates.
			///This is calcualted during loading of the part using the position of 
			///the parent part and the relative position specified in the configuration file.
			CStdFPoint m_oAbsPosition;

			///These are rotation and position coords relative to the parent. Not in world coords.
			///This is only used for a few special cases like contact sensors.
			CStdFPoint m_oLocalPosition;

			///This is used for reporting the position back to the GUI. It is the local position scaled for
			///distance units.
			CStdFPoint m_oReportLocalPosition;

			///This is used for reporting the position back to the GUI. It is the world position scaled for
			///distance units.
			CStdFPoint m_oReportWorldPosition;

			///The rotation to apply to this body part. It is defined by the three
			///euler angles in radians.
			CStdFPoint m_oRotation;

			///This is used for reporting the rotation back to the GUI. We need to keep the
			///regular rotation information so it can be used during a simulation reset.
			CStdFPoint m_oReportRotation;

			///Determines if this body is physically seen or not. If this is FALSE then 
			///whatever geometry this is, like a box, is not seen in the graphics.
			BOOL m_bIsVisible;

			//Transparencies
			/// The alpha transparency used in the Graphics VisualSelectionMode
			float m_fltGraphicsAlpha;

			/// The alpha transparency used in the Collisions VisualSelectionMode
			float m_fltCollisionsAlpha;

			/// The alpha transparency used in the Joints VisualSelectionMode
			float m_fltJointsAlpha;

			/// The alpha transparency used in the Receptive Fields VisualSelectionMode
			float m_fltReceptiveFieldsAlpha;

			/// The alpha transparency used in the Simulation VisualSelectionMode
			float m_fltSimulationAlpha;

			/// The current alpha transparency for this body part.
			float m_fltAlpha; //Current alpha

			/// The scale value used to build the dragger objects for when this part is selected
			float m_fltGripScale;

			/// This is an interface pointer to a callback class that allows us to notify the gui
			/// of events that occur within the simulation.
			IBodyPartCallback *m_lpCallback;

			/// This is an interface references to the Vs version of this object.
			/// It will allow us to call methods directly in the Vs (OSG) version of the object
			/// directly without having to overload a bunch of methods in each box, sphere, etc..
			IPhysicsBody *m_lpPhysicsBody;

			virtual void UpdateData(Simulator *lpSim, Structure *lpStructure);

		public:
			BodyPart(void);
			virtual ~BodyPart(void);

#pragma region AccessorMutators

			RigidBody *Parent();
			void Parent(RigidBody *lpValue);

			virtual int VisualSelectionType() ;
			virtual BOOL AllowMouseManipulation();

			virtual CStdFPoint LocalPosition();
			virtual void LocalPosition(CStdFPoint &oPoint, BOOL bUseScaling = TRUE, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);
			virtual void LocalPosition(float fltX, float fltY, float fltZ, BOOL bUseScaling = TRUE, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);
			virtual void LocalPosition(string strXml, BOOL bUseScaling = TRUE, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);

			virtual CStdFPoint AbsolutePosition();
			virtual void AbsolutePosition(CStdFPoint &oPoint);
			virtual void AbsolutePosition(float fltX, float fltY, float fltZ);

			virtual CStdFPoint ReportLocalPosition();
			virtual void ReportLocalPosition(CStdFPoint &oPoint);
			virtual void ReportLocalPosition(float fltX, float fltY, float fltZ);

			virtual CStdFPoint ReportWorldPosition();
			virtual void ReportWorldPosition(CStdFPoint &oPoint);
			virtual void ReportWorldPosition(float fltX, float fltY, float fltZ);

			virtual CStdFPoint GetCurrentPosition();

			virtual CStdFPoint Rotation();
			virtual void Rotation(CStdFPoint &oPoint, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);
			virtual void Rotation(float fltX, float fltY, float fltZ, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);
			virtual void Rotation(string strXml, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);

			virtual CStdFPoint ReportRotation();
			virtual void ReportRotation(CStdFPoint &oPoint);
			virtual void ReportRotation(float fltX, float fltY, float fltZ);

			virtual BOOL IsVisible();
			virtual void IsVisible(BOOL bVal);

			virtual float GraphicsAlpha();
			virtual void GraphicsAlpha(float fltVal);

			virtual float CollisionsAlpha();
			virtual void CollisionsAlpha(float fltVal);

			virtual float JointsAlpha();
			virtual void JointsAlpha(float fltVal);

			virtual float ReceptiveFieldsAlpha();
			virtual void ReceptiveFieldsAlpha(float fltVal);

			virtual float SimulationAlpha();
			virtual void SimulationAlpha(float fltVal);

			virtual float Alpha();
			virtual void Alpha(float fltAlpha);

			virtual float GripScale();
			virtual void GripScale(float fltScale);

			virtual IBodyPartCallback *Callback();
			virtual void Callback(IBodyPartCallback *lpCallback);

			virtual IPhysicsBody *PhysicsBody();
			virtual void PhysicsBody(IPhysicsBody *lpBody);

			virtual float GetBoundingRadius();
			virtual void Resize();

#pragma endregion

#pragma region DataAccesMethods

			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

#pragma endregion

			virtual void Selected(BOOL bValue, BOOL bSelectMultiple); 
			virtual void AddBodyClicked(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ);
			virtual void VisualSelectionModeChanged(int iNewMode);

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
