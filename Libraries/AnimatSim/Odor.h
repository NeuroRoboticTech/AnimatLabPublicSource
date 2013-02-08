/**
\file	Odor.h

\brief	Declares the odor class.
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		/**
		\brief	Odor. 
		
		\author	dcofer
		\date	3/23/2011
		**/
		class ANIMAT_PORT Odor : public AnimatBase 
		{
		protected:
			/// Pointer to the parent RigidBody part that is emitting the odor.
			RigidBody *m_lpParent;

			/// Pointer to the type of odor the body part is emitting.
			OdorType *m_lpOdorType;

				/// The quantity used to calculate the odor value. 
			float m_fltQuantity;

			/// If this is true then the food quantity of the parent RigidBody is used to calculate
			/// the odor strength instead of the m_fltQuantity of this odor object.
			BOOL m_bUseFoodQuantity;

		public:
			Odor(RigidBody *lpParent);
			virtual ~Odor();

			virtual void SetOdorType(string strType);
			virtual OdorType  *GetOdorType();

			virtual float Quantity();
			virtual void Quantity(float fltVal);

			virtual BOOL UseFoodQuantity();
			virtual void UseFoodQuantity(BOOL bVal);
			
			virtual float CalculateOdorValue(OdorType *lpType, CStdFPoint &oSensorPos);
						
			virtual BOOL SetData(const string &strDataType, const string &strValue, BOOL bThrowError = TRUE);
			virtual void QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes);

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
