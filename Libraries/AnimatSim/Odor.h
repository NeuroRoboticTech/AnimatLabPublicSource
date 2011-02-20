// Odor.h: interface for the Odor class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ODOR_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
#define AFX_ODOR_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

/*! \brief 
   xxxx.

   \remarks
   xxxx
		 
   \sa
	 xxx
	 
	 \ingroup AnimatSim
*/

namespace AnimatSim
{
	namespace Environment
	{

		class ANIMAT_PORT Odor : public AnimatBase 
		{
		protected:
				RigidBody *m_lpParent;
				OdorType *m_lpOdorType;

				///The unique Id for this odor. It is unique for each structure, 
				///but not across structures. So you could have two rigid bodies with the
				///same ID in two different organisms.
				string m_strID;  

				///The name for this body. 
				string m_strName;  

				float m_fltQuantity;
				BOOL m_bUseFoodQuantity;

		public:
			Odor(RigidBody *lpParent);
			virtual ~Odor();

			string ID() {return m_strID;};
			void ID(string strValue) {m_strID = strValue;};

			string Name() {return m_strName;};
			void Name(string strValue) {m_strName = strValue;};

			float Quantity();
			void Quantity(float fltVal) {m_fltQuantity = fltVal;};

			BOOL UseFoodQuantity() {return m_bUseFoodQuantity;};
			void UseFoodQuantity(BOOL bVal) {m_bUseFoodQuantity = bVal;};
			
			float CalculateOdorValue(Simulator *lpSim, OdorType *lpType, CStdFPoint &oSensorPos);

			virtual void Load(Simulator *lpSim, CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_ODOR_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
