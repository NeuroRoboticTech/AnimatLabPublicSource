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
				btUniversalConstraint  *m_btSocket;

				virtual void SetupPhysics();
				virtual void DeletePhysics(bool bIncludeChildren);

			public:
				BlUniversal();
				virtual ~BlUniversal();

#pragma region DataAccesMethods

				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);

#pragma endregion

				virtual void CreateJoint();
			};

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
