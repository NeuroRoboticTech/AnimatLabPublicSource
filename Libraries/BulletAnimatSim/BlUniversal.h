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
				/// The bullet socket class.
                //I switched this to use the cone twist constraint instead of the bullet universal constraint because
                // tests showed that their universal constraint did not work well at all. Even slight forces to the joint
                // caused it to blow up. I will probably need to address this at some point, but for now a ball-socket is a good enough approx.
				btConeTwistConstraint  *m_btSocket;

				virtual void SetupPhysics();

			public:
				BlUniversal();
				virtual ~BlUniversal();

#pragma region DataAccesMethods

				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

				virtual void CreateJoint();
    			virtual void Physics_ResetSimulation();
			};

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
