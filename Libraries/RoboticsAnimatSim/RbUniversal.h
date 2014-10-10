/**
\file	RbUniversal.h

\brief	Declares the vs universal class.
**/

#pragma once

namespace RoboticsAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class ROBOTICS_PORT RbUniversal : public RoboticsAnimatSim::Environment::RbJoint, public AnimatSim::Environment::Joints::BallSocket     
			{
			protected:

			public:
				RbUniversal();
				virtual ~RbUniversal();

#pragma region DataAccesMethods

				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

				virtual void CreateJoint();
			};

		}		//Joints
	}			// Environment
}				//RoboticsAnimatSim
