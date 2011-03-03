// VsAttachment.cpp: implementation of the VsAttachment class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsAttachment.h"
#include "VsSimulator.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsAttachment::VsAttachment()
{
	m_lpThis = this;
	m_lpThisBody = this;
	m_lpPhysicsBody = this;
}

VsAttachment::~VsAttachment()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VsAttachment\r\n", "", -1, FALSE, TRUE);}
}

void VsAttachment::CreateParts()
{
	osg::ref_ptr<osg::ShapeDrawable> osgDrawable = new osg::ShapeDrawable(new osg::Sphere(osg::Vec3(0,0,0), m_fltRadius));
	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(osgDrawable.get());
	m_osgNode = osgGroup;
	m_vxGeometry = NULL;  //No physics part.

	VsRigidBody::CreateBody();
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim

