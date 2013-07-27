#include "StdAfx.h"

#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgJoint.h"
#include "OsgDragger.h"
#include "OsgTranslateAxisDragger.h"
#include "OsgStructure.h"
#include "OsgUserData.h"

namespace OsgAnimatSim
{
	namespace Visualization
	{

OsgTranslateAxisDragger::OsgTranslateAxisDragger(BOOL bAllowTranslateX, BOOL bAllowTranslateY, BOOL bAllowTranslateZ)
{
	if(bAllowTranslateX)
	{
		_xDragger = new osgManipulator::Translate1DDragger(osg::Vec3(0.0,0.0,0.0), osg::Vec3(0.0,0.0,1.0));
		addChild(_xDragger.get());
		addDragger(_xDragger.get());
	}

	if(bAllowTranslateY)
	{
		_yDragger = new osgManipulator::Translate1DDragger(osg::Vec3(0.0,0.0,0.0), osg::Vec3(0.0,0.0,1.0));
		addChild(_yDragger.get());
		addDragger(_yDragger.get());
	}

	if(bAllowTranslateZ)
	{
		_zDragger = new osgManipulator::Translate1DDragger(osg::Vec3(0.0,0.0,0.0), osg::Vec3(0.0,0.0,1.0));
		addChild(_zDragger.get());
		addDragger(_zDragger.get());
	}

    setParentDragger(getParentDragger());
}

OsgTranslateAxisDragger::~OsgTranslateAxisDragger(void)
{
}

void OsgTranslateAxisDragger::setupDefaultGeometry()
{
	//If there are no valid draggers then skip creating the geometry.
	if(!_xDragger.valid() && !_xDragger.valid() && !_xDragger.valid())
		return;

	float fltLength = 1.0f;

    // Create a line.
    osg::Geode* lineGeode = new osg::Geode;
    {
        osg::Geometry* geometry = new osg::Geometry();
        
        osg::Vec3Array* vertices = new osg::Vec3Array(2);
        (*vertices)[0] = osg::Vec3(0.0f,0.0f,0.0f);
        (*vertices)[1] = osg::Vec3(0.0f,0.0f,fltLength);

        geometry->setVertexArray(vertices);
        geometry->addPrimitiveSet(new osg::DrawArrays(osg::PrimitiveSet::LINES,0,2));

        lineGeode->addDrawable(geometry);
    }

    // Turn of lighting for line and set line width.
    {
        osg::LineWidth* linewidth = new osg::LineWidth();
        linewidth->setWidth(7.0f);
        lineGeode->getOrCreateStateSet()->setAttributeAndModes(linewidth, osg::StateAttribute::ON);
        lineGeode->getOrCreateStateSet()->setMode(GL_LIGHTING,osg::StateAttribute::OFF);
    }

    // Add line to all the individual 1D draggers.
    osg::Geode* geode = new osg::Geode;
    
    // Create a cone.
    {
        osg::Cone* cone = new osg::Cone (osg::Vec3(0.0f, 0.0f, 1.0f), 0.06f, 0.30f);
        geode->addDrawable(new osg::ShapeDrawable(cone));
    }

    // Create an invisible cylinder for picking the line.
    {
        osg::Cylinder* cylinder = new osg::Cylinder (osg::Vec3(0.0f,0.0f,0.5f), 0.03f, fltLength);
        osg::Drawable* geometry = new osg::ShapeDrawable(cylinder);
        osgManipulator::setDrawableToAlwaysCull(*geometry);
        geode->addDrawable(geometry);
    }

    // Add geode to all 1D draggers.
	if(_xDragger.valid())
	{
	    _xDragger->addChild(lineGeode);
	    _xDragger->addChild(geode);

	    // Rotate X-axis dragger appropriately.
        osg::Quat rotation; rotation.makeRotate(osg::Vec3(0.0f, 0.0f, 1.0f), osg::Vec3(1.0f, 0.0f, 0.0f));
        _xDragger->setMatrix(osg::Matrix(rotation));

		// Send different colors for each dragger.
		_xDragger->setColor(osg::Vec4(1.0f,0.0f,0.0f,1.0f));
	}

	if(_yDragger.valid())
	{
		_yDragger->addChild(lineGeode);
		_yDragger->addChild(geode);

	    // Rotate Y-axis dragger appropriately.
        osg::Quat rotation; rotation.makeRotate(osg::Vec3(0.0f, 0.0f, 1.0f), osg::Vec3(0.0f, 1.0f, 0.0f));
        _yDragger->setMatrix(osg::Matrix(rotation));

		// Send different colors for each dragger.
	    _yDragger->setColor(osg::Vec4(0.0f,1.0f,0.0f,1.0f));
	}

	if(_zDragger.valid())
	{
		_zDragger->addChild(lineGeode);
		_zDragger->addChild(geode);

		// Send different colors for each dragger.
	    _zDragger->setColor(osg::Vec4(0.0f,0.0f,1.0f,1.0f));
	}

}

	}// end Visualization
}// end OsgAnimatSim