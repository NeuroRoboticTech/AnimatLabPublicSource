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

            /// Pointer to a robot interface node to allow the organism to be hooked to a robot.
            RobotPartInterface *m_lpRobot;

			virtual void UpdateData();

		public:
			BodyPart(void);
			virtual ~BodyPart(void);

#pragma region AccessorMutators

			virtual IPhysicsBody *PhysicsBody();
			virtual void PhysicsBody(IPhysicsBody *lpBody);

            virtual RobotPartInterface *GetRobotPartInterface();
            virtual void SetRobotPartInterface(RobotPartInterface *lpPart);

			virtual void Resize();

#pragma endregion

#pragma region DataAccesMethods

			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify);
			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
            virtual void UpdateExtraData();

#pragma endregion

			virtual void Selected(bool bValue, bool bSelectMultiple); 
			virtual void AddBodyClicked(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ);
			virtual void VisualSelectionModeChanged(int iNewMode);
			virtual void UpdatePhysicsPosFromGraphics();
            virtual void WakeDynamics();

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
