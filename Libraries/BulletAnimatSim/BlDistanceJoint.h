/**
\file	BlDistanceJoint.h

\brief	Declares the vortex distance joint class.
**/

#pragma once

namespace BulletAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class BULLET_PORT BlDistanceJoint : public BulletAnimatSim::Environment::BlJoint, public AnimatSim::Environment::Joint     
			{
			protected:
				/// The bullet socket class.
				btGeneric6DofConstraint *m_btDistance;

			public:
				BlDistanceJoint();
				virtual ~BlDistanceJoint();

				virtual void SetupGraphics();
				virtual void SetupPhysics();

#pragma region DataAccesMethods

				virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

#pragma endregion

				virtual void CreateJoint();
			};

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
