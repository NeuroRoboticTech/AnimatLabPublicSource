#include "StdAfx.h"

#include "OsgMovableItem.h"
//#include "OsgBody.h"
//#include "OsgJoint.h"
#include "OsgDragger.h"
#include "OsgTrackballDragger.h"
#include "OsgStructure.h"
#include "OsgUserData.h"
#include "OsgSimulator.h"

namespace OsgAnimatSim
{
	namespace Visualization
	{


osg::Geometry* createCircleGeometry(float radius, unsigned int numSegments)
{
    const float angleDelta = 2.0f*osg::PI/(float)numSegments;
    const float r = radius;
    float angle = 0.0f;
    osg::Vec3Array* vertexArray = new osg::Vec3Array(numSegments);
    osg::Vec3Array* normalArray = new osg::Vec3Array(numSegments);
    for(unsigned int i = 0; i < numSegments; ++i,angle+=angleDelta)
    {
        float c = cosf(angle);
        float s = sinf(angle);
        (*vertexArray)[i].set(c*r,s*r,0.0f);
        (*normalArray)[i].set(c,s,0.0f);
    }
    osg::Geometry* geometry = new osg::Geometry();
    geometry->setVertexArray(vertexArray);
    geometry->setNormalArray(normalArray);
    geometry->setNormalBinding(osg::Geometry::BIND_PER_VERTEX);
    geometry->addPrimitiveSet(new osg::DrawArrays(osg::PrimitiveSet::LINE_LOOP,0,vertexArray->size()));
    return geometry;
}

OsgTrackballDragger::OsgTrackballDragger(BOOL bAllowRotateX, BOOL bAllowRotateY, BOOL bAllowRotateZ)
{
	if(bAllowRotateX)
	{
		_xDragger = new osgManipulator::RotateCylinderDragger();
		addChild(_xDragger.get());
		addDragger(_xDragger.get());
	}

	if(bAllowRotateY)
	{
		_yDragger = new osgManipulator::RotateCylinderDragger();
		addChild(_yDragger.get());
		addDragger(_yDragger.get());
	}

	if(bAllowRotateZ)
	{
		_zDragger = new osgManipulator::RotateCylinderDragger();
		addChild(_zDragger.get());
		addDragger(_zDragger.get());
	}

    setParentDragger(getParentDragger());
}

OsgTrackballDragger::~OsgTrackballDragger(void)
{
}

void OsgTrackballDragger::setupDefaultGeometry()
{
	//If there are no valid draggers then skip creating the geometry.
	if(!_xDragger.valid() && !_xDragger.valid() && !_xDragger.valid())
		return;

    osg::Geode* geode = new osg::Geode;
    {
        osg::TessellationHints* hints = new osg::TessellationHints;
        hints->setCreateTop(false);
        hints->setCreateBottom(false);
        hints->setCreateBackFace(false);

        osg::Cylinder* cylinder = new osg::Cylinder;
        cylinder->setHeight(0.15f);
        osg::ShapeDrawable* cylinderDrawable = new osg::ShapeDrawable(cylinder,hints);
        geode->addDrawable(cylinderDrawable);
		osgManipulator::setDrawableToAlwaysCull(*cylinderDrawable);
        geode->addDrawable(createCircleGeometry(1.0f, 100));
    }

    // Draw in line mode.
    {
        osg::PolygonMode* polymode = new osg::PolygonMode;
        polymode->setMode(osg::PolygonMode::FRONT_AND_BACK,osg::PolygonMode::LINE);
        geode->getOrCreateStateSet()->setAttributeAndModes(polymode,osg::StateAttribute::OVERRIDE|osg::StateAttribute::ON);
        geode->getOrCreateStateSet()->setMode(GL_NORMALIZE, osg::StateAttribute::ON);
        geode->getOrCreateStateSet()->setAttributeAndModes(new osg::LineWidth(5.0f),osg::StateAttribute::ON);
    }

    // Add line to all the individual 1D draggers.
    _xDragger->addChild(geode);
    _yDragger->addChild(geode);
    _zDragger->addChild(geode);


    // Rotate X-axis dragger appropriately.
    {
        osg::Quat rotation; rotation.makeRotate(osg::Vec3(0.0f, 0.0f, 1.0f), osg::Vec3(1.0f, 0.0f, 0.0f));
        _xDragger->setMatrix(osg::Matrix(rotation));
    }

    // Rotate Y-axis dragger appropriately.
    {
        osg::Quat rotation; rotation.makeRotate(osg::Vec3(0.0f, 0.0f, 1.0f), osg::Vec3(0.0f, 1.0f, 0.0f));
        _yDragger->setMatrix(osg::Matrix(rotation));
    }

    // Send different colors for each dragger.
    _xDragger->setColor(osg::Vec4(1.0f,0.0f,0.0f,1.0f));
    _yDragger->setColor(osg::Vec4(0.0f,1.0f,0.0f,1.0f));
    _zDragger->setColor(osg::Vec4(0.0f,0.0f,1.0f,1.0f));

    //// Add invisible sphere for pick the spherical dragger.
    //{
    //    osg::Drawable* sphereDrawable = new osg::ShapeDrawable(new osg::Sphere());
    //    osgManipulator::setDrawableToAlwaysCull(*sphereDrawable);
    //    osg::Geode* sphereGeode = new osg::Geode;
    //    sphereGeode->addDrawable(sphereDrawable);

    //    _xyzDragger->addChild(sphereGeode);
    //}
}


	}// end Visualization
}// end OsgAnimatSim