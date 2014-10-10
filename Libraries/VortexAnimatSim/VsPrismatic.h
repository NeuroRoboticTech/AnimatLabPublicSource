/**
\file	VsPrismatic.h

\brief	Declares the vs prismatic class.
**/

#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class VORTEX_PORT VsPrismatic : public VsMotorizedJoint, public AnimatSim::Environment::Joints::Prismatic     
			{
			protected:
				Vx::VxPrismatic *m_vxPrismatic;

       			virtual void DeleteJointGraphics();
                virtual void CreateJointGraphics();
				virtual void SetupGraphics();
				virtual void SetupPhysics();
				virtual void DeletePhysics();

			public:
				VsPrismatic();
				virtual ~VsPrismatic();

				virtual void JointPosition(float fltPos);

				virtual void SetAlpha();

#pragma region DataAccesMethods

				virtual float *GetDataPointer(const std::string &strDataType);
				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

				virtual void EnableLimits(bool bVal);
				virtual void CreateJoint();
				virtual void StepSimulation();
			};

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
