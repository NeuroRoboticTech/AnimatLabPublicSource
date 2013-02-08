/**
\file	VsRPRO.h

\brief	Declares the vortex relative position, relative orientation class.
**/

#pragma once

namespace VortexAnimatSim
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
			class VORTEX_PORT VsRPRO : public VsJoint, public AnimatSim::Environment::Joints::RPRO     
			{
			protected:
				/// The vortex socket class.
				Vx::VxRPRO *m_vxSocket;

				virtual void SetupPhysics();
				virtual void DeletePhysics();

			public:
				VsRPRO();
				virtual ~VsRPRO();

#pragma region DataAccesMethods

				virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

#pragma endregion

				virtual void CreateJoint();
			};

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
