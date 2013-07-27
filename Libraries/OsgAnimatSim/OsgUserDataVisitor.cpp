#include "StdAfx.h"

#include "OsgMovableItem.h"
//#include "OsgBody.h"
//#include "OsgJoint.h"
#include "OsgStructure.h"
#include "OsgUserDataVisitor.h"
#include "OsgUserData.h"
#include "OsgDragger.h"
#include "OsgMouseSpring.h"
#include "OsgSimulator.h"

namespace OsgAnimatSim
{
	namespace Visualization
	{

OsgUserDataVisitor::OsgUserDataVisitor(OsgMovableItem *lpItem) : osg::NodeVisitor(osg::NodeVisitor::TRAVERSE_ALL_CHILDREN )
{
	m_lpItem = lpItem;
	//Force the osg user data visitor to visit all the nodes even if the node itself is set to not visit.
	//Fixes a problem where frozen nodes were set as static in osg, so they were not getting their user data set.
	setNodeMaskOverride(0xFFFFFFFF);
}

OsgUserDataVisitor::~OsgUserDataVisitor(void)
{
	m_lpItem = NULL;
}

void OsgUserDataVisitor::apply(osg::Geode &osgGeode)
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
			lpDraw->setUserData(new OsgUserData(m_lpItem));
		}
	}
}


	}// end Visualization
}// end OsgAnimatSim