// RbSwitchInputSensor.cpp: implementation of the RbSwitchInputSensor class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "RbMovableItem.h"
#include "RbBody.h"
#include "RbJoint.h"
#include "RbRigidBody.h"
#include "RbStructure.h"
#include "RbSwitchInputSensor.h"

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace InputSensorSystems
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbSwitchInputSensor::RbSwitchInputSensor() 
{
    m_lpBody = NULL;
}

RbSwitchInputSensor::~RbSwitchInputSensor()
{
	try
	{
        //Do not delete because we do not own it.
        m_lpBody = NULL;
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbSwitchInputSensor\r\n", "", -1, false, true);}
}

void RbSwitchInputSensor::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
    AnimatBase::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, bVerify);

    m_lpBody = dynamic_cast<RigidBody *>(lpNode);
	if(!m_lpBody) 
		THROW_PARAM_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "RigidBody: ", m_strID);
}

void RbSwitchInputSensor::StepSimulation()
{
    AnimatBase::StepSimulation();

    //Here we need to check the physical switch and get its value and then set it on the rigid body.
    int iState = 0;
    m_lpBody->SetSurfaceContactCount(iState);
}

		}		//InputSensorSystems
	}			// Robotics
}				//RoboticsAnimatSim

