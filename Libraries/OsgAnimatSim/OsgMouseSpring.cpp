#include "StdAfx.h"
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgRigidBody.h"
#include "OsgJoint.h"
#include "OsgOrganism.h"
#include "OsgStructure.h"
#include "OsgUserData.h"
#include "OsgUserDataVisitor.h"

#include "OsgLight.h"
#include "OsgCameraManipulator.h"
#include "OsgDragger.h"
#include "OsgMouseSpring.h"
#include "OsgSimulator.h"

namespace OsgAnimatSim
{
	namespace Visualization
	{

OsgMouseSpring::OsgMouseSpring(void)
{
	m_gdeLine = NULL;
	m_aryLines = NULL;
	m_v3Start.set(0,0,0);
	m_v3End.set(1,0,0);
    m_lpRB = NULL;
	m_osgRB = NULL;
}

OsgMouseSpring::~OsgMouseSpring(void)
{
}

void OsgMouseSpring::Visible(bool bVal)
{
	if(bVal)
		m_gdeLine->setNodeMask(0x1);
	else
		m_gdeLine->setNodeMask(0x0);
}

void OsgMouseSpring::Initialize()
{
	//create a line geometry
	m_linesGeom = new osg::Geometry();	

	m_aryLines = new osg::Vec3Array();
	
	m_aryLines->push_back(m_v3Start);
	m_aryLines->push_back(m_v3End);

	//set the vertex array
	m_linesGeom->setVertexArray(m_aryLines.get());	

	//add a primativeset to the geometry
	m_linesGeom->addPrimitiveSet(new osg::DrawArrays(osg::PrimitiveSet::LINE_STRIP,0,m_aryLines->size()));

	 // set the color
    osg::Vec4Array* colors = new osg::Vec4Array;
    colors->push_back(osg::Vec4(1.0f,1.0f,1.0f,1.0f));
	colors->push_back(osg::Vec4(1.0f, 1.0f, 1.0f, 1.0f));
    m_linesGeom->setColorArray(colors);
	m_linesGeom->setColorBinding(osg::Geometry::BIND_PER_VERTEX);

	//m_linesGeom display list so that the geometry updates each frame
	m_linesGeom->setDataVariance(osg::Object::DYNAMIC);
	m_linesGeom->setUseDisplayList(false);

	m_gdeLine = new osg::Geode();
	m_gdeLine->addDrawable(m_linesGeom.get());

	osg::StateSet *ss = new osg::StateSet();
	ss->setMode( GL_LIGHTING, osg::StateAttribute::PROTECTED | osg::StateAttribute::OFF );

    m_lineWidth = new osg::LineWidth();
    m_lineWidth->setWidth(1.0f);
    ss->setAttributeAndModes(m_lineWidth.get(), osg::StateAttribute::ON);
    ss->setMode(GL_LIGHTING, osg::StateAttribute::OFF);
    m_gdeLine->setStateSet(ss);

	//Initially not visible.
	Visible(false);
}

void OsgMouseSpring::SetStart(osg::Vec3 v3Start)
{
	m_v3Start = v3Start;
	Update();
}

void OsgMouseSpring::SetEnd(osg::Vec3 v3End)
{
	m_v3End = v3End;
	Update();
}

void OsgMouseSpring::Update()
{
	(*m_aryLines)[0].set(m_v3Start);
	(*m_aryLines)[1].set(m_v3End);

	m_linesGeom->dirtyBound();
}

	}// end Visualization
}// end OsgAnimatSim
