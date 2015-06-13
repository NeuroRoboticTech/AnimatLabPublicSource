// RbAnimatSerial.cpp: implementation of the RbAnimatSerial class.
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
#include "RbAnimatSerial.h"

#define PACKET_INFO_SIZE 6  //Size of the header, packet size, message id, and checksum in bytes
#define START_MESSAGE_INFO_BYTE 5
#define DATA_SIZE 6

//Union to store bytes and float on top of each other
typedef union {
    unsigned char b[4];
    float f;
} bfloat;

namespace RoboticsAnimatSim
{
	namespace Robotics
	{
		namespace RobotIOControls
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

RbAnimatSerial::RbAnimatSerial() 
{
	m_strPort = "";
	m_iBaudRate = 38400; 

    m_index = -1;
	m_checksum = 0;
	m_iMessageID  = -1;
	m_iPacketSize = 0;
	m_bUseRemoteDataTypes = false;

	for(int iIndex=0; iIndex<MAX_ANIMAT_BUFFER; iIndex++)
		m_vals[iIndex] = 0;
}

RbAnimatSerial::~RbAnimatSerial()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of RbAnimatSerial\r\n", "", -1, false, true);}
}

void RbAnimatSerial::Port(std::string strPort)
{
	m_strPort = strPort;
}

std::string RbAnimatSerial::Port() {return m_strPort;}

void RbAnimatSerial::BaudRate(int iRate)
{
	Std_IsAboveMin((int) 0, iRate, true, "BaudRate");
	m_iBaudRate = iRate;
}

int RbAnimatSerial::BaudRate() {return m_iBaudRate;}

#pragma region DataAccesMethods

bool RbAnimatSerial::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(AnimatSim::Robotics::RemoteControl::SetData(strDataType, strValue, false))
		return true;

	if(strType == "PORT")
	{
		Port(strValue);
		Initialize();
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

void RbAnimatSerial::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	AnimatSim::Robotics::RemoteControl::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Port", AnimatPropertyType::String, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("BaudRate", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

#pragma endregion

///We need to override this method because it does not care if it is in simulation mode or not.
void RbAnimatSerial::Initialize()
{
	// Open device. Do this before calling the Initialize on the parts so they can have communications.
	if(m_bEnabled)
	{
		//If the thread is running already then shut it down.
		if(m_bSetupComplete)
			ShutdownIO();

		if(!Std_IsBlank(m_strPort))
		{
			if(OpenIO())
			{
				StartIOThread();

				int iCount = m_aryLinks.GetSize();
				for(int iIndex=0; iIndex<iCount; iIndex++)
					m_aryLinks[iIndex]->Initialize();

				//If this remote control is not pre-defined with data types in the derived class
				//then we need to create them now from the remote linkages.
				CreateDataTypes();

				ResetData();
			}
		}
	}
}

bool RbAnimatSerial::OpenIO()
{
	bool bOpen = m_Port.setup(m_strPort, m_iBaudRate);

	if(!m_lpSim->InSimulation() && !bOpen)
		THROW_PARAM_ERROR(Rb_Err_lFailedUartSBeeConnection, Rb_Err_strFailedUartSBeeConnection, "ComPort", m_strPort);

	return bOpen;
}

void RbAnimatSerial::CloseIO()
{
	m_Port.close();
}

void RbAnimatSerial::SimStarting()
{
	//If we have not opened the port yet give it another try.
	if(!m_Port.isInitialized())
		Initialize();

	//Clear out anything that happened the first time we got stuff.
	m_Port.flush();
}

void RbAnimatSerial::ResetData()
{
	RemoteControl::ResetData();

    m_index = -1;
	m_checksum = 0;
	m_iMessageID  = -1;
	m_iPacketSize = 0;
}

void RbAnimatSerial::WaitForThreadNotifyReady()
{
	RobotIOControl::WaitForThreadNotifyReady();

	//Give it just a bit of time to start waiting if required.
	boost::this_thread::sleep(boost::posix_time::microseconds(10000));
}

void RbAnimatSerial::ReadData()
{
	bool bFound = false;

	while(m_Port.available() > 0 && !bFound)
	{
		//Get first header byte
		if(m_index == -1)
		{        
			// looking for new packet
			if(m_Port.readByte() == 0xff)
			{
				m_vals[0] = 0xff;
				m_checksum = (int) m_vals[0];
				m_index = 1;
				m_iMessageID  = -1;
				m_iPacketSize = 0;
			}
		}
		//Get second header byte
		else if(m_index == 1)
		{
			m_vals[m_index] = (unsigned char) m_Port.readByte();
			if(m_vals[m_index] == 0xff)
			{            
				m_checksum += (int) m_vals[m_index];
				m_index++;
			}
			else
				m_index = -1;  //Start over if the second byte is not 0xff
		}
		//Get the message ID
		else if(m_index==2)
		{
			m_vals[m_index] = (unsigned char) m_Port.readByte();
			m_iMessageID = m_vals[m_index];

			m_checksum += (int) m_vals[m_index];
			m_index++;
		}
		//Get the message size byte 1
		else if(m_index==3)
		{
			m_vals[m_index] = (unsigned char) m_Port.readByte();
			m_iPacketSize = m_vals[m_index];

			m_checksum += (int) m_vals[m_index];
			m_index++;
		}
		//Get the message size byte 2
		else if(m_index==4)
		{
			m_vals[m_index] = (unsigned char) m_Port.readByte();
			m_iPacketSize += (m_vals[m_index] << 8);

			//If the message size is greater than 128 then something is wrong. Start over.
			if(m_iPacketSize > 128)
				m_index = -1;
			else
			{
				m_checksum += (int) m_vals[m_index];
				m_index++;
			}
		}
		else if(m_index > 4 && m_index <= m_iPacketSize)
		{
			m_vals[m_index] = (unsigned char) m_Port.readByte();

			if(m_index == (m_iPacketSize-1))
			{ // packet complete
				//The transmitted checksum is the last entry
				int iChecksum = m_vals[m_index];

				if(m_checksum%256 != iChecksum)
				{
					// packet error!
					m_index = -1;
					return; // 0
				}
				else
				{
					int iStop = m_iPacketSize - 1; //Exclude the checksum at the end

					for(int iIdx=START_MESSAGE_INFO_BYTE; iIdx<iStop; iIdx+=DATA_SIZE)
					{
						m_id.bval[0] = m_vals[iIdx];
						m_id.bval[1] = m_vals[iIdx+1];
						m_value.bval[0] = m_vals[iIdx+2];
						m_value.bval[1] = m_vals[iIdx+3];
						m_value.bval[2] = m_vals[iIdx+4];
						m_value.bval[3] = m_vals[iIdx+5];

						SetDataValue(m_id.ival, m_value.fval);
					}
				}

				m_index = -1;
				m_Port.flush();
				bFound = true;
			}
			else
			{
				m_checksum += (int) m_vals[m_index];
				m_index++;
			}

			if(m_index >= MAX_ANIMAT_BUFFER)
				m_index = -1;
		}

	}
}

void RbAnimatSerial::StepIO()
{
	if(!m_lpSim->Paused())
	{
		ReadData();
		CheckStartedStopped();
		AnimatSim::Robotics::RemoteControl::StepIO();
	}
}

void RbAnimatSerial::Load(StdUtils::CStdXml &oXml)
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

