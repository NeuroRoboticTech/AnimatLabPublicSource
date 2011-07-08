/**
// file:	HudText.h
//
// summary:	Declares the heads-up display text class
**/

#pragma once

namespace AnimatSim
{

	class ANIMAT_PORT HudText : public HudItem 
	{
	protected:
		string m_strTargetID;
		string m_strDataType;

		CStdColor m_aryColor;
		CStdFPoint m_ptPosition;
		string m_strFont;
		int m_iCharSize;
		string m_strText;
		float *m_lpData;

	public:
		HudText();
		HudText(float *aryColor, CStdFPoint &ptPosition, string strFont, int iCharSize, string strText, string strTargetID, string strDataType);
		virtual ~HudText();

		virtual void Initialize(void *lpVoidProjection);
		virtual void Load(CStdXml &oXml);
	};

}				//AnimatSim
