/**
\file	BodyPart.h

\brief	Declares the body part class. 
**/

#pragma once

#include "RobotPartInterface.h"

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
		class ANIMAT_PORT BodyPart : public Node, public MovableItem
		{
		protected:
			/// This is an interface references to the Vs version of this object.
			/// It will allow us to call methods directly in the Vs (OSG) version of the object
			/// directly without having to overload a bunch of methods in each box, sphere, etc..
			IPhysicsBody *m_lpPhysicsBody;

            /// Array of pointers to robot part interfaces connected to this body part.
            CStdArray<RobotPartInterface *> m_aryRobotParts;

			///Determines whether the m_bRobotAdpaterSynch flag applies to this adapter. Adpaters between neural elements should not 
			///need to be synched because they are not dependent on IO timing. This flag allows you to control this by setting it to false
			///for adapters that do not need it.
			bool m_bSynchWithRobot;

			///This is how often we need to update this particular adapter
			float m_fltSynchUpdateInterval;

			///The number of ticks between each call to update for this adapter till the next update time.
			int m_iSynchUpdateInterval;

			///Keeps track of the last time we did a synch for the robot.
			int m_iSynchCount;

			virtual void UpdateData();

		public:
			BodyPart(void);
			virtual ~BodyPart(void);
																	
			static BodyPart *CastToDerived(AnimatBase *lpBase) {return static_cast<BodyPart*>(lpBase);}

#pragma region AccessorMutators

			virtual IPhysicsBody *PhysicsBody();
			virtual void PhysicsBody(IPhysicsBody *lpBody);

            virtual CStdArray<RobotPartInterface *> *GetRobotPartInterfaces();
            virtual void AddRobotPartInterface(RobotPartInterface *lpPart);
            virtual void RemoveRobotPartInterface(RobotPartInterface *lpPart);
			virtual int FindRobotPartListIndex(std::string strID, bool bThrowError = true);

			virtual bool SynchWithRobot();
			virtual void SynchWithRobot(bool bVal);

			virtual float SynchUpdateInterval();
			virtual void SynchUpdateInterval(float fltVal);

			virtual void Resize();

#pragma endregion

#pragma region DataAccesMethods

			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify);
			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
            virtual void UpdateExtraData();
			virtual bool NeedsRobotSynch();

#pragma endregion

			virtual void Initialize();
			virtual void ResetSimulation();
			virtual void TimeStepModified();
			virtual void Selected(bool bValue, bool bSelectMultiple); 
			virtual void AddBodyClicked(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ);
			virtual void VisualSelectionModeChanged(int iNewMode);
			virtual void UpdatePhysicsPosFromGraphics();
            virtual void WakeDynamics();

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
