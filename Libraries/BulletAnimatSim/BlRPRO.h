/**
\file	BlRPRO.h

\brief	Declares the vortex relative position, relative orientation class.
**/

#pragma once

#include "btAnimatGeneric6DofConstraint.h"

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
				btAnimatGeneric6DofConstraint *m_btSocket;

			public:
				BlRPRO();
				virtual ~BlRPRO();

				virtual void SetupPhysics();

#pragma region DataAccesMethods

				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);

#pragma endregion

				virtual void CreateJoint();
			};

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
