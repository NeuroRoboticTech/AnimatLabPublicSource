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
				/// The vortex socket class.
                 //FIX PHYSICS
				//Vx::VxDistanceJoint *m_vxDistance;

				virtual void SetupGraphics();
				virtual void SetupPhysics();
				virtual void DeletePhysics();

			public:
				BlDistanceJoint();
				virtual ~BlDistanceJoint();

#pragma region DataAccesMethods

				virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

#pragma endregion

				virtual void CreateJoint();
			};

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
