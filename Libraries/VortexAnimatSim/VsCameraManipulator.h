#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class VsCameraManipulator : public osgGA::TrackballManipulator
		{
			private:
				bool m_bShiftDown;
				bool m_bControlDown;	
				bool m_bInDrag;

				float m_fltPrevX;
				float m_fltPrevY;

				Simulator *m_lpSim;

				osg::Vec3 m_vPickPoint;
				osg::Vec3 m_vPickNormal;
				MovableItem *m_lpPicked;
				osg::Vec3 m_vSelectedVertex;

				osg::Quat m_quatRotation;
				osg::Vec3 m_v3Eye;

				osg::ref_ptr<osgViewer::Viewer> m_osgViewer;
				osg::ref_ptr<osg::Viewport> m_osgViewport;

				virtual const osg::Camera *GetCamera(float x, float y, float &local_x, float &local_y);
				virtual const osg::Camera *GetCamera(float x, float y);

				virtual osg::Vec3 FindSelectedVertex(osgUtil::LineSegmentIntersector::Intersections::iterator &hitr);

			protected:
				bool CanDoMouseSpring();
				bool DoMouseSpring(const GUIEventAdapter& ea, float x, float y);
				void DoPan(const GUIEventAdapter& ea, float x, float y);
				void DoZoom(const GUIEventAdapter& ea, float x, float y);
				void DoRotate(const GUIEventAdapter& ea, float x, float y);

				osg::Vec3 ConvertMouseTo3D(const GUIEventAdapter& ea, int x, int y, osg::Vec3 vGrabPos);

				void pick(const osgGA::GUIEventAdapter& ea, GUIActionAdapter& aa);
				float tb_project_to_sphere(float r, float x, float y);

			public:
				VsCameraManipulator(Simulator *lpSim, osgViewer::Viewer *osgViewer, osg::Viewport *osgViewport = NULL);
				~VsCameraManipulator(void);

				  /** Handle events, return true if handled, false otherwise. */
				virtual bool handle(const GUIEventAdapter& ea,GUIActionAdapter& us);

		};

	}// end Visualization
}// end VortexAnimatSim

