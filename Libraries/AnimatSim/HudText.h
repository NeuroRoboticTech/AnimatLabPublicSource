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
		std::string m_strDisplayTargetID;
		std::string m_strDisplayDataType;
		std::string m_strUpdateTargetID;
		std::string m_strUpdateDataType;

		float m_fltUpdateInterval;

		CStdColor m_aryColor;
		CStdFPoint m_ptPosition;
		std::string m_strFont;
		int m_iCharSize;
		std::string m_strText;
		float *m_lpDisplayData;
		float *m_lpUpdateData;

	public:
		HudText();
		HudText(float *aryColor, CStdFPoint &ptPosition, std::string strFont, int iCharSize, std::string strText, std::string strDisplayTargetID, std::string strDisplayDataType, std::string strUpdateTargetID, std::string strUpdateDataType, float fltUpdateInterval);
		virtual ~HudText();
			
		static HudText *CastToDerived(AnimatBase *lpBase) {return static_cast<HudText*>(lpBase);}

		virtual void Initialize(void *lpVoidProjection);
		virtual void Load(CStdXml &oXml);
	};

}				//AnimatSim
