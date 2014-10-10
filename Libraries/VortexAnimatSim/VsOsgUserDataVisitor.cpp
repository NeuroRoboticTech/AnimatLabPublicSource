#include "StdAfx.h"

#include "VsMouseSpring.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsStructure.h"
#include "VsSimulator.h"
#include "VsOsgUserDataVisitor.h"
#include "VsOsgUserData.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Visualization
	{

VsOsgUserDataVisitor::VsOsgUserDataVisitor(VsMovableItem *lpItem) : osg::NodeVisitor(osg::NodeVisitor::TRAVERSE_ALL_CHILDREN )
{
	m_lpItem = lpItem;
	//Force the osg user data visitor to visit all the nodes even if the node itself is set to not visit.
	//Fixes a problem where frozen nodes were set as static in osg, so they were not getting their user data set.
	setNodeMaskOverride(0xFFFFFFFF);
}

VsOsgUserDataVisitor::~VsOsgUserDataVisitor(void)
{
	m_lpItem = NULL;
}

void VsOsgUserDataVisitor::apply(osg::Geode &osgGeode)
{
	int iCount = osgGeode.getNumDrawables();

	for(int iIdx=0; iIdx<iCount; iIdx++)
	{
		osg::Drawable *lpDraw = osgGeode.getDrawable(iIdx);
		if(lpDraw)
		{
			AnimatBase *lpAB = dynamic_cast<AnimatBase *>(m_lpItem);
			if(lpAB)
				lpDraw->setName(lpAB->Name() + "_Drawable");
			lpDraw->setUserData(new VsOsgUserData(m_lpItem));
		}
	}
}


	}// end Visualization
}// end VortexAnimatSim