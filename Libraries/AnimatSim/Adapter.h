// Adapter.h: interface for the Adapter class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ADAPTER_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
#define AFX_ADAPTER_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_

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
	namespace Adapters
	{

		class ANIMAT_PORT Adapter : public Node 
		{
		protected:
			string m_strSourceModule;
			string m_strSourceID;
			string m_strSourceDataType;
			Node *m_lpSourceNode;
			float *m_lpSourceData;

			string m_strTargetModule;
			string m_strTargetID;
			string m_strTargetDataType;
			Node *m_lpTargetNode;

			Gain *m_lpGain;

		public:
			Adapter();
			virtual ~Adapter();

			virtual void Initialize(Simulator *lpSim, Structure *lpStructure);
			virtual string SourceModule() {return m_strSourceModule;};
			virtual string TargetModule() {return m_strTargetModule;};

			//Node Overrides
			virtual void AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput);
			virtual void AttachSourceAdapter(Simulator *lpSim, Structure *lpStructure, Node *lpNode);
			virtual void AttachTargetAdapter(Simulator *lpSim, Structure *lpStructure, Node *lpNode);
			virtual float *GetDataPointer(string strDataType);
			virtual void ResetSimulation(Simulator *lpSim, Structure *lpStructure) {};
			virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
			virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			virtual void Save(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
		};

	}			//Adapters
}				//AnimatSim

#endif // !defined(AFX_ADAPTER_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
