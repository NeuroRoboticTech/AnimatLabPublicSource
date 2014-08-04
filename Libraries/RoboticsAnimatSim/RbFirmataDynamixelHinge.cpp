// RbFirmataDynamixelHinge.cpp: implementation of the RbFirmataDynamixelHinge class.
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
#include "RbFirmataDynamixelServo.h"
#include "RbFirmataDynamixelHinge.h"
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

RbFirmataDynamixelHinge::RbFirmataDynamixelHinge() 
{
    m_lpHinge = NULL;
}

RbFirmataDynamixelHinge::~RbFirmataDynamixelHinge()
{
	try
	{
        //Do not delete because we do not own it.
        m_lpHinge = NULL;
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbFirmataDynamixelHinge\r\n", "", -1, false, true);}
}

void RbFirmataDynamixelHinge::Initialize()
{
	RobotPartInterface::Initialize();

	m_lpHinge = dynamic_cast<Hinge *>(m_lpPart);
	m_lpMotorJoint = dynamic_cast<MotorizedJoint *>(m_lpPart);

	RecalculateParams();
}

void RbFirmataDynamixelHinge::SetupIO()
{
	if(!m_lpSim->InSimulation() && m_lpHinge)
	{
		SetMinSimPos(m_lpHinge->LowerLimit()->LimitPos());
		SetMaxSimPos(m_lpHinge->UpperLimit()->LimitPos());
		InitMotorData();

		//Set the next goal positions to the current ones.
		m_iNextGoalPos = m_iLastGoalPos;

		m_iNextGoalVelocity = m_iLastGoalVelocity;
	}
}
			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

