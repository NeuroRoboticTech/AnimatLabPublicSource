// Hinge.h: interface for the Hinge class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALHINGEJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
#define AFX_ALHINGEJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace AnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			/*! \brief 
				A hinge type of joint.
			   
				\remarks
				This type of joint is constrained so that it can only
				rotate about one axis. You can define which axis it rotates
				around in the configuration file using the normalized 
				RotationAxis vector element. You can also specify the
				rotational constraints for this joint. This prevents it
				from rotating further than the constrained value.

				Also, this joint is motorized. So you can specify a desired
				velocity of motion at a given time step using the CNlInjectionMgr
				and the physics engine will automatically apply the forces
				necessary to move the joint at the desired velocity.

				\sa
				Joint, Hinge, CAlStaticJoint
				 
				\ingroup AnimatSim
			*/

			class ANIMAT_PORT Hinge : public Joint    
			{
			protected:
				///This is the minimum radian value that the joint can rotate about its axis.
				///Its orginal position is used as zero radians.
				float m_fltConstraintLow;

				///This is the maximum radian value that the joint can rotate about its axis.
				///Its orginal position is used as zero radians.
				float m_fltConstraintHigh;

				float m_fltMaxTorque;

				float m_ftlServoGain;
				BOOL m_bServoMotor;

			public:
				Hinge();
				virtual ~Hinge();

				virtual float ConstraintLow() {return m_fltConstraintLow;};
				virtual void ConstraintLow(float fltVal) {m_fltConstraintLow = fltVal;};

				virtual float ConstraintHigh() {return m_fltConstraintHigh;};
				virtual void ConstraintHigh(float fltVal) {m_fltConstraintHigh = fltVal;};

				void ServoMotor(BOOL bServo) {m_bServoMotor = bServo;};
				BOOL ServoMotor() {return m_bServoMotor;};

				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

				virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			};

		}		//Joints
	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_ALHINGEJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
