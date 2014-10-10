// HiSpike2.cpp: implementation of the HiSpike2 class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include <stdarg.h>
#include "HiSpike2.h"

namespace HybridInterfaceSim
{
	namespace Robotics
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

HiSpike2::HiSpike2() 
{
	m_iPortNumber = 3;
	m_iCounter = 0;
	m_iInternalData = 0;
	m_fltData = 0;
}

HiSpike2::~HiSpike2()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of HiSpike2\r\n", "", -1, false, true);}
}

void HiSpike2::PortNumber(int iPort)
{
	Std_IsAboveMin((int) 0, iPort, true, "PortNumber", true);
	m_iPortNumber = iPort;
}

int HiSpike2::PortNumber() {return m_iPortNumber;}

#pragma region DataAccesMethods

float *HiSpike2::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "DATA")
		return &m_fltData;

	return RemoteControl::GetDataPointer(strDataType);
}

bool HiSpike2::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(RemoteControl::SetData(strDataType, strValue, false))
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

void HiSpike2::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RemoteControl::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Data", AnimatPropertyType::Integer, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("PortNumber", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

#pragma endregion

void HiSpike2::Initialize()
{
	OpenIO();

	StartIOThread();

	RemoteControl::Initialize();
}

void HiSpike2::ResetSimulation()
{
	RemoteControl::ResetSimulation();

	m_iCounter = 0;
	m_iInternalData = 0;
	m_fltData = 0;
}

bool HiSpike2::OpenIO()
{
	//Open spike 2 communications ports here
	return true;
}

void HiSpike2::CloseIO()
{
	//Close spike 2 communications ports here
}

void HiSpike2::StepIO()
{
	if(m_bEnabled && !m_lpSim->Paused())
	{

		//This is just test code to demonstrate how it will only fire when m_fltData matches
		//a speicfic value. You just need to set m_fltData to the unsigned int value that you
		//get from spike 2. We are using floats here instead of ints because of the generic 
		//method I use within animatlab to get data.
		m_fltData = 0;
		m_iCounter++;

		if(m_iCounter == 10)
		{
			m_iInternalData++;
			m_fltData = (float) m_iInternalData;
			m_iCounter = 0;
		}

		if(m_iInternalData == 10)
			m_iInternalData = 0;
	}

	//Temp code to slow this down like there was real communication
	boost::this_thread::sleep(boost::posix_time::microseconds(1000));

	RemoteControl::StepIO();
}

void HiSpike2::Load(StdUtils::CStdXml &oXml)
{
	RemoteControl::Load(oXml);

	oXml.IntoElem();
	PortNumber(oXml.GetChildInt("PortNumber", m_iPortNumber));
	oXml.OutOfElem();
}


	}			// Robotics
}				//HybridInterfaceSim

