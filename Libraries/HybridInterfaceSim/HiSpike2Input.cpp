// HiSpike2Input.cpp: implementation of the HiSpike2Input class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "HiSpike2Input.h"

namespace HybridInterfaceSim
{
	namespace Robotics
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

HiSpike2Input::HiSpike2Input() 
{
	m_iPortNumber = 3;
}

HiSpike2Input::~HiSpike2Input()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of HiSpike2Input\r\n", "", -1, false, true);}
}

void HiSpike2Input::PortNumber(int iPort)
{
	Std_IsAboveMin((int) 0, iPort, true, "PortNumber", true);
	m_iPortNumber = iPort;
}

int HiSpike2Input::PortNumber() {return m_iPortNumber;}

#pragma region DataAccesMethods

float *HiSpike2Input::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	return RobotIOControl::GetDataPointer(strDataType);
}

bool HiSpike2Input::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
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

void HiSpike2Input::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RobotIOControl::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("PortNumber", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

#pragma endregion

void HiSpike2Input::Initialize()
{
	// Open device. Do this before calling the Initialize on the parts so they can have communications.
	if(!m_lpSim->InSimulation())
	{

		StartIOThread();
	}

	RobotIOControl::Initialize();
}

void HiSpike2Input::ProcessIO()
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

void HiSpike2Input::ExitIOThread()
{
	RobotIOControl::ExitIOThread();

//	if(!m_lpSim->InSimulation())
//		dxl_terminate();
}


void HiSpike2Input::Load(StdUtils::CStdXml &oXml)
{
	RobotIOControl::Load(oXml);

	oXml.IntoElem();
	PortNumber(oXml.GetChildInt("PortNumber", m_iPortNumber));
	oXml.OutOfElem();
}


	}			// Robotics
}				//HybridInterfaceSim

