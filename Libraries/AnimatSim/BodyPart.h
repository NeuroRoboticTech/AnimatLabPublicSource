#pragma once

namespace AnimatSim
{
	namespace Environment
	{

		class ANIMAT_PORT BodyPart : public Node
		{
		protected:
			///The parent rigid body of this part. If this value is null
			///then it is assumed that this is the root part of a structure.
			RigidBody *m_lpParent;

			///The absolute position of the rigid body in world coordinates.
			///This is calcualted during loading of the part using the position of 
			///the parent part and the relative position specified in the configuration file.
			CStdFPoint m_oAbsPosition;

			//These are rotation and position coords relative to the parent. Not in world coords.
			//This is only used for a few special cases like contact sensors.
			CStdFPoint m_oLocalPosition;

			//This is used for reporting the position back to the GUI. It is the local position scaled for
			//distance units.
			CStdFPoint m_oReportLocalPosition;

			//This is used for reporting the position back to the GUI. It is the world position scaled for
			//distance units.
			CStdFPoint m_oReportWorldPosition;

			///The rotation to apply to this rigid body. It is defined by the three
			///euler angles in radians.
			CStdFPoint m_oRotation;

			//This is used for reporting the rotation back to the GUI. We need to keep the
			//regular rotation information so it can be used during a simulation reset.
			CStdFPoint m_oReportRotation;

			///Determines if this body is physically seen or not. If this is FALSE then 
			///whatever geometry this is, like a box, is not seen in the graphics.
			BOOL m_bIsVisible;

			//Transparencies
			float m_fltGraphicsAlpha;
			float m_fltCollisionsAlpha;
			float m_fltJointsAlpha;
			float m_fltReceptiveFieldsAlpha;
			float m_fltSimulationAlpha;
			float m_fltAlpha; //Current alpha

			float m_fltGripScale;

			BOOL m_bAllowMouseManipulation;

			//This is an interface pointer to a callback class that allows us to notify the gui
			//of events that occur within the simulation.
			IBodyPartCallback *m_lpCallback;

			//This is an interface references to the Vs version of this object.
			//It will allow us to call methods directly in the Vs (OSG) version of the object
			//directly without having to overload a bunch of methods in each box, sphere, etc..
			IPhysicsBody *m_lpPhysicsBody;

			virtual void UpdateData(Simulator *lpSim, Structure *lpStructure);

		public:
			BodyPart(void);
			virtual ~BodyPart(void);

#pragma region AccessorMutators

			RigidBody *Parent() {return m_lpParent;};
			void Parent(RigidBody *lpValue) {m_lpParent = lpValue;};

			virtual int VisualSelectionType() {return 0;};
			virtual void Selected(BOOL bValue, BOOL bSelectMultiple); 

			virtual void AddBodyClicked(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ);

			virtual CStdFPoint LocalPosition() {return m_oLocalPosition;};
			virtual void LocalPosition(CStdFPoint &oPoint, BOOL bUseScaling = TRUE, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);
			virtual void LocalPosition(float fltX, float fltY, float fltZ, BOOL bUseScaling = TRUE, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);
			virtual void LocalPosition(string strXml, BOOL bUseScaling = TRUE, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);

			virtual CStdFPoint AbsolutePosition() {return m_oAbsPosition;};
			virtual void AbsolutePosition(CStdFPoint &oPoint) {m_oAbsPosition = oPoint;};
			virtual void AbsolutePosition(float fltX, float fltY, float fltZ) {m_oAbsPosition.Set(fltX, fltY, fltZ);};

			virtual CStdFPoint ReportLocalPosition() {return m_oReportLocalPosition;};
			virtual void ReportLocalPosition(CStdFPoint &oPoint) {m_oReportLocalPosition = oPoint;};
			virtual void ReportLocalPosition(float fltX, float fltY, float fltZ) {m_oReportLocalPosition.Set(fltX, fltY, fltZ);};

			virtual CStdFPoint ReportWorldPosition() {return m_oReportWorldPosition;};
			virtual void ReportWorldPosition(CStdFPoint &oPoint) {m_oReportWorldPosition = oPoint;};
			virtual void ReportWorldPosition(float fltX, float fltY, float fltZ) {m_oReportWorldPosition.Set(fltX, fltY, fltZ);};
			
			virtual CStdFPoint Rotation()	{return m_oRotation;};
			virtual void Rotation(CStdFPoint &oPoint, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);
			virtual void Rotation(float fltX, float fltY, float fltZ, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);
			virtual void Rotation(string strXml, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);

			virtual CStdFPoint ReportRotation() {return m_oReportRotation;};
			virtual void ReportRotation(CStdFPoint &oPoint) {m_oReportRotation = oPoint;};
			virtual void ReportRotation(float fltX, float fltY, float fltZ) {m_oReportRotation.Set(fltX, fltY, fltZ);};

			virtual BOOL IsVisible() {return m_bIsVisible;};
			virtual void IsVisible(BOOL bVal);

			virtual float GraphicsAlpha() {return m_fltGraphicsAlpha;};
			virtual void GraphicsAlpha(float fltVal);

			virtual float CollisionsAlpha() {return m_fltCollisionsAlpha;};
			virtual void CollisionsAlpha(float fltVal);

			virtual float JointsAlpha() {return m_fltJointsAlpha;};
			virtual void JointsAlpha(float fltVal);

			virtual float ReceptiveFieldsAlpha() {return m_fltReceptiveFieldsAlpha;};
			virtual void ReceptiveFieldsAlpha(float fltVal);

			virtual float SimulationAlpha() {return m_fltSimulationAlpha;};
			virtual void SimulationAlpha(float fltVal);

			virtual float Alpha() {return m_fltAlpha;};
			virtual void Alpha(float fltAlpha) {m_fltAlpha = fltAlpha;};

			virtual float GripScale() {return m_fltGripScale;};
			virtual void GripScale(float fltScale) {m_fltGripScale = fltScale;};

			virtual BOOL AllowMouseManipulation() {return m_bAllowMouseManipulation;};
			virtual void AllowMouseManipulation(BOOL bVal) {m_bAllowMouseManipulation = bVal;};

			virtual IBodyPartCallback *Callback() {return m_lpCallback;};
			virtual void Callback(IBodyPartCallback *lpCallback) {m_lpCallback = lpCallback;};

			virtual IPhysicsBody *PhysicsBody() {return m_lpPhysicsBody;};
			virtual void PhysicsBody(IPhysicsBody *lpBody) {m_lpPhysicsBody = lpBody;};

			virtual float GetBoundingRadius();
			virtual void Resize() {};

#pragma endregion

#pragma region DataAccesMethods

			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

#pragma endregion

			virtual void VisualSelectionModeChanged(int iNewMode);

			virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
