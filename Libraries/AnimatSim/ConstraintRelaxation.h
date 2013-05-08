/**
\file	ConstraintRelaxation.h

\brief	Declares the material pair class.
**/

#pragma once

namespace AnimatSim
{

	namespace Environment
	{
		/**
		\brief	A material type name.
				
		\author	dcofer
		\date	3/22/2011
		**/
		class ANIMAT_PORT ConstraintRelaxation : public AnimatBase
		{
		protected:
            ///The constraint coordinate ID 
            int m_iCoordinateID;

            ///whether the relaxation is enabled or not
            BOOL m_bEnabled;

			/// The compliance of the collision between those two materials.
			float m_fltStiffness;

			/// The damping of the collision between those two materials.
			float m_fltDamping;

			/// The primary linear slip of the collision between those two materials.
			float m_fltLoss;

			virtual void SetRelaxationProperties() = 0;

		public:
			ConstraintRelaxation();
			virtual ~ConstraintRelaxation();

            virtual int CoordinateID();
            virtual void CoordinateID(int iVal);

            virtual BOOL Enabled();
            virtual void Enabled(BOOL bVal);

    		virtual float Stiffness();
			virtual void Stiffness(float fltVal, BOOL bUseScaling = TRUE);

			virtual float Damping();
			virtual void Damping(float fltVal, BOOL bUseScaling = TRUE);

			virtual float Loss();
			virtual void Loss(float fltVal, BOOL bUseScaling = TRUE);

			virtual void CreateDefaultUnits();
			virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

			virtual void Load(CStdXml &oXml);
        };

	}			// Visualization
}				//VortexAnimatSim
