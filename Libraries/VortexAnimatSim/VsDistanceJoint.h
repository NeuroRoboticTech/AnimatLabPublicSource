/**
\file	VsDistanceJoint.h

\brief	Declares the vortex distance joint class.
**/

#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

			class VORTEX_PORT VsDistanceJoint : public VortexAnimatSim::Environment::VsJoint, public AnimatSim::Environment::Joint     
			{
			protected:
				/// The vortex socket class.
				Vx::VxDistanceJoint *m_vxDistance;

				virtual void SetupGraphics();
				virtual void SetupPhysics();
				virtual void DeletePhysics();

			public:
				VsDistanceJoint();
				virtual ~VsDistanceJoint();

#pragma region DataAccesMethods

				virtual bool SetData(const string &strDataType, const string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

#pragma endregion

				virtual void CreateJoint();
			};

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
