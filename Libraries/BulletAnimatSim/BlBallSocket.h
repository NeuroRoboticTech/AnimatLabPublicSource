/**
\file	BlBallSocket.h

\brief	Declares the vortex ball socket class.
**/

#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{
			/**
			\brief	Vortex ball-and-socket joint class.

			\details This class implements a spherical, or ball and socket joint.
			This type of joint prevents linear motion for all three dimensions, but
			allows angular motion around all three axises. This type of joint does
			not have any contraint limits available for it. To prevent motion for
			this joint you must use rigid bodies within the simulation to constrain
			the movement. This joit type also is not motorized. It can only be passively
			moved, or it can be controlled through muscle movements.
			
			\author	dcofer
			\date	4/15/2011
			**/
			class BULLET_PORT BlBallSocket : public BlJoint, public AnimatSim::Environment::Joints::BallSocket     
			{
			protected:
				/// The vortex socket class.
				Vx::VxBallAndSocket *m_vxSocket;

				virtual void SetupPhysics();
				virtual void DeletePhysics();

			public:
				BlBallSocket();
				virtual ~BlBallSocket();

#pragma region DataAccesMethods

				virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

#pragma endregion

				virtual void CreateJoint();
			};

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
