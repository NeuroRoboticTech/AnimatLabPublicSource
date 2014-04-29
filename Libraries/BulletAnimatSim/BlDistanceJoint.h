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

				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

				virtual void CreateJoint();
			};

		}		//Joints
	}			// Environment
}				//BulletAnimatSim
