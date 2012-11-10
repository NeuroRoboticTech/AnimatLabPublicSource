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
		string m_strDisplayTargetID;
		string m_strDisplayDataType;
		string m_strUpdateTargetID;
		string m_strUpdateDataType;

		float m_fltUpdateInterval;

		CStdColor m_aryColor;
		CStdFPoint m_ptPosition;
		string m_strFont;
		int m_iCharSize;
		string m_strText;
		float *m_lpDisplayData;
		float *m_lpUpdateData;

	public:
		HudText();
		HudText(float *aryColor, CStdFPoint &ptPosition, string strFont, int iCharSize, string strText, string strDisplayTargetID, string strDisplayDataType, string strUpdateTargetID, string strUpdateDataType, float fltUpdateInterval);
		virtual ~HudText();

		virtual void Initialize(void *lpVoidProjection);
		virtual void Load(CStdXml &oXml);
	};

}				//AnimatSim
