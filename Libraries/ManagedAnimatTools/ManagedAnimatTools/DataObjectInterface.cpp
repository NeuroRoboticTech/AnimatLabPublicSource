#include "stdafx.h"
#include "Util.h"
#include "Logger.h"
#include "PropertyUpdateException.h"
#include "SimulatorInterface.h"
#include "SimGUICallback.h"
#include "DataObjectInterface.h"
#include "BodyPartCallback.h"

namespace AnimatGUI
{
	namespace Interfaces
	{

DataObjectInterface::DataObjectInterface(Interfaces::SimulatorInterface ^SimInt, String ^strID)
{
	m_Sim = SimInt;
	m_lpSim = m_Sim->Sim();
	string strSID = Util::StringToStd(strID);
	m_lpBase = m_lpSim->FindByID(strSID);

	//If this is a bodypart or struture type then lets add the callbacks to it so those
	//classes can fire managed events back up to the gui.
	BodyPartCallback *lpCallback = NULL;
	BodyPart *lpPart = dynamic_cast<BodyPart *>(m_lpBase);
	Structure *lpStruct = dynamic_cast<Structure *>(m_lpBase);
	if(lpPart)
	{
		lpCallback = new BodyPartCallback(this);
		lpPart->Callback(lpCallback);
		GetPointers();
	}
	else if(lpStruct)
	{
		lpCallback = new BodyPartCallback(this);
		lpStruct->Callback(lpCallback);
		GetPointers();
	}
}

void DataObjectInterface::GetPointers()
{
	try
	{
		m_lpLocalPositionX = m_lpBase->GetDataPointer("LOCALPOSITIONX");
		m_lpLocalPositionY = m_lpBase->GetDataPointer("LOCALPOSITIONY");
		m_lpLocalPositionZ = m_lpBase->GetDataPointer("LOCALPOSITIONZ");

		m_lpWorldPositionX = m_lpBase->GetDataPointer("WORLDPOSITIONX");
		m_lpWorldPositionY = m_lpBase->GetDataPointer("WORLDPOSITIONY");
		m_lpWorldPositionZ = m_lpBase->GetDataPointer("WORLDPOSITIONZ");

		m_lpRotationX = m_lpBase->GetDataPointer("ROTATIONX");
		m_lpRotationY = m_lpBase->GetDataPointer("ROTATIONY");
		m_lpRotationZ = m_lpBase->GetDataPointer("ROTATIONZ");
	}
	catch(...)
	{
		m_lpLocalPositionX = NULL;
		m_lpLocalPositionY = NULL;
		m_lpLocalPositionZ = NULL;

		m_lpWorldPositionX = NULL;
		m_lpWorldPositionY = NULL;
		m_lpWorldPositionZ = NULL;

		m_lpRotationX = NULL;
		m_lpRotationY = NULL;
		m_lpRotationZ = NULL;
	}
}

System::Boolean DataObjectInterface::SetData(String ^sDataType, String ^sValue, System::Boolean bThrowError)
{
	try
	{
		if(m_lpBase) 
		{
			if(m_lpSim->WaitForSimulationBlock())
			{
				string strDataType = Std_Trim(Std_ToUpper(Util::StringToStd(sDataType)));
				string strValue = Util::StringToStd(sValue);
				BOOL bVal = m_lpBase->SetData(strDataType, strValue, bThrowError);

				m_lpSim->UnblockSimulation();

				return bVal;
			}
			else
				return false;
		}
		else
			return false;
	}
	catch(CStdErrorInfo oError)
	{
		string strError = "An error occurred while attempting to set a data value.\nError: " + oError.m_strError;
		String ^strErrorMessage = gcnew String(strError.c_str());
		throw gcnew PropertyUpdateException(strErrorMessage);
	}
	catch(System::Exception ^ex)
	{throw ex;}
	catch(...)
	{
		String ^strErrorMessage = "An unknown error occurred while attempting to set a data value.";
		throw gcnew System::Exception(strErrorMessage);
	}
	return false;
}

void DataObjectInterface::SelectItem(bool bVal, bool bSelectMultiple)
{
	if(m_lpSim->WaitForSimulationBlock())
	{
		if(m_lpBase && ((BOOL) bVal) != m_lpBase->Selected())
			m_lpBase->Selected( (BOOL) bVal, (BOOL) bSelectMultiple);

		m_lpSim->UnblockSimulation();
	}
}


	}
}