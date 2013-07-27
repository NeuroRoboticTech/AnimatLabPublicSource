#include "StdAfx.h"
#include <stdarg.h>
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgJoint.h"
#include "VsOsgUserData.h"
#include "VsOsgUserDataVisitor.h"

#include "OsgMouseSpring.h"
#include "OsgLight.h"
#include "OsgCameraManipulator.h"
#include "OsgDragger.h"

namespace VortexAnimatSim
{
	namespace Environment
	{
/**
\brief	Default constructor.

\author	dcofer
\date	5/2/2011
**/
OsgBody::OsgBody()
{
	m_eControlType = VxEntity::kControlDynamic; //Default to dynamic control of the rigid body.
}

/**
\brief	Destructor.

\author	dcofer
\date	5/2/2011
**/
OsgBody::~OsgBody()
{
}

void OsgBody::SetThisPointers()
{
	VsMovableItem::SetThisPointers();

	m_lpThisBP = dynamic_cast<BodyPart *>(this);
	if(!m_lpThisBP)
		THROW_TEXT_ERROR(Vs_Err_lThisPointerNotDefined, Vs_Err_strThisPointerNotDefined, "m_lpThisBP, " + m_lpThisAB->Name());
}

	}			// Environment
//}				//VortexAnimatSim

}