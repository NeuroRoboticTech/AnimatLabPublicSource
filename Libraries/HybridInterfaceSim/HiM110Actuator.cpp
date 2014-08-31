// HiM110Actuator.cpp: implementation of the HiM110Actuator class.
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

HiM110Actuator::HiM110Actuator() 
{
}

HiM110Actuator::~HiM110Actuator()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of HiM110Actuator\r\n", "", -1, false, true);}
}

#pragma region DataAccesMethods

float *HiM110Actuator::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	//if(strType == "READPARAMTIME")
	//	return &m_fltReadParamTime;
	//else
		return RobotPartInterface::GetDataPointer(strDataType);
}

bool HiM110Actuator::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(RobotPartInterface::SetData(strDataType, strValue, false))
		return true;

	if(strType == "SERVOID")
	{
		//ServoID((int) atoi(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void HiM110Actuator::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RobotPartInterface::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("ReadParamTime", AnimatPropertyType::Float, AnimatPropertyDirection::Get));

	aryProperties.Add(new TypeProperty("ServoID", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

#pragma endregion


void HiM110Actuator::Initialize()
{
	RobotPartInterface::Initialize();

	//m_lpHinge = dynamic_cast<Hinge *>(m_lpPart);
	m_lpParentC884 = dynamic_cast<HiC884Controller *>(m_lpParentIOControl);
}

void HiM110Actuator::SetupIO()
{
	//Put setup code here if needed.
}

void HiM110Actuator::StepIO(int iPartIdx)
{	
	//Send actual move commands here.
}

void HiM110Actuator::ShutdownIO()
{
	//If you need to do motor specific shutdown it goes here.
}

void HiM110Actuator::StepSimulation()
{
	RobotPartInterface::StepSimulation();

	//Transfer data from the motor back to the sim and 
	//sim data that will go to the motor here.
}

void HiM110Actuator::ResetSimulation()
{
	AnimatSim::Robotics::RobotPartInterface::ResetSimulation();
}

void HiM110Actuator::Load(StdUtils::CStdXml &oXml)
{
	RobotPartInterface::Load(oXml);

	oXml.IntoElem();

	oXml.OutOfElem();
}

	}			// Robotics
}				//HybridInterfaceSim

