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

				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

#pragma endregion

				virtual void CreateJoint();
			};

		}		//Joints
	}			// Environment
}				//VortexAnimatSim
