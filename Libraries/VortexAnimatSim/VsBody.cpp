#include "StdAfx.h"
#include <stdarg.h>
#include "VsMovableItem.h"
#include "VsMovableItem.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsOrganism.h"
#include "VsStructure.h"
#include "VsClassFactory.h"
#include "VsSimulator.h"
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"

//#include "VsSimulationRecorder.h"
#include "VsMouseSpring.h"
#include "VsLight.h"
#include "VsCameraManipulator.h"
#include "VsDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{
/**
\brief	Default constructor.

\author	dcofer
\date	5/2/2011
**/
VsBody::VsBody()
{
	m_eControlType = VxEntity::kControlDynamic; //Default to dynamic control of the rigid body.
}

/**
\brief	Destructor.

\author	dcofer
\date	5/2/2011
**/
VsBody::~VsBody()
{
}

void VsBody::SetThisPointers()
{
	VsMovableItem::SetThisPointers();

	m_lpThisBP = dynamic_cast<BodyPart *>(this);
	if(!m_lpThisBP)
		THROW_TEXT_ERROR(Vs_Err_lThisPointerNotDefined, Vs_Err_strThisPointerNotDefined, "m_lpThisBP, " + m_lpThisAB->Name());
}

	}			// Environment
//}				//VortexAnimatSim

}