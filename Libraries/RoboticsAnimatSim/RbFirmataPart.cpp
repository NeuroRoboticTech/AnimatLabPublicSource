// RbFirmataAnalogInput.cpp: implementation of the RbFirmataAnalogInput class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbMotorizedJoint.h"
#include "RbHingeLimit.h"
#include "RbHinge.h"
#include "RbRigidBody.h"
#include "RbStructure.h"
#include "RbFirmataPart.h"
#include "RbFirmataController.h"

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{
			namespace Firmata
			{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbFirmataPart::RbFirmataPart() 
{
	m_lpFirmata = NULL;
}

RbFirmataPart::~RbFirmataPart()
{
	m_lpFirmata = NULL;
}

void RbFirmataPart::ParentIOControl(RobotIOControl *lpParent)
{
	RobotPartInterface::ParentIOControl(lpParent);

	m_lpFirmata = dynamic_cast<RbFirmataController *>(lpParent);
	if(!m_lpFirmata)
		THROW_ERROR(Rb_Err_lUnableToObtainFirmataPointer, Rb_Err_strUnableToObtainFirmataPointer);
}

void RbFirmataPart::SetFirmata(RbFirmataController *lpFirmata) {m_lpFirmata = lpFirmata;}

RbFirmataController *RbFirmataPart::GetFirmata() {return m_lpFirmata;}

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

