#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class VsDraggerHandler : public osgGA::GUIEventHandler
		{
			private:
				Simulator *m_lpSim;

				osg::ref_ptr<osgViewer::Viewer> m_osgViewer;
				osg::ref_ptr<osg::Viewport> m_osgViewport;

		        osgManipulator::Dragger *m_osgActiveDragger;
			    osgManipulator::PointerInfo m_osgManipInfo;

				virtual void EndGripDrag();

			protected:
				void pick(const osgGA::GUIEventAdapter& ea, GUIActionAdapter& aa);

			public:
				VsDraggerHandler(Simulator *lpSim, osgViewer::Viewer *osgViewer, osg::Viewport *osgViewport = NULL);
				~VsDraggerHandler(void);

				  /** Handle events, return true if handled, false otherwise. */
				virtual bool handle(const GUIEventAdapter& ea,GUIActionAdapter& us);
		};

	}// end Visualization
}// end VortexAnimatSim

