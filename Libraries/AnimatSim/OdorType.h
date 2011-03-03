// OdorType.h: interface for the OdorType class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ODOR_TYPE_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
#define AFX_ODOR_TYPE_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace AnimatSim
{
	namespace Environment
	{
		class Odor;

		class ANIMAT_PORT OdorType : public AnimatBase 
		{
		protected:
				///The unique Id for this OdorType. It is unique for each structure, 
				///but not across structures. So you could have two rigid bodies with the
				///same ID in two different organisms.
				string m_strID;  

				///The name for this body. 
				string m_strName;  
				float m_fltDiffusionConstant;

				CStdMap<string, Odor *> m_aryOdorSources;

		public:
			OdorType();
			virtual ~OdorType();

			string ID() {return m_strID;};
			void ID(string strValue) {m_strID = strValue;};

			string Name() {return m_strName;};
			void Name(string strValue) {m_strName = strValue;};

			float DiffusionConstant() {return m_fltDiffusionConstant;};
			void DiffusionConstant(float fltVal) {m_fltDiffusionConstant = fltVal;};

			Odor *FindOdorSource(string strOdorID, BOOL bThrowError = TRUE);
			void AddOdorSource(Odor *lpOdor);
			
			float CalculateOdorValue(CStdFPoint &oSensorPos);

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim

#endif // !defined(AFX_ODOR_TYPE_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
