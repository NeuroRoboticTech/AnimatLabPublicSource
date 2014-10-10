/**
\file	OsgHudText.h

\brief	Declares the vortex heads-up display text class.
**/

#pragma once

namespace OsgAnimatSim
{
	namespace Visualization
	{

		class ANIMAT_OSG_PORT OsgHudText : public AnimatSim::HudText  
		{
		protected:
			float m_fltPrevious;
			osg::ref_ptr<osgText::Text> m_osgText;
			osg::ref_ptr<osg::Geode> m_osgGeode;

		public:
			OsgHudText();
			OsgHudText(float *aryColor, CStdFPoint &ptPosition, std::string strFont, int iCharSize, std::string strText, std::string strDisplayTargetID, std::string strDisplayDataType, std::string strUpdateTargetID, std::string strUpdateDataType, float fltUpdateInterval);
			virtual ~OsgHudText();

			virtual void ResetSimulation();

			osgText::Text *OsgText() {return m_osgText.get();};
			osg::Geode *Geode() {return m_osgGeode.get();};

			virtual void Initialize(void *lpVoidProjection);
			virtual void Update();
		};

	}			// Visualization
}				//OsgAnimatSim
