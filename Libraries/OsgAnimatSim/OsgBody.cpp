#include "StdAfx.h"
#include <stdarg.h>
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgRigidBody.h"
#include "OsgJoint.h"
#include "OsgStructure.h"
#include "OsgUserData.h"
#include "OsgUserDataVisitor.h"

#include "OsgMouseSpring.h"
#include "OsgLight.h"
#include "OsgCameraManipulator.h"
#include "OsgDragger.h"

namespace OsgAnimatSim
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
	m_eControlType = DynamicsControlType::ControlDynamic; //Default to dynamic control of the rigid body.
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
	OsgMovableItem::SetThisPointers();

	m_lpThisBP = dynamic_cast<BodyPart *>(this);
	if(!m_lpThisBP)
		THROW_TEXT_ERROR(Osg_Err_lThisPointerNotDefined, Osg_Err_strThisPointerNotDefined, "m_lpThisBP, " + m_lpThisAB->Name());
}

	}			// Environment
//}				//OsgAnimatSim

}