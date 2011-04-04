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
				float m_fltDistanceUnits;

			public:
				VsPrismatic();
				virtual ~VsPrismatic();
				virtual void CreateJoint();
				virtual float *GetDataPointer(string strDataType);
				virtual void StepSimulation();
			};

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
