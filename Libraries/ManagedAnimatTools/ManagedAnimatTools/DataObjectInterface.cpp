#include "stdafx.h"
#include "Util.h"
#include "Logger.h"
#include "PropertyUpdateException.h"
#include "SimulatorInterface.h"
#include "SimGUICallback.h"
#include "DataObjectInterface.h"
#include "MovableItemCallback.h"

namespace AnimatGUI
{
	namespace Interfaces
	{

DataObjectInterface::DataObjectInterface(Interfaces::SimulatorInterface ^SimInt, String ^strID)
{
	m_aryDataPointers = NULL;
	m_Sim = SimInt;
	m_lpSim = m_Sim->Sim();
	string strSID = Util::StringToStd(strID);
	m_lpBase = m_lpSim->FindByID(strSID);
	m_lpMovable = dynamic_cast<MovableItem *>(m_lpBase);

	//If this is a bodypart or struture type then lets add the callbacks to it so those
	//classes can fire managed events back up to the gui.
	MovableItemCallback *lpCallback = NULL;
	BodyPart *lpPart = dynamic_cast<BodyPart *>(m_lpBase);
	Structure *lpStruct = dynamic_cast<Structure *>(m_lpBase);
	if(lpPart)
	{
		lpCallback = new MovableItemCallback(this);
		lpPart->Callback(lpCallback);
		GetPointers();
	}
	else if(lpStruct)
	{
		lpCallback = new MovableItemCallback(this);
		lpStruct->Callback(lpCallback);
		GetPointers();
	}
}

DataObjectInterface::!DataObjectInterface()
{
	try
	{
		if(m_aryDataPointers)
		{
			m_aryDataPointers->RemoveAll();
			delete m_aryDataPointers;
		}
	}
	catch(...)
	{
	}
}

DataObjectInterface::~DataObjectInterface()
{
	this->!DataObjectInterface();
}

void DataObjectInterface::GetPointers()
{
	try
	{
		m_lpPositionX = m_lpBase->GetDataPointer("POSITIONX");
		m_lpPositionY = m_lpBase->GetDataPointer("POSITIONY");
		m_lpPositionZ = m_lpBase->GetDataPointer("POSITIONZ");

		m_lpWorldPositionX = m_lpBase->GetDataPointer("WORLDPOSITIONX");
		m_lpWorldPositionY = m_lpBase->GetDataPointer("WORLDPOSITIONY");
		m_lpWorldPositionZ = m_lpBase->GetDataPointer("WORLDPOSITIONZ");

		m_lpRotationX = m_lpBase->GetDataPointer("ROTATIONX");
		m_lpRotationY = m_lpBase->GetDataPointer("ROTATIONY");
		m_lpRotationZ = m_lpBase->GetDataPointer("ROTATIONZ");
	}
	catch(...)
	{
		m_lpPositionX = NULL;
		m_lpPositionY = NULL;
		m_lpPositionZ = NULL;

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
		if(m_lpSim) m_lpSim->UnblockSimulation();
		string strError = "An error occurred while attempting to set a data value.\nError: " + oError.m_strError;
		String ^strErrorMessage = gcnew String(strError.c_str());
		throw gcnew PropertyUpdateException(strErrorMessage);
	}
	catch(System::Exception ^ex)
	{throw ex;}
	catch(...)
	{
		if(m_lpSim) m_lpSim->UnblockSimulation();
		String ^strErrorMessage = "An unknown error occurred while attempting to set a data value.";
		throw gcnew System::Exception(strErrorMessage);
	}
	return false;
}

void DataObjectInterface::SelectItem(bool bVal, bool bSelectMultiple)
{
	try
	{
		if(m_lpSim->WaitForSimulationBlock())
		{
			if(m_lpBase && ((BOOL) bVal) != m_lpBase->Selected())
				m_lpBase->Selected( (BOOL) bVal, (BOOL) bSelectMultiple);

			m_lpSim->UnblockSimulation();
		}
	}
	catch(CStdErrorInfo oError)
	{
		if(m_lpSim) m_lpSim->UnblockSimulation();
		string strError = "An error occurred while attempting to select an item.\nError: " + oError.m_strError;
		String ^strErrorMessage = gcnew String(strError.c_str());
		throw gcnew PropertyUpdateException(strErrorMessage);
	}
	catch(System::Exception ^ex)
	{throw ex;}
	catch(...)
	{
		if(m_lpSim) m_lpSim->UnblockSimulation();
		String ^strErrorMessage = "An unknown error occurred while attempting to select an item.";
		throw gcnew System::Exception(strErrorMessage);
	}
}

void DataObjectInterface::GetDataPointer(String ^sData)
{
	try
	{
		if(m_lpBase) 
		{
			string strData = Util::StringToStd(sData);
			float *lpData = m_lpBase->GetDataPointer(strData);

			if(!lpData) 
				throw gcnew System::Exception("The data pointer is not defined!");

			if(!m_aryDataPointers)
				m_aryDataPointers = new CStdMap<string, float *>;
			
			m_aryDataPointers->Add(Std_CheckString(strData), lpData);
		}
	}
	catch(CStdErrorInfo oError)
	{
		string strError = "An error occurred while attempting to get a data pointer.\nError: " + oError.m_strError;
		String ^strErrorMessage = gcnew String(strError.c_str());
		throw gcnew PropertyUpdateException(strErrorMessage);
	}
	catch(System::Exception ^ex)
	{throw ex;}
	catch(...)
	{
		String ^strErrorMessage = "An unknown error occurred while attempting to get a data pointer.";
		throw gcnew System::Exception(strErrorMessage);
	}
}

float DataObjectInterface::GetDataValue(String ^sData)
{
	if(!m_lpBase) 
		throw gcnew System::Exception("The base object has not been defined!");

	if(!m_aryDataPointers) 
		throw gcnew System::Exception("The data pointers array has not been defined!");

	try
	{
		string strData = Util::StringToStd(sData);
		float *lpData = NULL;

		CStdMap<string, float *>::iterator oPos;
		oPos = m_aryDataPointers->find(Std_CheckString(strData));

		if(oPos != m_aryDataPointers->end())
			lpData =  oPos->second;
		else
			throw gcnew System::Exception("The data pointer has not been defined: " + sData);

		if(!lpData) 
			throw gcnew System::Exception("The data pointer is not defined: " + sData);

		return *lpData;
	}
	catch(CStdErrorInfo oError)
	{
		string strError = "An error occurred while attempting to get a data value.\nError: " + oError.m_strError;
		String ^strErrorMessage = gcnew String(strError.c_str());
		throw gcnew PropertyUpdateException(strErrorMessage);
	}
	catch(System::Exception ^ex)
	{throw ex;}
	catch(...)
	{
		String ^strErrorMessage = "An unknown error occurred while attempting to get a data value.";
		throw gcnew System::Exception(strErrorMessage);
	}
}


float DataObjectInterface::GetBoundingBoxValue(int iIndex)
{
	if(m_lpMovable)
	{
		BoundingBox bb = m_lpMovable->GetBoundingBox();

		if(iIndex == 0 && m_lpRotationX)
			return bb.Length();
		else if(iIndex == 1 && m_lpRotationY)
			return bb.Height();
		else if(iIndex == 2 && m_lpRotationZ)
			return bb.Width();
		else
			return 0;
	}
	else
		return 0;
}

void DataObjectInterface::OrientNewPart(double dblXPos, double dblYPos, double dblZPos, double dblXNorm, double dblYNorm, double dblZNorm)
{
	if(m_lpMovable)
		m_lpMovable->OrientNewPart(dblXPos, dblYPos, dblZPos, dblXNorm, dblYNorm, dblZNorm); 
}

	}
}