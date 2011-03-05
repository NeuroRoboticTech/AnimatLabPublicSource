// Link.h: interface for the Adapter class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_LINK_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
#define AFX_LINK_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace AnimatSim
{

	class ANIMAT_PORT Link : public AnimatBase 
	{
	protected:
		/// The pointer to this link's organism
		Organism *m_lpOrganism;

		///Determines if this Link is enabled. This will only have any effect if this Link can be disabled.
		///The majority of Links, like rigid bodies, can not be disabled.
		BOOL m_bEnabled;

		///This is for reporting purposes.
		float m_fltEnabled;

		virtual void UpdateData();

	public:
		Link();
		virtual ~Link();

		virtual BOOL Enabled() {return m_bEnabled;};
		virtual void Enabled(BOOL bValue) 
		{
			m_bEnabled = bValue;
			m_fltEnabled = (float) m_bEnabled;
		};

		virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify = TRUE);
		virtual void VerifySystemPointers();
	};

}				//AnimatSim

#endif // !defined(AFX_LINK_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
