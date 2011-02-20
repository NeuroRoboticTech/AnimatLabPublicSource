// ActivatedItemMgr.h: interface for the ActivatedItemMgr class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ACTIVATEDITEMMGR_H__CBDA72E7_469E_4C42_BEF4_41F1D7F40627__INCLUDED_)
#define AFX_ACTIVATEDITEMMGR_H__CBDA72E7_469E_4C42_BEF4_41F1D7F40627__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatSim
{

	class ANIMAT_PORT ActivatedItemMgr : public AnimatBase    
	{
	protected:
		CStdArray<ActivatedItem *> m_aryItems;
		CStdPtrMap<string, ActivatedItem> m_aryItemsMap;

	public:
		ActivatedItemMgr();
		virtual ~ActivatedItemMgr();

		virtual void Add(Simulator *lpSim, ActivatedItem *lpItem);
		virtual void Remove(Simulator *lpSim, string strID, BOOL bThrowError = TRUE);
		virtual ActivatedItem *Find(string strID, int &iIndex, BOOL bThrowError);
		virtual ActivatedItem *Find(string strID, BOOL bThrowError = TRUE);
		virtual int FindListPos(string strID, BOOL bThrowError = TRUE);

		virtual void Reset();
		virtual void Initialize(Simulator *lpSim);
		virtual void ResetSimulation(Simulator *lpSim);
		virtual void ReInitialize(Simulator *lpSim);
		virtual void StepSimulation(Simulator *lpSim);
		virtual void Load(Simulator *lpSim, CStdXml &oXml) = 0;
	};

}			//AnimatSim

#endif // !defined(AFX_ACTIVATEDITEMMGR_H__CBDA72E7_469E_4C42_BEF4_41F1D7F40627__INCLUDED_)
