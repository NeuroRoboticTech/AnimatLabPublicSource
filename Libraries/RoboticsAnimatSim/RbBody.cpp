#include "StdAfx.h"
#include <stdarg.h>
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbRigidBody.h"
#include "RbJoint.h"
#include "RbStructure.h"

namespace RoboticsAnimatSim
{
	namespace Environment
	{
/**
\brief	Default constructor.

\author	dcofer
\date	5/2/2011
**/
RbBody::RbBody()
{
}

/**
\brief	Destructor.

\author	dcofer
\date	5/2/2011
**/
RbBody::~RbBody()
{
}

void RbBody::SetThisPointers()
{
	RbMovableItem::SetThisPointers();

	m_lpThisBP = dynamic_cast<BodyPart *>(this);
	if(!m_lpThisBP)
		THROW_TEXT_ERROR(Rb_Err_lThisPointerNotDefined, Rb_Err_strThisPointerNotDefined, "m_lpThisBP, " + m_lpThisAB->Name());
}

	}			// Environment
//}				//RbAnimatSim

}