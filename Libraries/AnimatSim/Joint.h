/**
\file	Joint.h

\brief	Declares the joint class.
**/

#pragma once

#include "ConstraintFriction.h"
#include "ConstraintRelaxation.h"

namespace AnimatSim
{
	namespace Environment
	{

		/**
		\brief	The base class for all of the joint type of objects.
		
		\details This class provides the base functionality for a joint. 
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
		perform. For instance, a HingeJoint is constrained to rotate
		about one axis just like the hinge on a door.<br><br>
		Some joints can be motorized, and some cannot. Hinge and Prismatic 
		joints are currently the only joints that are motorized. So all of the
		parameters related to motors are only relevant for those joint types.

		\author	dcofer
		\date	3/22/2011
		**/
		class ANIMAT_PORT Joint : public BodyPart  
		{
		protected:
			///The child rigid body for this joint. 
			RigidBody *m_lpChild;

			/// If true then any ConstraintLimits for this joint are enabled.
			bool m_bEnableLimits;

			/// The current position of the joint. This value can be in radians or meters 
			/// depending on the type of joint. UsesRadians lets you know whether it is using radians.
			float m_fltPosition;

			/// The current velocity of the joint.
			/// Velocities can be in rad/sec or m/s depending on the type of joint. UsesRadians lets you know whether it is using radians.
			float m_fltVelocity;

			/// The current force being applied to the joint by the motor.
			float m_fltForce;

			///Scales the size of the graphics for this joint.
			float m_fltSize;

            ///The relaxations for the constraints
            ConstraintRelaxation *m_aryRelaxations[6];

            ///The friction for this joint
            ConstraintFriction *m_lpFriction;

            virtual ConstraintRelaxation *LoadConstraintRelaxation(CStdXml &oXml, std::string strName);
            virtual ConstraintFriction *LoadConstraintFriction(CStdXml &oXml);
            virtual void ClearRelaxations();

		public:
			Joint();
			virtual ~Joint();

			virtual bool UsesRadians();

			virtual float Size();
			virtual void Size(float fltVal, bool bUseScaling = true);

			virtual bool EnableLimits();
			virtual void EnableLimits(bool bVal);

            virtual ConstraintRelaxation *Relaxation1();
            virtual void Relaxation1(ConstraintRelaxation *lpRelax);
			virtual void Relaxation1(std::string strXml);

            virtual ConstraintRelaxation *Relaxation2();
            virtual void Relaxation2(ConstraintRelaxation *lpRelax);
			virtual void Relaxation2(std::string strXml);

            virtual ConstraintRelaxation *Relaxation3();
            virtual void Relaxation3(ConstraintRelaxation *lpRelax);
			virtual void Relaxation3(std::string strXml);

            virtual ConstraintRelaxation *Relaxation4();
            virtual void Relaxation4(ConstraintRelaxation *lpRelax);
			virtual void Relaxation4(std::string strXml);

            virtual ConstraintRelaxation *Relaxation5();
            virtual void Relaxation5(ConstraintRelaxation *lpRelax);
			virtual void Relaxation5(std::string strXml);

            virtual ConstraintRelaxation *Relaxation6();
            virtual void Relaxation6(ConstraintRelaxation *lpRelax);
			virtual void Relaxation6(std::string strXml);

            virtual ConstraintFriction *Friction();
            virtual void Friction(ConstraintFriction *lpFriction);
			virtual void Friction(std::string strXml);

			virtual float GetPositionWithinLimits(float fltPos);
			virtual float GetLimitRange();

			virtual int VisualSelectionType();

			virtual RigidBody *Child();
			virtual void Child(RigidBody *lpValue);

			virtual float JointPosition();
			virtual void JointPosition(float fltPos);
			virtual float JointVelocity();
			virtual void JointVelocity(float fltVel);
			virtual float JointForce();
			virtual void JointForce(float fltForce);

            virtual void WakeDynamics();

            virtual void CreateJoint();
			virtual void UpdatePhysicsPosFromGraphics();
            virtual void Initialize();

			virtual float *GetDataPointer(const std::string &strDataType);
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

			virtual void AddExternalNodeInput(float fltInput);
			virtual void ResetSimulation();
			virtual void AfterResetSimulation();
			virtual void StepSimulation();
			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
