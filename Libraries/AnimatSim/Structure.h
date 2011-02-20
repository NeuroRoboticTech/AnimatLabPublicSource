// AlStructure.h: interface for the Structure class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALSTRUCTURE_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
#define AFX_ALSTRUCTURE_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace AnimatSim
{
	namespace Environment
	{
		class ANIMAT_PORT CollisionPair : public AnimatBase
		{
		public:
			string m_strPart1ID;
			string m_strPart2ID;

			CollisionPair() {};
			~CollisionPair() {};
		};


		/*! \brief 
			A "static" structure in the simulation.

			\remarks
			This object is a "static" structure in the simulation. I say "static" 
			because can move, but it is part of the background environment and does
			not have a brain controlling its movement. For example, if you wanted to
			have a house as an obstacle in the environemt then that house would probably
			have a door. The door would be connected to the wall of the house using a 
			hinge joint. So the door can open and close, but it does not have a brain 
			to control its movement. Another example is a rock. If you create a rock and
			place it at a precarious position on the top of a hill then it will move by
			rolling down the hill, but this is a passive effect of gravity acting on the
			body of the rock, and not something that the rock actively did. An organism
			on the other hand does have a neural network that actively controls the 
			movements of its limbs to change its position and orientation within the world.
					
			\sa
			Structure, Organism, Body, Joint
				
			\ingroup AnimatSim
		*/

		class ANIMAT_PORT Structure : public AnimatBase 
		{
		protected:
			Simulator *m_lpSim;

			///The path to the directory to use for this project.
			string m_strProjectPath;

			///The filename of the layout configuration of this structure.
			string m_strLayoutFile;

			///The root rigid body object of this structure.
			RigidBody *m_lpBody;

			///The initial position of this structure in world coordinates.
			///The root rigid body position is relative to this position.
			CStdFPoint m_oPosition;

			//This is used for reporting the position back to the GUI. It is the position scaled for
			//distance units.
			CStdFPoint m_oReportPosition;

			//This is used for reporting the rotation back to the GUI. We need to keep the
			//regular rotation information so it can be used during a simulation reset.
			CStdFPoint m_oReportRotation;

			///A list of rigid bodies contained within this structure.
			///The objects in this list are references only. They are not
			///destroyed by this list. It is used primarily for finding
			///rigid bodies and ensuring that only unique ID's are used
			///for the rigid bodies.
			CStdMap<string, RigidBody *> m_aryRigidBodies;

			///A list of joints contained within this structure.
			///The objects in this list are references only. They are not
			///destroyed by this list. It is used primarily for finding
			///joints and ensuring that only unique ID's are used
			///for the joints.
			CStdMap<string, Joint *> m_aryJoints;

			///This is the list of other body part ID's to exclude from collision tests.
			CStdPtrArray<CollisionPair> m_aryExcludeCollisionList;
		
			IBodyPartCallback *m_lpCallback;

			virtual void LoadLayout(Simulator *lpSim);
			virtual void LoadLayout(Simulator *lpSim, CStdXml &oXml);
			virtual void LoadCollisionPair(CStdXml &oXml);
			virtual RigidBody *LoadRoot(CStdXml &oXml);

			virtual void AddRoot(string strXml);
			virtual void RemoveRoot(string strID, BOOL bThrowError = TRUE);
			
			virtual void CollectStructureData(Simulator *lpSim);

		public:
			Structure();
			virtual ~Structure();

			void Sim(Simulator *lpSim) {m_lpSim = lpSim;};

			virtual string ProjectPath() {return m_strProjectPath;};
			virtual void ProjectPath(string strValue) {m_strProjectPath = strValue;};

			virtual string LayoutFile() {return m_strLayoutFile;};
			virtual void LayoutFile(string strValue) {m_strLayoutFile = strValue;};

			virtual CStdFPoint Position() {return m_oPosition;};
			virtual void Position(CStdFPoint &oPoint) {m_oPosition = oPoint;};

			virtual CStdFPoint ReportPosition() {return m_oReportPosition;};
			virtual void ReportPosition(CStdFPoint &oPoint) {m_oReportPosition = oPoint;};
			virtual void ReportPosition(float fltX, float fltY, float fltZ) {m_oReportPosition.Set(fltX, fltY, fltZ);};

			virtual CStdFPoint ReportRotation() {return m_oReportRotation;};
			virtual void ReportRotation(CStdFPoint &oPoint) {m_oReportRotation = oPoint;};
			virtual void ReportRotation(float fltX, float fltY, float fltZ) {m_oReportRotation.Set(fltX, fltY, fltZ);};

			virtual CStdPtrArray<CollisionPair> ExclusionList() {return m_aryExcludeCollisionList;};
			virtual void AddCollisionPair(string strID1, string strID2);

			virtual RigidBody *Body() {return m_lpBody;};

			virtual IBodyPartCallback *Callback() {return m_lpCallback;};
			virtual void Callback(IBodyPartCallback *lpCallback) {m_lpCallback = lpCallback;};

			virtual void *Assembly() = 0;
			virtual void *GetMatrixPointer() = 0;

			virtual void Initialize(Simulator *lpSim);
			virtual void StepPhysicsEngine(Simulator *lpSim);

			void AddJoint(Joint *lpJoint);
			void AddRigidBody(RigidBody *lpBody);

			virtual Joint *FindJoint(string strJointID, BOOL bThrowError = TRUE);
			virtual RigidBody *FindRigidBody(string strBodyID, BOOL bThrowError = TRUE);
			virtual Node *FindNode(string strID, BOOL bThrowError = TRUE);
			//virtual AnimatBase *FindCollisionPair(string strID, BOOL bThrowError = TRUE);

			virtual void EnableMotor(string strJointID, BOOL bVal);
			virtual void SetMotorInput(string strJointID, float fltInput);

			virtual void EnableCollision(Simulator *lpSim, RigidBody *lpCollisionBody);
			virtual void DisableCollision(Simulator *lpSim, RigidBody *lpCollisionBody);

			virtual void ResetSimulation(Simulator *lpSim);

#pragma region DataAccesMethods

			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
			virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE);
			virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);

#pragma endregion

			virtual long CalculateSnapshotByteSize();
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);

			virtual void Load(Simulator *lpSim, CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_ALSTRUCTURE_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
