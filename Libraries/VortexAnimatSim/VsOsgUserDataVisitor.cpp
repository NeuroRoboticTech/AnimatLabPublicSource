#include "StdAfx.h"

#include "VsMouseSpring.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsSimulator.h"
#include "VsOsgUserDataVisitor.h"
#include "VsOsgUserData.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Visualization
	{

VsOsgUserDataVisitor::VsOsgUserDataVisitor(VsRigidBody *lpBody) : osg::NodeVisitor(osg::NodeVisitor::TRAVERSE_ALL_CHILDREN )
{
	m_lpVsBody = lpBody;
	m_lpBody = dynamic_cast<RigidBody *>(lpBody);
	m_lpVsJoint = NULL;
	m_lpJoint = NULL;
}

VsOsgUserDataVisitor::VsOsgUserDataVisitor(VsJoint *lpJoint) : osg::NodeVisitor(osg::NodeVisitor::TRAVERSE_ALL_CHILDREN )
{
	m_lpVsBody = NULL;
	m_lpBody = NULL;
	m_lpVsJoint = lpJoint;
	m_lpJoint = dynamic_cast<Joint *>(lpJoint);
}


VsOsgUserDataVisitor::~VsOsgUserDataVisitor(void)
{
	m_lpVsBody = NULL;
	m_lpBody = NULL;
	m_lpVsJoint = NULL;
	m_lpJoint = NULL;
}

void VsOsgUserDataVisitor::apply(osg::Geode &osgGeode)
{
	int iCount = osgGeode.getNumDrawables();

	for(int iIdx=0; iIdx<iCount; iIdx++)
	{
		osg::Drawable *lpDraw = osgGeode.getDrawable(iIdx);
		if(lpDraw)
		{
			if(m_lpBody)
			{
				lpDraw->setName(m_lpBody->ID().c_str());
				lpDraw->setUserData(new VsOsgUserData(m_lpVsBody));
			}
			else if(m_lpJoint)
			{
				lpDraw->setName(m_lpJoint->ID().c_str());
				lpDraw->setUserData(new VsOsgUserData(m_lpVsJoint));
			}

		}
	}
}


	}// end Visualization
}// end VortexAnimatSim