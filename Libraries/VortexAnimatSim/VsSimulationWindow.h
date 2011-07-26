#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

class VORTEX_PORT VsSimulationWindow : public AnimatSim::SimulationWindow, osgGA::GUIEventHandler 
{
	protected:
		VsSimulationWindowMgr *m_lpWinMgr;

		osg::ref_ptr<osgViewer::Viewer> m_osgViewer;

		osg::ref_ptr<osgGA::TrackballManipulator> m_osgManip;

		BodyPart *m_lpTrackBody;

		virtual void InitEmbedded(Simulator *lpSim, VsSimulator *lpVsSim);
		virtual void InitStandalone(Simulator *lpSim, VsSimulator *lpVsSim);
		virtual void TrackCamera();

	public:
		VsSimulationWindow(void);
		virtual ~VsSimulationWindow(void);

		virtual BodyPart *TrackBody() {return m_lpTrackBody;};
		virtual osg::Matrix GetScreenMatrix();
		virtual osg::Viewport* GetViewport();
		virtual osgViewer::Viewer *Viewer() {return m_osgViewer.get();};

		virtual void SetupTrackCamera();
		virtual void SetCameraLookAt(CStdFPoint oTarget);
		
		virtual void UpdateBackgroundColor();

		virtual void Initialize();
		virtual void Update();
		virtual void Close();

		virtual void OnGetFocus();
		virtual void OnLoseFocus();
};

	}// end Visualization
}// end VortexAnimatSim

