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

//#ifndef OSG_AUTOTRANSFORM
//#define OSG_AUTOTRANSFORM 1

#include "StdAfx.h"
//#include <osg/Group>
//#include <osg/Transform>
//#include <osg/Quat>
//#include <osg/Viewport>

namespace VortexAnimatSim
{
	namespace Visualization
	{

/** AutoTransform is a derived form of Transform that automatically
  * scales or rotates to keep its children aligned with screen coordinates.
*/
class VsAutoTransform : public osg::Transform
{
    public :
        VsAutoTransform();

        VsAutoTransform(const VsAutoTransform& pat,const osg::CopyOp& copyop= osg::CopyOp::SHALLOW_COPY);

        virtual osg::Object* cloneType() const { return new VsAutoTransform (); }
        virtual osg::Object* clone(const osg::CopyOp& copyop) const { return new VsAutoTransform (*this,copyop); }
        virtual bool isSameKindAs(const osg::Object* obj) const { return dynamic_cast<const VsAutoTransform *>(obj)!=NULL; }
        virtual const char* className() const { return "VsAutoTransform"; }
        virtual const char* libraryName() const { return "osg"; }

        virtual void accept(osg::NodeVisitor& nv);

        virtual VsAutoTransform* asAutoTransform() { return this; }
        virtual const VsAutoTransform* asAutoTransform() const { return this; }

        inline void setPosition(const osg::Vec3d& pos) { _position = pos; _matrixDirty=true; dirtyBound(); }
        inline const osg::Vec3d& getPosition() const { return _position; }


        inline void setRotation(const osg::Quat& quat) { _rotation = quat; _matrixDirty=true; dirtyBound(); }
        inline const osg::Quat& getRotation() const { return _rotation; }

        inline void setScale(double scale) { setScale(osg::Vec3(scale,scale,scale)); }

        void setScale(const osg::Vec3d& scale);
        inline const osg::Vec3d& getScale() { return _scale; }

        void setMinimumScale(double minimumScale) { _minimumScale = minimumScale; }
        double getMinimumScale() { return _minimumScale; }

        void setMaximumScale(double maximumScale) { _maximumScale = maximumScale; }
        double getMaximumScale() { return _maximumScale; }

        inline void setPivotPoint(const osg::Vec3d& pivot) { _pivotPoint = pivot; _matrixDirty=true; dirtyBound(); }
        inline const osg::Vec3d& getPivotPoint() { return _pivotPoint; }


        void setAutoUpdateEyeMovementTolerance(float tolerance) { _autoUpdateEyeMovementTolerance = tolerance; }
        float getAutoUpdateEyeMovementTolerance() { return _autoUpdateEyeMovementTolerance; }


        enum AutoRotateMode
        {
            NO_ROTATION,
            ROTATE_TO_SCREEN,
            ROTATE_TO_CAMERA
        };

        void setAutoRotateMode(AutoRotateMode mode) { _autoRotateMode = mode; _firstTimeToInitEyePoint = true; }

        AutoRotateMode getAutoRotateMode() const { return _autoRotateMode; }

        void setAutoScaleToScreen(bool autoScaleToScreen) { _autoScaleToScreen = autoScaleToScreen; _matrixDirty=true; }

        bool getAutoScaleToScreen() const { return _autoScaleToScreen; }

        void setAutoScaleTransitionWidthRatio(float ratio) { _autoScaleTransitionWidthRatio = ratio; }
        float getAutoScaleTransitionWidthRatio() const { return _autoScaleTransitionWidthRatio; }


        virtual bool computeLocalToWorldMatrix(osg::Matrix& matrix,osg::NodeVisitor* nv) const;

        virtual bool computeWorldToLocalMatrix(osg::Matrix& matrix,osg::NodeVisitor* nv) const;

        virtual osg::BoundingSphere computeBound() const;


    protected :

        virtual ~VsAutoTransform() {}

        osg::Vec3d                           _position;
        osg::Vec3d                           _pivotPoint;
        double                          _autoUpdateEyeMovementTolerance;

        AutoRotateMode                  _autoRotateMode;

        bool                            _autoScaleToScreen;

        mutable osg::Quat                    _rotation;
        mutable osg::Vec3d                   _scale;
        mutable bool                    _firstTimeToInitEyePoint;
        mutable osg::Vec3               _previousEyePoint;
        mutable osg::Vec3               _previousLocalUp;
        mutable osg::Viewport::value_type    _previousWidth;
        mutable osg::Viewport::value_type    _previousHeight;
        mutable osg::Matrixd            _previousProjection;
        mutable osg::Vec3d              _previousPosition;

        double                          _minimumScale;
        double                          _maximumScale;
        double                          _autoScaleTransitionWidthRatio;

        void computeMatrix() const;

        mutable bool                    _matrixDirty;
        mutable osg::Matrixd            _cachedMatrix;
};

	}// end Visualization
}// end VortexAnimatSim

//#endif
