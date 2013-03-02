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

		//osg::ref_ptr<VsStatsHandler> m_osgStatsHandler;

		BodyPart *m_lpTrackBody;

		float m_fltCameraPosX, m_fltCameraPosY, m_fltCameraPosZ;

		virtual void InitEmbedded(Simulator *lpSim, VsSimulator *lpVsSim);
		virtual void InitStandalone(Simulator *lpSim, VsSimulator *lpVsSim);
		virtual void TrackCamera();

	public:
		VsSimulationWindow(void);
		virtual ~VsSimulationWindow(void);

		virtual CStdFPoint GetCameraPosition();

		virtual BodyPart *TrackBody() {return m_lpTrackBody;};
		virtual osg::Matrix GetScreenMatrix();
		virtual osg::Viewport* GetViewport();
		virtual osgViewer::Viewer *Viewer() {return m_osgViewer.get();};

		virtual void SetupTrackCamera();
		virtual void SetCameraLookAt(CStdFPoint oTarget);
		virtual void SetCameraPositionAndLookAt(CStdFPoint oCameraPos, CStdFPoint oTarget);
		virtual void SetCameraPositionAndLookAt(osg::Vec3d vCameraPos, osg::Vec3d vTarget);
		virtual void SetCameraPostion(CStdFPoint vCameraPos);

		virtual float *GetDataPointer(const string &strDataType);

		virtual void UpdateBackgroundColor();

		virtual void Initialize();
		virtual void Update();
		virtual void Close();

		virtual void OnGetFocus();
		virtual void OnLoseFocus();
};

	}// end Visualization
}// end VortexAnimatSim

