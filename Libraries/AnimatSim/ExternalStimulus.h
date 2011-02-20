// ExternalStimulus.h: interface for the ExternalStimulus class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_EXTERNALSTIMULUS_H__5DB0175F_4EEF_4DEC_91CC_AAE31CBBB695__INCLUDED_)
#define AFX_EXTERNALSTIMULUS_H__5DB0175F_4EEF_4DEC_91CC_AAE31CBBB695__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatSim
{
	namespace ExternalStimuli
	{

		class ANIMAT_PORT ExternalStimulus : public ActivatedItem 
		{   
		public:
			ExternalStimulus();
			virtual ~ExternalStimulus();

			float m_fltInput;

			virtual float *GetDataPointer(string strDataType) {return NULL;};
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

			//ActiveItem overrides
			virtual void Load(Simulator *lpSim, CStdXml &oXml) = 0;
			virtual void Save(Simulator *lpSim, CStdXml &oXml) = 0;
			virtual BOOL operator<(ActivatedItem *lpItem);
		};

	}			//ExternalStimuli
}				//AnimatSim

#endif // !defined(AFX_EXTERNALSTIMULUS_H__5DB0175F_4EEF_4DEC_91CC_AAE31CBBB695__INCLUDED_)
