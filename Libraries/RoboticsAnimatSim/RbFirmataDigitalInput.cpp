// RbFirmataDigitalInput.cpp: implementation of the RbFirmataDigitalInput class.
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
#include "RbFirmataDigitalInput.h"
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

RbFirmataDigitalInput::RbFirmataDigitalInput() 
{
}

RbFirmataDigitalInput::~RbFirmataDigitalInput()
{
}

void RbFirmataDigitalInput::SetupIO()
{
	if(!m_lpSim->InSimulation() && m_lpFirmata)
		m_lpFirmata->sendDigitalPinMode(m_iIOComponentID, ARD_INPUT);
}

void RbFirmataDigitalInput::StepIO(int iPartIdx)
{
	if(!m_lpSim->InSimulation())
	{
		int iValue = m_lpFirmata->getDigital(m_iIOComponentID);

		if(iValue != -1 && iValue != m_iIOValue)
		{
			m_bChanged = true;
			IOValue(iValue);

			if(iValue)
				std::cout << "Pin " << m_iIOComponentID << " Turned ON." << "\r\n";
			else
				std::cout << "Pin " << m_iIOComponentID << " Turned OFF." << "\r\n";
		}
	}
}

void RbFirmataDigitalInput::StepSimulation()
{
    RobotPartInterface::StepSimulation();

	if(m_bChanged)
	{
		m_bChanged = false;

		//Calculate the gain of the IO value.
		float fltValue = m_lpGain->CalculateGain(m_fltIOValue);

		//Remove any previously added value from the param
		*m_lpProperty -= m_fltIOScaledValue;

		m_fltIOScaledValue = fltValue;

		//Add the value back.
		*m_lpProperty += m_fltIOScaledValue;
	}
}

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

