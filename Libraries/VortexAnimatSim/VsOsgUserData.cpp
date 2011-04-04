#include "StdAfx.h"

#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Visualization
	{

VsOsgUserData::VsOsgUserData(VsBody *lpBody)
{
	m_lpVsBodyPart = lpBody;
	m_lpVsBody = dynamic_cast<VsRigidBody *>(lpBody);
	m_lpBody = dynamic_cast<RigidBody *>(lpBody);
	m_lpVsJoint = dynamic_cast<VsJoint *>(lpBody);
	m_lpJoint = dynamic_cast<Joint *>(lpBody);
}

VsOsgUserData::VsOsgUserData(VsRigidBody *lpBody)
{
	m_lpVsBodyPart = dynamic_cast<VsBody *>(lpBody);
	m_lpVsBody = lpBody;
	m_lpBody = dynamic_cast<RigidBody *>(lpBody);
	m_lpVsJoint = NULL;
	m_lpJoint = NULL;
}

VsOsgUserData::VsOsgUserData(VsJoint *lpJoint)
{
	m_lpVsBodyPart = dynamic_cast<VsBody *>(lpJoint);
	m_lpVsBody = NULL;
	m_lpBody = NULL;
	m_lpVsJoint = lpJoint;
	m_lpJoint = dynamic_cast<Joint *>(lpJoint);
}


VsOsgUserData::~VsOsgUserData(void)
{
	m_lpVsBodyPart = NULL;
	m_lpVsBody = NULL;
	m_lpBody = NULL;
	m_lpVsJoint = NULL;
	m_lpJoint = NULL;
}

	}// end Visualization
}// end VortexAnimatSim