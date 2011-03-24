/**
\file	C:\Projects\AnimatLabSDK\AnimatLabPublicSource\Libraries\AnimatSim\BallSocket.h

\brief	Declares the ball socket class.
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			/**
			\brief	A BallSocket type of joint.
			
			\details This joint allows free rotation around a point in space, and it allows 
			rotation around line of the joint, but it does allow translations to occur. In order 
			to constrain the motion of the joint you must place rigid bodies around it to prevent its
			movement. Also, this joint type is not motorized.

			\author	dcofer
			\date	3/24/2011
			**/
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

				virtual void Load(CStdXml &oXml);
			};

		}		//Joints
	}			// Environment
}				//AnimatSim
