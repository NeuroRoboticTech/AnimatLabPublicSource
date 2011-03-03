// ContactAdapter.h: interface for the ContactAdapter class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_CONTACT_ADAPTER_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
#define AFX_CONTACT_ADAPTER_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace AnimatSim
{
	namespace Adapters
	{

		class ANIMAT_PORT ContactAdapter : public Adapter 
		{
		protected:
			string m_strSourceBodyID;
			string m_strTargetModule;
			CStdPtrArray<ReceptiveFieldPair> m_aryFieldPairs; 

			ReceptiveFieldPair *LoadFieldPair(CStdXml &oXml);

		public:
			ContactAdapter();
			virtual ~ContactAdapter();

			virtual void Initialize(Simulator *lpSim, Structure *lpStructure);
			virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
			virtual string SourceModule();
			virtual string TargetModule();

			//Node Overrides
			virtual void Load(CStdXml &oXml);
		};

	}			//Adapters
}				//AnimatSim

#endif // !defined(AFX_CONTACT_ADAPTER_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
