/**
\file	VsHudText.h

\brief	Declares the vortex heads-up display text class.
**/

#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class VORTEX_PORT VsHudText : public AnimatSim::HudText  
		{
		protected:
			osg::ref_ptr<osgText::Text> m_osgText;
			osg::ref_ptr<osg::Geode> m_osgGeode;

		public:
			VsHudText();
			VsHudText(float *aryColor, CStdFPoint &ptPosition, string strFont, int iCharSize, string strText, string strTargetID, string strDataType);
			virtual ~VsHudText();

			osgText::Text *OsgText() {return m_osgText.get();};
			osg::Geode *Geode() {return m_osgGeode.get();};

			virtual void Initialize(void *lpVoidProjection);
			virtual void Update();
		};

	}			// Visualization
}				//VortexAnimatSim
