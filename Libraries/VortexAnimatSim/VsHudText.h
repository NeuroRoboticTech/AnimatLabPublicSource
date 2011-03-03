// VsHudText.h: interface for the VsHudText class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class VORTEX_PORT VsHudText : public VsHudItem 
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

			osg::ref_ptr<osgText::Text> m_osgText;
			osg::ref_ptr<osg::Geode> m_osgGeode;

		public:
			VsHudText();
			VsHudText(float *aryColor, CStdFPoint &ptPosition, string strFont, int iCharSize, string strText, string strTargetID, string strDataType);
			virtual ~VsHudText();

			osgText::Text *OsgText() {return m_osgText.get();};
			osg::Geode *Geode() {return m_osgGeode.get();};

			virtual void Initialize(osg::Projection *lpProjection);
			virtual void Update();
			virtual void Load(CStdXml &oXml);
		};

	}			// Visualization
}				//VortexAnimatSim
