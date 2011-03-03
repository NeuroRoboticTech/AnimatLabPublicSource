// ExternalStimuliMgr.h: interface for the ExternalStimuliMgr class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_EXTERNALSTIMULIMGR_H__16DEACA1_2EE0_457B_893F_DC0526F2D544__INCLUDED_)
#define AFX_EXTERNALSTIMULIMGR_H__16DEACA1_2EE0_457B_893F_DC0526F2D544__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatSim
{
	namespace ExternalStimuli
	{

		class ANIMAT_PORT ExternalStimuliMgr : public ActivatedItemMgr    
		{
		protected:

			ExternalStimulus *LoadExternalStimuli(CStdXml &oXml);

		public:
			ExternalStimuliMgr();
			virtual ~ExternalStimuliMgr();

			virtual BOOL AddStimulus(string strXml);
			virtual BOOL RemoveStimulus(string strID);

			virtual void Load(string strProjectPath, string strFileName);
			virtual void Load(CStdXml &oXml);
		};

	}			//ExternalStimuli
}				//AnimatSim

#endif // !defined(AFX_EXTERNALSTIMULIMGR_H__16DEACA1_2EE0_457B_893F_DC0526F2D544__INCLUDED_)
