#pragma once

namespace AnimatSim
{
	namespace Environment
	{

		/*! \brief 
			The base class for all of the joint type of objects.

			\remarks
			This class provides the base functionality for a joint. 
			A joint is what glues the different rigid bodies together
			into a complete structure. Each object is connected by a 
			joint to its parent part. The only exception to this rule
			is the root rigid body of a structure/organism. The joint is
			positioned relative to the center of the parent object. 
			This can be a little confusing becuase in the configuration
			file the joint is actually contained within the child object
			tag. So you may be tempted to try and make the joint relative
			to the child object. The different sub classes will define 
			data parameters that specify the motion that they joint can 
			perform. For instance, a CAlHingeJoint is constrained to rotate
			about one axis just like the hinge on a door. But if you simply
			wanted one part act as if it is phyiscally connected to its parent
			part then you would use a CAlStaticJoint.

			\sa
			Joint, CAlHingeJoint, CAlStaticJoint
			 
			\ingroup AnimatSim
		*/

		class ANIMAT_PORT Joint : public BodyPart  
		{
		protected:
			///The child rigid body for this joint. 
			RigidBody *m_lpChild;

			///This is the velocity to use for the motorized joint. The motor must be enabled
			///for this parameter to have any effect.
			float m_fltSetVelocity;
			float m_fltDesiredVelocity;
			float m_fltMaxVelocity;
			float m_fltPrevVelocity;

			BOOL m_bEnableMotor;
			BOOL m_bEnableMotorInit;
			BOOL m_bEnableLimits;

			float m_fltPosition;
			float m_fltVelocity;
			float m_fltForce;

			///Scales the size of the graphics for this joint.
			float m_fltSize;

			virtual void SetVelocityToDesired();

		public:
			Joint();
			virtual ~Joint();

			//Sometimes we need to know if the joint rotates or moves linearly. 
			//This param tells us which it does.
			virtual BOOL UsesRadians() {return TRUE;};

			virtual float Size();
			virtual void Size(float fltVal, BOOL bUseScaling = TRUE);

			virtual BOOL EnableLimits();
			virtual void EnableLimits(BOOL bVal);

			virtual BOOL EnableMotor();
			virtual void EnableMotor(BOOL bVal);

			virtual float MaxVelocity();
			virtual void MaxVelocity(float fltVal, BOOL bUseScaling = TRUE);

			virtual int VisualSelectionType() {return JOINT_SELECTION_MODE;};

			RigidBody *Child() {return m_lpChild;};
			void Child(RigidBody *lpValue) {m_lpChild = lpValue;};

			virtual float JointPosition() {return m_fltPosition;};
			virtual void JointPosition(float fltPos) {m_fltPosition = fltPos;};
			virtual float JointVelocity() {return m_fltVelocity;};
			virtual void JointVelocity(float fltVel) {m_fltVelocity = fltVel;};
			virtual float JointForce() {return m_fltForce;};
			virtual void JointForce(float fltForce) {m_fltForce = fltForce;};

			float SetVelocity() {return m_fltSetVelocity;};
			float DesiredVelocity() {return m_fltDesiredVelocity;};
			virtual void DesiredVelocity(float fltVelocity) {m_fltDesiredVelocity = fltVelocity;};
			virtual void MotorInput(float fltInput) {m_fltDesiredVelocity = fltInput;}

			virtual void CreateJoint();

			//Node Overrides
			virtual void AddExternalNodeInput(float fltInput);
			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

			virtual void ResetSimulation();
			virtual void StepSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
