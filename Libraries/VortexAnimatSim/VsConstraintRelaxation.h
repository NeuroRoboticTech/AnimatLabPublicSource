// VsConstraintRelaxation.h: interface for the VsConstraintRelaxation class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace VortexAnimatSim
{
	namespace Environment
	{

		class VORTEX_PORT VsConstraintRelaxation : public AnimatSim::Environment::ConstraintRelaxation
		{
		protected:
			/// The primary linear slip of the collision between those two materials.
			float m_fltLoss;

			virtual void SetRelaxationProperties();

		public:
			VsConstraintRelaxation();
			virtual ~VsConstraintRelaxation();

			virtual float Loss();
			virtual void Loss(float fltVal, bool bUseScaling = true);

            virtual void Initialize();

			virtual void CreateDefaultUnits();
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdArray<std::string> &aryNames, CStdArray<std::string> &aryTypes);

			virtual void Load(CStdXml &oXml);
        };

	}			// Visualization
}				//VortexAnimatSim
