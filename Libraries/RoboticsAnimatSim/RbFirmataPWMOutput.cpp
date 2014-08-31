// RbFirmataPWMOutput.cpp: implementation of the RbFirmataPWMOutput class.
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
#include "RbFirmataPWMOutput.h"
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

RbFirmataPWMOutput::RbFirmataPWMOutput() 
{
}

RbFirmataPWMOutput::~RbFirmataPWMOutput()
{
}

void RbFirmataPWMOutput::SetupIO()
{
	if(!m_lpSim->InSimulation() && m_lpFirmata)
		m_lpFirmata->sendDigitalPinMode(m_iIOComponentID, ARD_PWM);
}

void RbFirmataPWMOutput::StepIO(int iPartIdx)
{
	if(!m_lpSim->InSimulation())
	{
		if(m_iIOValue != m_lpFirmata->getPwm(m_iIOComponentID))
		{
			std::cout << "PWM: " << m_iIOValue << " OFF." << "\r\n";

			m_lpFirmata->sendPwm(m_iIOComponentID, m_iIOValue, false);
		}
	}
}

void RbFirmataPWMOutput::StepSimulation()
{
    RobotPartInterface::StepSimulation();

	//If it is associated with a part property then we need to get that value.
	//run it through the gain to transform it, round it out to int, and then
	//send it if the value has changed.
	if(m_lpProperty &&m_lpGain && m_lpFirmata)
	{
		m_fltIOScaledValue = m_lpGain->CalculateGain(*m_lpProperty);

		if(m_fltIOScaledValue < 0) m_fltIOScaledValue = 0;
		if(m_fltIOScaledValue > 255) m_fltIOScaledValue = 255;

		m_iIOValue = (int) m_fltIOScaledValue;
		m_fltIOValue = m_fltIOScaledValue;
	}
}

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

