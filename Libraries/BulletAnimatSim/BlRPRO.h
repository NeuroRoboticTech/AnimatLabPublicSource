/**
\file	BlRPRO.h

\brief	Declares the vortex relative position, relative orientation class.
**/

#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{
			/**
			\brief	Vortex relative position, relative orientation joint class.

			\details This class implements a relative position, relative orientation joint.
			
			\author	dcofer
			\date	4/15/2011
			**/
			class BULLET_PORT BlRPRO : public BlJoint, public AnimatSim::Environment::Joints::RPRO     
			{
			protected:
				/// The vortex socket class.
                //FIX PHYSICS
				//Vx::VxRPRO *m_vxSocket;

				virtual void SetupPhysics();
				virtual void DeletePhysics();

			public:
				BlRPRO();
				virtual ~BlRPRO();

#pragma region DataAccesMethods

				virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

#pragma endregion

				virtual void CreateJoint();
			};

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
