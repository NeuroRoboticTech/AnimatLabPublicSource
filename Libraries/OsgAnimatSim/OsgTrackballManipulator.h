#pragma once

/* -*-c++-*- OpenSceneGraph - Copyright (C) 1998-2006 Robert Osfield 
 *
 * This library is open source and may be redistributed and/or modified under  
 * the terms of the OpenSceneGraph Public License (OSGPL) version 0.0 or 
 * (at your option) any later version.  The full license is in LICENSE file
 * included with this distribution, and on the openscenegraph.org website.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the 
 * OpenSceneGraph Public License for more details.
*/

#include <osgGA/MatrixManipulator>
#include <osg/Quat>

namespace OsgAnimatSim
{
	namespace Visualization
	{

class ANIMAT_OSG_PORT OsgTrackballManipulator : public osgGA::MatrixManipulator
{
    public:
        OsgTrackballManipulator();

        virtual const char* className() const { return "Trackball"; }

        /** set the position of the matrix manipulator using a 4x4 Matrix.*/
        virtual void setByMatrix(const osg::Matrixd& matrix);

        /** set the position of the matrix manipulator using a 4x4 Matrix.*/
        virtual void setByInverseMatrix(const osg::Matrixd& matrix) { setByMatrix(osg::Matrixd::inverse(matrix)); }

        /** get the position of the manipulator as 4x4 Matrix.*/
        virtual osg::Matrixd getMatrix() const;

        /** get the position of the manipulator as a inverse matrix of the manipulator, typically used as a model view matrix.*/
        virtual osg::Matrixd getInverseMatrix() const;

        /** Get the FusionDistanceMode. Used by SceneView for setting up stereo convergence.*/
        virtual osgUtil::SceneView::FusionDistanceMode getFusionDistanceMode() const { return osgUtil::SceneView::USE_FUSION_DISTANCE_VALUE; }

        /** Get the FusionDistanceValue. Used by SceneView for setting up stereo convergence.*/
        virtual float getFusionDistanceValue() const { return _distance; }

        /** Attach a node to the manipulator. 
            Automatically detaches previously attached node.
            setNode(NULL) detaches previously nodes.
            Is ignored by manipulators which do not require a reference model.*/
        virtual void setNode(osg::Node*);

        /** Return node if attached.*/
        virtual const osg::Node* getNode() const;

        /** Return node if attached.*/
        virtual osg::Node* getNode();

        /** Move the camera to the default position. 
            May be ignored by manipulators if home functionality is not appropriate.*/
        virtual void home(const GUIEventAdapter& ea,GUIActionAdapter& us);
        virtual void home(double);
        
        /** Start/restart the manipulator.*/
        virtual void init(const GUIEventAdapter& ea,GUIActionAdapter& us);

        /** handle events, return true if handled, false otherwise.*/
        virtual bool handle(const GUIEventAdapter& ea,GUIActionAdapter& us);

        /** Get the keyboard and mouse usage of this manipulator.*/
        virtual void getUsage(osg::ApplicationUsage& usage) const;


        /** set the minimum distance (as ratio) the eye point can be zoomed in towards the
            center before the center is pushed forward.*/        
        void setMinimumZoomScale(double minimumZoomScale) { _minimumZoomScale=minimumZoomScale; }

        /** get the minimum distance (as ratio) the eye point can be zoomed in */
        double getMinimumZoomScale() const { return _minimumZoomScale; }

        /** Set the center of the trackball. */
        void setCenter(const osg::Vec3d& center) { _center = center; }

        /** Get the center of the trackball. */
        const osg::Vec3d& getCenter() const { return _center; }

        /** Set the rotation of the trackball. */
        void setRotation(const osg::Quat& rotation) { _rotation = rotation; }

        /** Get the rotation of the trackball. */
        const osg::Quat& getRotation() const { return _rotation; }

        /** Set the distance of the trackball. */
        void setDistance(double distance) { _distance = distance; }

        /** Get the distance of the trackball. */
        double getDistance() const { return _distance; }

        /** Set the size of the trackball. */
        void setTrackballSize(float size);

        /** Get the size of the trackball. */
        float getTrackballSize() const { return _trackballSize; }
    protected:

        virtual ~OsgTrackballManipulator();

        /** Reset the internal GUIEvent stack.*/
        void flushMouseEventStack();
        /** Add the current mouse GUIEvent to internal stack.*/
        void addMouseEvent(const GUIEventAdapter& ea);

        void computePosition(const osg::Vec3& eye,const osg::Vec3& lv,const osg::Vec3& up);

        /** For the give mouse movement calculate the movement of the camera.
            Return true is camera has moved and a redraw is required.*/
        bool calcMovement();
        
        void trackball(osg::Vec3& axis,float& angle, float p1x, float p1y, float p2x, float p2y);
        float tb_project_to_sphere(float r, float x, float y);


        /** Check the speed at which the mouse is moving.
            If speed is below a threshold then return false, otherwise return true.*/
        bool isMouseMoving();

        // Internal event stack comprising last two mouse events.
        osg::ref_ptr<const GUIEventAdapter> _ga_t1;
        osg::ref_ptr<const GUIEventAdapter> _ga_t0;

        osg::ref_ptr<osg::Node>       _node;

        double _modelScale;
        double _minimumZoomScale;

        bool _thrown;
        
        osg::Vec3d   _center;
        osg::Quat    _rotation;
        double       _distance;
        float        _trackballSize;

};



	}			// Visualization
}
