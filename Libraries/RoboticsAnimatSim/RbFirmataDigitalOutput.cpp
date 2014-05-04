// RbFirmataDigitalOutput.cpp: implementation of the RbFirmataDigitalOutput class.
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
#include "RbFirmataDigitalOutput.h"
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

RbFirmataDigitalOutput::RbFirmataDigitalOutput() 
{
}

RbFirmataDigitalOutput::~RbFirmataDigitalOutput()
{
}

void RbFirmataDigitalOutput::SetupIO()
{
	m_lpFirmata->sendDigitalPinMode(m_iIOComponentID, ARD_OUTPUT);
}

void RbFirmataDigitalOutput::StepIO()
{
	if(!m_lpParentInterface->InSimulation())
	{
		int iValue = (int) round(m_fltIOValue);

		if(iValue != m_lpFirmata->getDigital(m_iIOComponentID))
		{
			if(iValue)
				std::cout << "Turning pin " << m_iIOComponentID << " ON." << "\r\n";
			else
				std::cout << "Turning pin " << m_iIOComponentID << " OFF." << "\r\n";

			m_lpFirmata->sendDigital(m_iIOComponentID, iValue);
		}
	}
}

void RbFirmataDigitalOutput::StepSimulation()
{
    RobotPartInterface::StepSimulation();

	//If it is associated with a part property then we need to get that value.
	//run it through the gain to transform it, round it out to int, and then
	//send it if the value has changed.
	if(m_lpProperty &&m_lpGain && m_lpFirmata)
	{
		float fltValue = m_lpGain->CalculateGain(*m_lpProperty);

		int iValue = 0;
		if(fltValue > 0.5f)
			iValue = 1;

		m_fltIOValue = iValue;
	}

}

			}	//Firmata
		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

