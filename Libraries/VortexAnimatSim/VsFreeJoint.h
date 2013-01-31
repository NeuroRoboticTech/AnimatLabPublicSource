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

			class VORTEX_PORT VsFreeJoint : public VsJoint, public BallSocket     
			{
			protected:

				virtual void SetupPhysics();
				virtual void DeletePhysics();

			public:
				VsFreeJoint();
				virtual ~VsFreeJoint();

#pragma region DataAccesMethods

				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
				virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

#pragma endregion

				virtual void CreateJoint();
			};

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
