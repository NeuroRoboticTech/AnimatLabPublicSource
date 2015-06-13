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

	m_aryData.RemoveAll();

	//m_aryData.Add(BUT_ID_WALKV, new RemoteControlData("WalkV", BUT_ID_WALKV, m_iChangeSimStepCount));
	//m_aryData.Add(BUT_ID_WALKH, new RemoteControlData("WalkH", BUT_ID_WALKH, m_iChangeSimStepCount));
	//m_aryData.Add(BUT_ID_LOOKV, new RemoteControlData("LookV", BUT_ID_LOOKV, m_iChangeSimStepCount));
	//m_aryData.Add(BUT_ID_LOOKH, new RemoteControlData("LookH", BUT_ID_LOOKH, m_iChangeSimStepCount));
	//m_aryData.Add(BUT_ID_PAN, new RemoteControlData("Pan", BUT_ID_PAN, m_iChangeSimStepCount));
	//m_aryData.Add(BUT_ID_TILT, new RemoteControlData("Tilt", BUT_ID_TILT, m_iChangeSimStepCount));
	//m_aryData.Add(BUT_ID_R1, new RemoteControlData("R1", BUT_ID_R1, m_iChangeSimStepCount));
	//m_aryData.Add(BUT_ID_R2, new RemoteControlData("R2", BUT_ID_R2, m_iChangeSimStepCount));
	//m_aryData.Add(BUT_ID_R3, new RemoteControlData("R3", BUT_ID_R3, m_iChangeSimStepCount));
	//m_aryData.Add(BUT_ID_L4, new RemoteControlData("L4", BUT_ID_L4, m_iChangeSimStepCount));
	//m_aryData.Add(BUT_ID_L5, new RemoteControlData("L5", BUT_ID_L5, m_iChangeSimStepCount));
	//m_aryData.Add(BUT_ID_L6, new RemoteControlData("L6", BUT_ID_L6, m_iChangeSimStepCount));
	//m_aryData.Add(BUT_ID_RT, new RemoteControlData("RT", BUT_ID_RT, m_iChangeSimStepCount));
	//m_aryData.Add(BUT_ID_LT, new RemoteControlData("LT", BUT_ID_LT, m_iChangeSimStepCount));

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

void RbXBeeCommander::CreateDataIDMap()
{
	m_aryDataIDMap.RemoveAll();
	m_aryDataIDMap.Add("WalkV", BUT_ID_WALKV);
	m_aryDataIDMap.Add("WalkH", BUT_ID_WALKH);
	m_aryDataIDMap.Add("LookV", BUT_ID_LOOKV);
	m_aryDataIDMap.Add("LookH", BUT_ID_LOOKH);
	m_aryDataIDMap.Add("Pan", BUT_ID_PAN);
	m_aryDataIDMap.Add("Tilt", BUT_ID_TILT);
	m_aryDataIDMap.Add("R1", BUT_ID_R1);
	m_aryDataIDMap.Add("R2", BUT_ID_R2);
	m_aryDataIDMap.Add("R3", BUT_ID_R2);
	m_aryDataIDMap.Add("L4", BUT_ID_L4);
	m_aryDataIDMap.Add("L5", BUT_ID_L5);
	m_aryDataIDMap.Add("L6", BUT_ID_L6);
	m_aryDataIDMap.Add("RT", BUT_ID_RT);
	m_aryDataIDMap.Add("LT", BUT_ID_LT);
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
	
	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RbXBeeCommander::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	AnimatSim::Robotics::RobotIOControl::QueryProperties(aryProperties);

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

				CreateDataTypes();

				ResetData();
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
	RemoteControl::ResetData();

	m_iButtons = 0;
	m_iExt = 0;

    index = -1;
	checksum = 0;
    status = 0;
}

void RbXBeeCommander::SimStarting()
{
	//If we have not opened the port yet give it another try.
	if(!m_Port.isInitialized())
		Initialize();

	//Clear out anything that happened the first time we got stuff.
	m_Port.flush();
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
		//m_aryData[BUT_ID_WALKH].m_fltValue = 50;

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
				else
					index = -1;  //Start over if the second byte is not 0xff
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
							SetDataValue(BUT_ID_WALKV, (float) ((signed char)( (int)vals[0]-128 )));
							SetDataValue(BUT_ID_WALKH, (float) ((signed char)( (int)vals[1]-128 ) + 1));
							SetDataValue(BUT_ID_LOOKV, (float) ((signed char)( (int)vals[2]-128 )));
							SetDataValue(BUT_ID_LOOKH, (float) ((signed char)( (int)vals[3]-128 )));
						}
						else
						{
							SetDataValue(BUT_ID_LOOKV, (float) ((signed char)( (int)vals[0]-128 )));
							SetDataValue(BUT_ID_LOOKH, (float) ((signed char)( (int)vals[1]-128 )));
							SetDataValue(BUT_ID_WALKV, (float) ((signed char)( (int)vals[2]-128 )));
							SetDataValue(BUT_ID_WALKH, (float) ((signed char)( (int)vals[3]-128 ) - 1));
						}

						SetDataValue(BUT_ID_PAN, (float) ((vals[0]<<8) + vals[1]));
						SetDataValue(BUT_ID_TILT, (float) ((vals[2]<<8) + vals[3]));

						m_iButtons = vals[4];
						m_iExt = vals[5];

						SetDataValue(BUT_ID_R1, (m_iButtons & BUT_R1));
						SetDataValue(BUT_ID_R2, (m_iButtons & BUT_R2));
						SetDataValue(BUT_ID_R3, (m_iButtons & BUT_R3));
						SetDataValue(BUT_ID_L4, (m_iButtons & BUT_L4));
						SetDataValue(BUT_ID_L5, (m_iButtons & BUT_L5));
						SetDataValue(BUT_ID_L6, (m_iButtons & BUT_L6));
						SetDataValue(BUT_ID_RT, (m_iButtons & BUT_RT));
						SetDataValue(BUT_ID_LT, (m_iButtons & BUT_LT));
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

