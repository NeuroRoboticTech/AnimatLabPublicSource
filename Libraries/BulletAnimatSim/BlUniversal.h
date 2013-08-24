/**
\file	BlUniversal.h

\brief	Declares the vs universal class.
**/

#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class BULLET_PORT BlUniversal : public BulletAnimatSim::Environment::BlJoint, public AnimatSim::Environment::Joints::BallSocket     
			{
			protected:
				/// The vortex socket class.
				Vx::VxHomokinetic *m_vxSocket;

				virtual void SetupPhysics();
				virtual void DeletePhysics();

			public:
				BlUniversal();
				virtual ~BlUniversal();

#pragma region DataAccesMethods

				virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

#pragma endregion

				virtual void CreateJoint();
			};

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
