/**
\file	VsUniversal.h

\brief	Declares the vs universal class.
**/

#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class VORTEX_PORT VsUniversal : public VortexAnimatSim::Environment::VsJoint, public AnimatSim::Environment::Joints::BallSocket     
			{
			protected:
				/// The vortex socket class.
				Vx::VxHomokinetic *m_vxSocket;

				virtual void SetupPhysics();
				virtual void DeletePhysics();

			public:
				VsUniversal();
				virtual ~VsUniversal();

#pragma region DataAccesMethods

				virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

#pragma endregion

				virtual void CreateJoint();
			};

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
