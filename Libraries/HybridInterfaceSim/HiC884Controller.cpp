// HiC884Controller.cpp: implementation of the HiC884Controller class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "HiC884Controller.h"
#include "HiM110Actuator.h"

namespace HybridInterfaceSim
{
	namespace Robotics
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

HiC884Controller::HiC884Controller() 
{
	m_iPortNumber = 3;
}

HiC884Controller::~HiC884Controller()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of HiC884Controller\r\n", "", -1, false, true);}
}

void HiC884Controller::PortNumber(int iPort)
{
	Std_IsAboveMin((int) 0, iPort, true, "PortNumber", true);
	m_iPortNumber = iPort;
}

int HiC884Controller::PortNumber() {return m_iPortNumber;}

#pragma region DataAccesMethods

float *HiC884Controller::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	return RobotIOControl::GetDataPointer(strDataType);
}

bool HiC884Controller::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(RobotIOControl::SetData(strDataType, strValue, false))
		return true;

	if(strType == "PORTNUMBER")
	{
		PortNumber((int) atoi(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void HiC884Controller::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RobotIOControl::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("PortNumber", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

#pragma endregion

void HiC884Controller::Initialize()
{
	// Open device. Do this before calling the Initialize on the parts so they can have communications.
	if(!m_lpSim->InSimulation())
	{
		//if(!dxl_initialize(m_iPortNumber, m_iBaudRate))
		//	THROW_PARAM_ERROR(Rb_Err_lFailedDynamixelConnection, Rb_Err_strFailedDynamixelConnection, "Port", m_iPortNumber);

		StartIOThread();
	}

	RobotIOControl::Initialize();
}

void HiC884Controller::ProcessIO()
{
	try
	{
		m_bIOThreadProcessing = true;

		SetupIO();

		m_bSetupComplete = true;
		m_WaitForIOSetupCond.notify_all();

		while(!m_bStopIO)
		{
			StepIO();

#ifndef Win32
		//Not needed in windows, not sure in linux. Keep it in till verify.
		m_lpSim->MicroSleep(15000);
#endif
		}
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

void HiC884Controller::ExitIOThread()
{
	RobotIOControl::ExitIOThread();

	//if(!m_lpSim->InSimulation())
	//	dxl_terminate();
}

void HiC884Controller::Load(StdUtils::CStdXml &oXml)
{
	RobotIOControl::Load(oXml);

	oXml.IntoElem();
	PortNumber(oXml.GetChildInt("PortNumber", m_iPortNumber));
	oXml.OutOfElem();
}


	}			// Robotics
}				//HybridInterfaceSim

