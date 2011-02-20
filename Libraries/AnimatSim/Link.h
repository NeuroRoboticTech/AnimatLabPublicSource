// Link.h: interface for the Adapter class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_LINK_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
#define AFX_LINK_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_

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

	class ANIMAT_PORT Link : public AnimatBase 
	{
	protected:
		Simulator *m_lpSim;
		Structure *m_lpStructure;
		NeuralModule *m_lpModule;
		Node *m_lpNode;

		///Determines if this Link is enabled. This will only have any effect if this Link can be disabled.
		///The majority of Links, like rigid bodies, can not be disabled.
		BOOL m_bEnabled;

		///This is for reporting purposes.
		float m_fltEnabled;

		virtual void UpdateData(Simulator *lpSim, Structure *lpStructure);

	public:
		Link();
		virtual ~Link();

		virtual BOOL Enabled() {return m_bEnabled;};
		virtual void Enabled(BOOL bValue) 
		{
			m_bEnabled = bValue;
			m_fltEnabled = (float) m_bEnabled;
		};

		virtual void ResetSimulation(Simulator *lpSim, Structure *lpStructure, Node *lpNode) = 0;
		virtual void AfterResetSimulation(Simulator *lpSim, Structure *lpStructure) {};

		virtual float *GetDataPointer(string strDataType) = 0;
		virtual void *GetDataItem(string strItemType, string strID, BOOL bThrowError = TRUE); 
		virtual void Initialize(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode);
		virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode);
		virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure, Node *lpNode) = 0;
		virtual void Load(Simulator *lpSim, Structure *lpStructure, Node *lpNode, CStdXml &oXml);
		virtual void Save(Simulator *lpSim, Structure *lpStructure, Node *lpNode, CStdXml &oXml) {};
	};

}				//AnimatSim

#endif // !defined(AFX_LINK_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
