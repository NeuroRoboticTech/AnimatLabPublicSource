#include "StdAfx.h"

#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsStructure.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Visualization
	{

VsOsgUserData::VsOsgUserData(VsMovableItem *lpItem)
{
	m_lpItem = lpItem;
}


VsOsgUserData::~VsOsgUserData(void)
{
	m_lpItem = NULL;
}

	}// end Visualization
}// end VortexAnimatSim