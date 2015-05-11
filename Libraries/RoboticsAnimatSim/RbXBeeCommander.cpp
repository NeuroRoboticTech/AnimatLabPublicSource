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
	m_strPort = "";
	m_iBaudRate = 38400; 
	m_iChangeSimStepCount = 5;

	ResetData();
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

void RbXBeeCommander::ChangeSimStepCount(int iRate)
{
	Std_IsAboveMin((int) 0, iRate, true, "ChangeSimStepCount");
	m_iChangeSimStepCount = iRate;

	//Reset all the button data.
	for(int i=0; i<BUT_ID_TOTAL; i++)
		m_ButtonData[i].m_iChangeSimStepCount = m_iChangeSimStepCount;
}

int RbXBeeCommander::ChangeSimStepCount() {return m_iChangeSimStepCount;}

#pragma region DataAccesMethods

float *RbXBeeCommander::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "WALKV")
		return &m_ButtonData[BUT_ID_WALKV].m_fltValue;
	else if(strType == "WALKVSTART")
		return &m_ButtonData[BUT_ID_WALKV].m_fltStart;
	else if(strType == "WALKVSTOP")
		return &m_ButtonData[BUT_ID_WALKV].m_fltStop;
	else if(strType == "WALKH")
		return &m_ButtonData[BUT_ID_WALKH].m_fltValue;
	else if(strType == "WALKHSTART")
		return &m_ButtonData[BUT_ID_WALKH].m_fltStart;
	else if(strType == "WALKHSTOP")
		return &m_ButtonData[BUT_ID_WALKH].m_fltStop;
	else if(strType == "LOOKV")
		return &m_ButtonData[BUT_ID_LOOKV].m_fltValue;
	else if(strType == "LOOKVSTART")
		return &m_ButtonData[BUT_ID_LOOKV].m_fltStart;
	else if(strType == "LOOKVSTOP")
		return &m_ButtonData[BUT_ID_LOOKV].m_fltStop;
	else if(strType == "LOOKH")
		return &m_ButtonData[BUT_ID_LOOKH].m_fltValue;
	else if(strType == "LOOKHSTART")
		return &m_ButtonData[BUT_ID_LOOKH].m_fltStart;
	else if(strType == "LOOKHSTOP")
		return &m_ButtonData[BUT_ID_LOOKH].m_fltStop;
	else if(strType == "PAN")
		return &m_ButtonData[BUT_ID_PAN].m_fltValue;
	else if(strType == "PANSTART")
		return &m_ButtonData[BUT_ID_PAN].m_fltStart;
	else if(strType == "PANSTOP")
		return &m_ButtonData[BUT_ID_PAN].m_fltStop;
	else if(strType == "TILT")
		return &m_ButtonData[BUT_ID_TILT].m_fltValue;
	else if(strType == "TILTSTART")
		return &m_ButtonData[BUT_ID_TILT].m_fltStart;
	else if(strType == "TILTSTOP")
		return &m_ButtonData[BUT_ID_TILT].m_fltStop;
	else if(strType == "R1")
		return &m_ButtonData[BUT_ID_R1].m_fltValue;
	else if(strType == "R1START")
		return &m_ButtonData[BUT_ID_R1].m_fltStart;
	else if(strType == "R1STOP")
		return &m_ButtonData[BUT_ID_R1].m_fltStop;
	else if(strType == "R2")
		return &m_ButtonData[BUT_ID_R2].m_fltValue;
	else if(strType == "R2START")
		return &m_ButtonData[BUT_ID_R2].m_fltStart;
	else if(strType == "R2STOP")
		return &m_ButtonData[BUT_ID_R2].m_fltStop;
	else if(strType == "R3")
		return &m_ButtonData[BUT_ID_R3].m_fltValue;
	else if(strType == "R3START")
		return &m_ButtonData[BUT_ID_R3].m_fltStart;
	else if(strType == "R3STOP")
		return &m_ButtonData[BUT_ID_R3].m_fltStop;
	else if(strType == "L4")
		return &m_ButtonData[BUT_ID_L4].m_fltValue;
	else if(strType == "L4START")
		return &m_ButtonData[BUT_ID_L4].m_fltStart;
	else if(strType == "L4STOP")
		return &m_ButtonData[BUT_ID_L4].m_fltStop;
	else if(strType == "L5")
		return &m_ButtonData[BUT_ID_L5].m_fltValue;
	else if(strType == "L5START")
		return &m_ButtonData[BUT_ID_L5].m_fltStart;
	else if(strType == "L5STOP")
		return &m_ButtonData[BUT_ID_L5].m_fltStop;
	else if(strType == "L6")
		return &m_ButtonData[BUT_ID_L6].m_fltValue;
	else if(strType == "L6START")
		return &m_ButtonData[BUT_ID_L6].m_fltStart;
	else if(strType == "L6STOP")
		return &m_ButtonData[BUT_ID_L6].m_fltStop;
	else if(strType == "RT")
		return &m_ButtonData[BUT_ID_RT].m_fltValue;
	else if(strType == "RTSTART")
		return &m_ButtonData[BUT_ID_RT].m_fltStart;
	else if(strType == "RTSTOP")
		return &m_ButtonData[BUT_ID_RT].m_fltStop;
	else if(strType == "LT")
		return &m_ButtonData[BUT_ID_LT].m_fltValue;
	else if(strType == "LTSTART")
		return &m_ButtonData[BUT_ID_LT].m_fltStart;
	else if(strType == "LTSTOP")
		return &m_ButtonData[BUT_ID_LT].m_fltStop;

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
		Initialize();
		return true;
	}
	else if(strType == "BAUDRATE")
	{
		BaudRate((int) atoi(strValue.c_str()));
		return true;
	}
	else if(strType == "CHANGESIMSTEPCOUNT")
	{
		ChangeSimStepCount((int) atoi(strValue.c_str()));
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
	aryProperties.Add(new TypeProperty("ChangeSimStepCount", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));

	aryProperties.Add(new TypeProperty("WalkV", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("WalkVStart", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("WalkVStop", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("WalkH", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("WalkHStart", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("WalkHStop", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("LookV", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("LookVStart", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("LookVStop", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("LookH", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("LookHStart", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("LookHStop", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("Pan", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("PanStart", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("PanStop", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("Tilt", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("TiltStart", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("TiltStop", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("R1", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("R1Start", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("R1Stop", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("R2", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("R2Start", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("R2Stop", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("R3", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("R3Start", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("R3Stop", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("L4", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("L4Start", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("L4Stop", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("L5", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("L5Start", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("L5Stop", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("L6", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("L6Start", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("L6Stop", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("RT", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("RTStart", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("RTStop", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("LT", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("LTStart", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	aryProperties.Add(new TypeProperty("LTStop", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
}

#pragma endregion

void RbXBeeCommander::Initialize()
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
			}
		}
	}
}

bool RbXBeeCommander::OpenIO()
{
	bool bOpen = m_Port.setup(m_strPort, m_iBaudRate);

	if(!m_lpSim->InSimulation() && !bOpen)
		THROW_PARAM_ERROR(Rb_Err_lFailedUartSBeeConnection, Rb_Err_strFailedUartSBeeConnection, "ComPort", m_strPort);

	return bOpen;
	//return false;
}

void RbXBeeCommander::CloseIO()
{
	m_Port.close();
}

void RbXBeeCommander::ResetData()
{
	for(int iIdx=0; iIdx<BUT_ID_TOTAL; iIdx++)
		m_ButtonData[iIdx].ClearData();

	m_iButtons = 0;
	m_iExt = 0;

    index = -1;
    status = 0;
}

void RbXBeeCommander::ResetSimulation()
{
	RemoteControl::ResetSimulation();
	ResetData();
}

void RbXBeeCommander::SimStarting()
{
	//If we have not opened the port yet give it another try.
	if(!m_Port.isInitialized())
		Initialize();

	//Clear out anything that happened the first time we got stuff.
	m_Port.flush();
}

void RbXBeeCommanderButtonData::CheckStartedStopped()
{
	if(m_fltValue == m_fltPrev)
		m_iCount++;
	else
		m_iCount = 0;

	if(m_iCount == 3)
	{
		if(!m_bStarted && m_fltValue != 0)
		{
			m_iStartDir = Std_Sign(m_fltValue);
			m_fltStart = 1*m_iStartDir;
			m_bStarted = true;
			////Test Code
			//OutputDebugString("Start\r\n");
		}
		else if(m_bStarted && m_fltValue == 0)
		{
			m_fltStop = m_iStartDir;
			m_bStarted = false;
			////Test Code
			//OutputDebugString("Stop\r\n");
		}

		m_iCount = 0;
	}

	m_fltPrev = m_fltValue;

	////Test Code
	//std::string strVal = "Val: " + STR((int) m_fltValue) + " Prev: " + STR((int) m_fltPrev) + " Count: " + STR(m_iCount) + " Started: " + STR(m_bStarted) + " Start: " + STR((int) m_fltStart) + " Stop: " + STR((int) m_fltStop) + "\r\n";
	//OutputDebugString(strVal.c_str());
}

void RbXBeeCommander::CheckStartedStopped()
{
	for(int iIdx=0; iIdx<BUT_ID_TOTAL; iIdx++)
		m_ButtonData[iIdx].CheckStartedStopped();

	////Test Code
	//m_ButtonData[BUT_ID_RT].CheckStartedStopped();
}

void RbXBeeCommanderButtonData::ClearStartStops()
{
	if((fabs(m_fltStart) > 0 || fabs(m_fltStop) > 0))
	{
		if(m_iSimStepped >= m_iChangeSimStepCount)
		{
			////Test Code
			//if(m_fltStart > 0)
			//	OutputDebugString("Cleared Start\r\n");
			//if(m_fltStop > 0)
			//	OutputDebugString("Cleared Stop\r\n");

			m_fltStart = 0;
			m_fltStop = 0;
			m_iSimStepped = 0;
		}
		else
			m_iSimStepped++;
	}
}

void RbXBeeCommander::ClearStartStops()
{
	for(int iIdx=0; iIdx<BUT_ID_TOTAL; iIdx++)
		m_ButtonData[iIdx].ClearStartStops();

	////Test Code
	//m_ButtonData[BUT_ID_LOOKH].ClearStartStops();
}

void RbXBeeCommander::WaitForThreadNotifyReady()
{
	RobotIOControl::WaitForThreadNotifyReady();

	//Give it just a bit of time to start waiting if required.
	boost::this_thread::sleep(boost::posix_time::microseconds(10000));
}

void RbXBeeCommander::StepIO()
{
	if(!m_lpSim->Paused())
	{
		bool bFound = false;

		////Test Code
		//m_ButtonData[BUT_ID_WALKH].m_fltValue = 50;

		while(m_Port.available() > 0 && !bFound)
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
							m_ButtonData[BUT_ID_WALKV].m_fltValue = (float) ((signed char)( (int)vals[0]-128 ));
							m_ButtonData[BUT_ID_WALKH].m_fltValue = (float) ((signed char)( (int)vals[1]-128 ) + 1);
							m_ButtonData[BUT_ID_LOOKV].m_fltValue = (float) ((signed char)( (int)vals[2]-128 ));
							m_ButtonData[BUT_ID_LOOKH].m_fltValue = (float) ((signed char)( (int)vals[3]-128 ));
						}
						else
						{
							m_ButtonData[BUT_ID_LOOKV].m_fltValue = (float) ((signed char)( (int)vals[0]-128 ));
							m_ButtonData[BUT_ID_LOOKH].m_fltValue = (float) ((signed char)( (int)vals[1]-128 ));
							m_ButtonData[BUT_ID_WALKV].m_fltValue = (float) ((signed char)( (int)vals[2]-128 ));
							m_ButtonData[BUT_ID_WALKH].m_fltValue = (float) ((signed char)( (int)vals[3]-128 ) - 1);
						}
						m_ButtonData[BUT_ID_PAN].m_fltValue = (float) ((vals[0]<<8) + vals[1]);
						m_ButtonData[BUT_ID_TILT].m_fltValue = (float) ((vals[2]<<8) + vals[3]);
						m_iButtons = vals[4];
						m_iExt = vals[5];

						m_ButtonData[BUT_ID_R1].m_fltValue = (m_iButtons & BUT_R1);
						m_ButtonData[BUT_ID_R2].m_fltValue = (m_iButtons & BUT_R2);
						m_ButtonData[BUT_ID_R3].m_fltValue = (m_iButtons & BUT_R3);
						m_ButtonData[BUT_ID_L4].m_fltValue = (m_iButtons & BUT_L4);
						m_ButtonData[BUT_ID_L5].m_fltValue = (m_iButtons & BUT_L5);
						m_ButtonData[BUT_ID_L6].m_fltValue = (m_iButtons & BUT_L6);
						m_ButtonData[BUT_ID_RT].m_fltValue = (m_iButtons & BUT_RT);
						m_ButtonData[BUT_ID_LT].m_fltValue = (m_iButtons & BUT_LT);
					}

					index = -1;
					m_Port.flush();
					bFound = true;
				}
			}

		}

		CheckStartedStopped();
		AnimatSim::Robotics::RemoteControl::StepIO();
	}
}

void RbXBeeCommander::StepSimulation()
{
	RemoteControl::StepSimulation();

	////Test Code
	//int i=4;
	//if(	m_ButtonData[BUT_ID_LOOKH].m_fltStart > 0 || m_ButtonData[BUT_ID_LOOKH].m_fltStop > 0)
	//	i=6;

	ClearStartStops();
}

void RbXBeeCommander::Load(StdUtils::CStdXml &oXml)
{
	AnimatSim::Robotics::RemoteControl::Load(oXml);

	oXml.IntoElem();
	Port(oXml.GetChildString("Port", m_strPort));
	BaudRate(oXml.GetChildInt("BaudRate", m_iBaudRate));
	ChangeSimStepCount(oXml.GetChildInt("ChangeSimStepCount", m_iChangeSimStepCount));
	oXml.OutOfElem();
}


		}		//RobotIOControls
	}			// Robotics
}				//RoboticsAnimatSim

