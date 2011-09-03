/**
\file	Structure.h

\brief	Declares the structure class. 
**/

#pragma once

namespace AnimatSim
{

	/**
	\namespace	AnimatSim::Environment

	\brief	This namespace contains all of the objects that are related to the physical part of the 
	virtual 3-D world. All of the neural components are located in the Behavioral namespace. 
	**/
	namespace Environment
	{

		/**
		\class	CollisionPair
		
		\brief	Collision pair. 
		
		\details Keeps track of the ID's for two pairs of body parts that can, or cannot collide.
		This is primarily used in the collision exclusion list to disable collisions between the 
		two objects.

		\author	dcofer
		\date	2/25/2011
		**/
		class ANIMAT_PORT CollisionPair : public AnimatBase
		{
		public:
			/// GUID ID of the first part of the collision pair.
			string m_strPart1ID;
			/// GUID ID of the second part of the collision pair.
			string m_strPart2ID;

			/**
			\brief	Default constructor.
			
			\author	dcofer
			\date	3/22/2011
			**/
			CollisionPair() {};

			/**
			\brief	Finaliser.
			
			\author	dcofer
			\date	3/22/2011
			**/
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
		*/
		class ANIMAT_PORT Structure : public AnimatBase, public MovableItem 
		{
		protected:
			///The root rigid body object of this structure.
			RigidBody *m_lpBody;

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

			///This is a pointer to an instance of the IMovableItemCallback interface. This
			///interface is used to send back information to the GUI. If you are running in
			///simulation mode only then this will be NULL and no info will be sent back up.
			///If you are running with the GUI then it makes the calls back up to it.
			IMovableItemCallback *m_lpCallback;

			/// The radius of the graphical sphere shown for the structure position.
			float m_fltSize;

			virtual void LoadLayout(CStdXml &oXml);
			virtual void LoadCollisionPair(CStdXml &oXml);
			virtual RigidBody *LoadRoot(CStdXml &oXml);

			virtual void AddRoot(string strXml);
			virtual void RemoveRoot(string strID, BOOL bThrowError = TRUE);
			
			virtual void UpdateData();

		public:
			Structure();
			virtual ~Structure();

			void Sim(Simulator *lpSim);

			virtual void Body(RigidBody *lpBody);
			virtual RigidBody *Body();

			virtual CStdFPoint Position();
			virtual void Position(CStdFPoint &oPoint, BOOL bUseScaling = TRUE, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);

			virtual float Size();
			virtual void Size(float fltVal, BOOL bUseScaling = TRUE);

			virtual BOOL AllowTranslateDragX();
			virtual BOOL AllowTranslateDragY();
			virtual BOOL AllowTranslateDragZ();

			virtual BOOL AllowRotateDragX();
			virtual BOOL AllowRotateDragY();
			virtual BOOL AllowRotateDragZ();

			virtual CStdPtrArray<CollisionPair> ExclusionList();
			virtual void AddCollisionPair(string strID1, string strID2);

			virtual void MinTimeStep(float &fltMin);

			/**
			\fn	virtual void Structure::*Assembly() = 0;
			
			\brief	Gets the assembly pointer.

			\details This is a pure virtual method that is used in the derived class to return 
			a pointer to the vortex assembly object. 
			
			\author	dcofer
			\date	2/25/2011
			
			\return	null if it fails, else. 
			**/
			virtual void *Assembly() = 0;

			virtual void Create();
			virtual void StepPhysicsEngine();
			virtual void ResetSimulation();

			void AddJoint(Joint *lpJoint);
			void RemoveJoint(string strID);
			void AddRigidBody(RigidBody *lpBody);
			void RemoveRigidBody(string strID);

			virtual Joint *FindJoint(string strJointID, BOOL bThrowError = TRUE);
			virtual RigidBody *FindRigidBody(string strBodyID, BOOL bThrowError = TRUE);
			virtual Node *FindNode(string strID, BOOL bThrowError = TRUE);
			//virtual AnimatBase *FindCollisionPair(string strID, BOOL bThrowError = TRUE);

			virtual void EnableMotor(string strJointID, BOOL bVal);
			virtual void SetMotorInput(string strJointID, float fltInput);

			virtual void EnableCollision(RigidBody *lpCollisionBody);
			virtual void DisableCollision(RigidBody *lpCollisionBody);

			virtual void Selected(BOOL bValue, BOOL bSelectMultiple); 
			virtual void UpdatePhysicsPosFromGraphics();

#pragma region DataAccesMethods

			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify);
			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
			virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE);
			virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);

#pragma endregion

#pragma region SnapshotMethods
			virtual long CalculateSnapshotByteSize();
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);
#pragma endregion

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
