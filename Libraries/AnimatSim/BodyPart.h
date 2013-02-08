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
		class ANIMAT_PORT BodyPart : public Node, public MovableItem
		{
		protected:
			/// This is an interface references to the Vs version of this object.
			/// It will allow us to call methods directly in the Vs (OSG) version of the object
			/// directly without having to overload a bunch of methods in each box, sphere, etc..
			IPhysicsBody *m_lpPhysicsBody;

			virtual void UpdateData();

		public:
			BodyPart(void);
			virtual ~BodyPart(void);

#pragma region AccessorMutators

			virtual IPhysicsBody *PhysicsBody();
			virtual void PhysicsBody(IPhysicsBody *lpBody);

			virtual void Resize();

#pragma endregion

#pragma region DataAccesMethods

			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify);
			virtual float *GetDataPointer(const string &strDataType);
			virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

#pragma endregion

			virtual void Selected(BOOL bValue, BOOL bSelectMultiple); 
			virtual void AddBodyClicked(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ);
			virtual void VisualSelectionModeChanged(int iNewMode);
			virtual void UpdatePhysicsPosFromGraphics();

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
