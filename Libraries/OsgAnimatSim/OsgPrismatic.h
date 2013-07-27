/**
\file	OsgPrismatic.h

\brief	Declares the vs prismatic class.
**/

#pragma once

namespace OsgAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class ANIMAT_OSG_PORT OsgPrismatic : public VsMotorizedJoint, public AnimatSim::Environment::Joints::Prismatic     
			{
			protected:
				Vx::VxPrismatic *m_vxPrismatic;

       			virtual void DeleteJointGraphics();
                virtual void CreateJointGraphics();
				virtual void SetupGraphics();
				virtual void SetupPhysics();
				virtual void DeletePhysics();

			public:
				OsgPrismatic();
				virtual ~OsgPrismatic();

				virtual void JointPosition(float fltPos);

				virtual void SetAlpha();

#pragma region DataAccesMethods

				virtual float *GetDataPointer(const string &strDataType);
				virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

#pragma endregion

				virtual void EnableLimits(BOOL bVal);
				virtual void CreateJoint();
				virtual void StepSimulation();
			};

		}		//Joints
	}			// Environment
}				//OsgAnimatSim
