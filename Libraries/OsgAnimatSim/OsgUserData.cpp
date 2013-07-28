#include "StdAfx.h"

#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgRigidBody.h"
#include "OsgJoint.h"
#include "OsgStructure.h"
#include "OsgUserData.h"
#include "OsgDragger.h"
#include "OsgSimulator.h"

namespace OsgAnimatSim
{
	namespace Visualization
	{

OsgUserData::OsgUserData(OsgMovableItem *lpItem)
{
	m_lpItem = lpItem;
}


OsgUserData::~OsgUserData(void)
{
	m_lpItem = NULL;
}

	}// end Visualization
}// end OsgAnimatSim