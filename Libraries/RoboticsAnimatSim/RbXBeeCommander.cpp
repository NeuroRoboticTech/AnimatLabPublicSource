// RbXBeeCommander.cpp: implementation of the RbXBeeCommander class.
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
#include "RbXBeeCommander.h"

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbXBeeCommander::RbXBeeCommander() 
{
	m_strPort = "COM3";
	m_iBaudRate = 38400; 

    m_iWalkV = 0;
	m_iWalkH = 0;
	m_iLookV = 0;
	m_iLookH = 0;
	m_iPan = 0;
    m_iTilt = 0;
    m_iButtons = 0;
	m_iExt = 0;

    index = -1;
    status = 0;
}

RbXBeeCommander::~RbXBeeCommander()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbXBeeCommander\r\n", "", -1, false, true);}
}

void RbXBeeCommander::Port(std::string strPort)
{
	m_strPort = strPort;
}

std::string RbXBeeCommander::Port() {return m_strPort;}

void RbXBeeCommander::BaudRate(int iRate)
{
	Std_IsAboveMin((int) 0, iRate, true, "BaudRate");
	m_iBaudRate = iRate;
}

int RbXBeeCommander::BaudRate() {return m_iBaudRate;}

#pragma region DataAccesMethods

float *RbXBeeCommander::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	return AnimatSim::Robotics::RemoteControl::GetDataPointer(strDataType);
}

bool RbXBeeCommander::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(AnimatSim::Robotics::RemoteControl::SetData(strDataType, strValue, false))
		return true;

	if(strType == "PORT")
	{
		Port(strValue);
		return true;
	}
	else if(strType == "BAUDRATE")
	{
		BaudRate((int) atoi(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RbXBeeCommander::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	AnimatSim::Robotics::RemoteControl::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Port", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("BaudRate", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

#pragma endregion

void RbXBeeCommander::Initialize()
{
	// Open device. Do this before calling the Initialize on the parts so they can have communications.
	//if(m_bEnabled)
	if(!m_lpSim->InSimulation())
	{
		if(!m_Port.setup(m_strPort, m_iBaudRate))
			THROW_PARAM_ERROR(Rb_Err_lFailedUartSBeeConnection, Rb_Err_strFailedUartSBeeConnection, "Port", m_strPort);

		StartIOThread();
	}

	AnimatSim::Robotics::RemoteControl::Initialize();
}

void RbXBeeCommander::ProcessIO()
{
	try
	{
		m_bIOThreadProcessing = true;

		SetupIO();

		m_bSetupComplete = true;
		m_WaitForIOSetupCond.notify_all();

		while(!m_bStopIO)
			StepIO();
	}
	catch(CStdErrorInfo oError)
	{
		m_bIOThreadProcessing = false;
	}
	catch(...)
	{
		m_bIOThreadProcessing = false;
	}

	m_bIOThreadProcessing = false;
}

void RbXBeeCommander::StepIO()
{
	while(m_Port.available() > 0)
	{
		if(index == -1)
		{         // looking for new packet
			if(m_Port.readByte() == 0xff)
			{
				index = 0;
				checksum = 0;
			}
		}
		else if(index == 0)
		{
			vals[index] = (unsigned char) m_Port.readByte();
			if(vals[index] != 0xff)
			{            
				checksum += (int) vals[index];
				index++;
			}
		}
		else
		{
			vals[index] = (unsigned char) m_Port.readByte();
			checksum += (int) vals[index];
			index++;
			if(index == 7)
			{ // packet complete
				if(checksum%256 != 255)
				{
					// packet error!
					index = -1;
					return; // 0
				}
				else
				{
					if((status&0x01) > 0)
					{     // SouthPaw
						m_iWalkV = (signed char)( (int)vals[0]-128 );
						m_iWalkH = (signed char)( (int)vals[1]-128 );
						m_iLookV = (signed char)( (int)vals[2]-128 );
						m_iLookH = (signed char)( (int)vals[3]-128 );
					}
					else
					{
						m_iLookV = (signed char)( (int)vals[0]-128 );
						m_iLookH = (signed char)( (int)vals[1]-128 );
						m_iWalkV = (signed char)( (int)vals[2]-128 );
						m_iWalkH = (signed char)( (int)vals[3]-128 );
					}
					m_iPan = (vals[0]<<8) + vals[1];
					m_iTilt = (vals[2]<<8) + vals[3];
					m_iButtons = vals[4];
					m_iExt = vals[5];
				}
				index = -1;
				m_Port.flush();
				return; // 1
			}
		}
	}
		
    return; // 0
}

void RbXBeeCommander::ExitIOThread()
{
	AnimatSim::Robotics::RemoteControl::ExitIOThread();

	m_Port.close();
}

void RbXBeeCommander::Load(StdUtils::CStdXml &oXml)
{
	AnimatSim::Robotics::RemoteControl::Load(oXml);

	oXml.IntoElem();
	Port(oXml.GetChildString("Port", m_strPort));
	BaudRate(oXml.GetChildInt("BaudRate", m_iBaudRate));
	oXml.OutOfElem();
}


		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

