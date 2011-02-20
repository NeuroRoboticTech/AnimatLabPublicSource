// BallSocket.h: interface for the BallSocket class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_BALLSOCKETJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
#define AFX_BALLSOCKETJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_

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
				A BallSocket type of joint.
			   
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
				Joint, BallSocket, CAlStaticJoint
				 
				\ingroup AnimatSim
			*/

			class ANIMAT_PORT BallSocket : public Joint    
			{
			protected:
				///The "direction" the ball and socket is facing.  That is which way the cone limit points
				///This should be normalized.
				CStdFPoint m_oConstraintAxis;

				///This is the minimum radian value that the joint can rotate about its axis.
				///Its orginal position is used as zero radians.
				float m_fltConstraintAngle;

				///This is the maximum radian value that the joint can rotate about its axis.
				///Its orginal position is used as zero radians.
				float m_fltStiffness;
				float m_fltDamping;

			public:
				BallSocket();
				virtual ~BallSocket();

				virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			};

		}		//Joints
	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_ALBALLSOCKETJOINT_H__FB4AFDAA_982E_4893_83F3_05BFF60F5643__INCLUDED_)
