/**
\file	RbRPRO.h

\brief	Declares the vortex relative position, relative orientation class.
**/

#pragma once

namespace RoboticsAnimatSim
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
			class ROBOTICS_PORT RbRPRO : public RbJoint, public AnimatSim::Environment::Joints::RPRO     
			{
			protected:

			public:
				RbRPRO();
				virtual ~RbRPRO();

#pragma region DataAccesMethods

				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);

#pragma endregion

				virtual void CreateJoint();
			};

		}		//Joints
	}			// Environment
}				//RoboticsAnimatSim
