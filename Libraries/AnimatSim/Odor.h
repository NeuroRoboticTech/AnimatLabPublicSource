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
			bool m_bUseFoodQuantity;

			//Enable the odor
			bool m_bEnabled;

		public:
			Odor(RigidBody *lpParent);
			virtual ~Odor();

			virtual void Enabled(bool bEnabled);
			virtual bool Enabled();

			virtual void SetOdorType(std::string strType);
			virtual OdorType  *GetOdorType();

			virtual float Quantity();
			virtual void Quantity(float fltVal);

			virtual bool UseFoodQuantity();
			virtual void UseFoodQuantity(bool bVal);
			
			virtual float CalculateOdorValue(OdorType *lpType, CStdFPoint &oSensorPos);
						
			virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
			virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
