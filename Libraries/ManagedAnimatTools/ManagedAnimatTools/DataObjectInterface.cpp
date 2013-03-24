#include "stdafx.h"
#include "Util.h"
#include "PropertyUpdateException.h"
#include "SimulatorInterface.h"
#include "SimGUICallback.h"
#include "DataObjectInterface.h"
#include "MovableItemCallback.h"

namespace AnimatGUI
{
	namespace Interfaces
	{

DataObjectInterface::DataObjectInterface(ManagedAnimatInterfaces::ISimulatorInterface ^SimInt, String ^strID)
{
	try
	{
		AnimatGUI::Interfaces::SimulatorInterface ^RealSimInt = dynamic_cast<AnimatGUI::Interfaces::SimulatorInterface ^>(SimInt);

		if(RealSimInt != nullptr)
		{
			m_aryDataPointers = NULL;
			m_Sim = RealSimInt;
			m_lpSim = RealSimInt->Sim();
			string strSID = Util::StringToStd(strID);
			m_lpBase = m_lpSim->FindByID(strSID);
			m_lpMovable = dynamic_cast<MovableItem *>(m_lpBase);
			m_lpRigidBody = dynamic_cast<RigidBody *>(m_lpBase);

			//If this is a bodypart or struture type then lets add the callbacks to it so those
			//classes can fire managed events back up to the gui.
			if(m_lpMovable)
			{
				MovableItemCallback *lpCallback = NULL;
				lpCallback = new MovableItemCallback(this);
				m_lpMovable->Callback(lpCallback);
				GetPointers();
			}
		}
	}
	catch(CStdErrorInfo oError)
	{
		string strError = "An error occurred while attempting to set a data value.\nError: " + oError.m_strError;
		String ^strErrorMessage = gcnew String(strError.c_str());
		throw gcnew System::Exception(strErrorMessage);
	}
	catch(System::Exception ^ex)
	{throw ex;}
	catch(...)
	{
		String ^strErrorMessage = "An unknown error occurred while attempting to set a data value.";
		throw gcnew System::Exception(strErrorMessage);
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
		TRACE_DEBUG("Getting Dataobject position pointers.\r\n");

		m_lpPositionX = m_lpBase->GetDataPointer("POSITIONX");
		m_lpPositionY = m_lpBase->GetDataPointer("POSITIONY");
		m_lpPositionZ = m_lpBase->GetDataPointer("POSITIONZ");

		m_lpWorldPositionX = m_lpBase->GetDataPointer("WORLDPOSITIONX");
		m_lpWorldPositionY = m_lpBase->GetDataPointer("WORLDPOSITIONY");
		m_lpWorldPositionZ = m_lpBase->GetDataPointer("WORLDPOSITIONZ");

		m_lpRotationX = m_lpBase->GetDataPointer("ROTATIONX");
		m_lpRotationY = m_lpBase->GetDataPointer("ROTATIONY");
		m_lpRotationZ = m_lpBase->GetDataPointer("ROTATIONZ");

		TRACE_DEBUG("Got Dataobject position pointers.\r\n");
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

				TRACE_DEBUG("Setting data. Object ID: " + m_lpBase->ID() + ", DataType: " + strDataType + ", Value: " + strValue + "\r\n");

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


void DataObjectInterface::QueryProperties(System::Collections::ArrayList ^aryPropertyNames, System::Collections::ArrayList ^aryPropertyTypes)
{
	try
	{
		if(m_lpBase)
		{
			CStdArray<string> aryNames, aryTypes;
			m_lpBase->QueryProperties(aryNames, aryTypes);

			aryPropertyNames->Clear();
			aryPropertyTypes->Clear();

			int iCount = aryNames.GetSize();
			for(int iIdx=0; iIdx<iCount; iIdx++)
			{
				System::String^ sName = gcnew String(aryNames[iIdx].c_str());
				System::String^ sType = gcnew String(aryTypes[iIdx].c_str());

				aryPropertyNames->Add(sName);
				aryPropertyTypes->Add(sType);
			}
		}
	}
	catch(CStdErrorInfo oError)
	{
		string strError = "An error occurred while attempting to QueryProperties.\nError: " + oError.m_strError;
		String ^strErrorMessage = gcnew String(strError.c_str());
		throw gcnew PropertyUpdateException(strErrorMessage);
	}
	catch(System::Exception ^ex)
	{throw ex;}
	catch(...)
	{
		String ^strErrorMessage = "An unknown error occurred while attempting to QueryProperties.";
		throw gcnew System::Exception(strErrorMessage);
	}
}

void DataObjectInterface::SelectItem(bool bVal, bool bSelectMultiple)
{
	try
	{
		if(m_lpBase && m_lpSim->WaitForSimulationBlock())
		{
			TRACE_DEBUG("Selecting Item. Object ID: " + m_lpBase->ID() + ", Val: " + STR(bVal) + ", Select Multiple: " + STR(bSelectMultiple) + "\r\n");

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

			TRACE_DEBUG("Getting Data pointer Item. Object ID: " + m_lpBase->ID() + ", Data: " + strData+ "\r\n");

			float *lpData = m_lpBase->GetDataPointer(strData);

			if(!lpData) 
				throw gcnew System::Exception("The data pointer is not defined!");

			if(!m_aryDataPointers)
				m_aryDataPointers = new CStdMap<string, float *>;

			if(FindDataPointer(strData, FALSE) == NULL)
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

float *DataObjectInterface::FindDataPointer(string strData, BOOL bThrowError)
{
	float *lpData = NULL;

	if(m_aryDataPointers)
	{
		CStdMap<string, float *>::iterator oPos;
	
		TRACE_DEBUG("FindDataPointer. Object ID: " + m_lpBase->ID() + ", Data: " + strData+ "\r\n");

		oPos = m_aryDataPointers->find(Std_CheckString(strData));

		if(oPos != m_aryDataPointers->end())
			lpData =  oPos->second;
		//else if(bThrowError)
		//	THROW_TEXT_ERROR(Al_Err_lDataTypeNotFound, Al_Err_strDataTypeNotFound, " Data Type: " + strData);
	}

	return lpData;
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

		TRACE_DEBUG("GetDataValue. Object ID: " + m_lpBase->ID() + ", Data: " + strData + "\r\n");

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



float DataObjectInterface::GetDataValueImmediate(String ^sData)
{
	if(!m_lpBase) 
		throw gcnew System::Exception("The base object has not been defined!");

	try
	{
		string strData = Util::StringToStd(sData);
		float *lpData = m_lpBase->GetDataPointer(strData);

		if(!lpData) 
			throw gcnew System::Exception("The data pointer is not defined!");

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
	try
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
	catch(CStdErrorInfo oError)
	{
		string strError = "An error occurred while attempting to call GetBoundingBoxValue.\nError: " + oError.m_strError;
		String ^strErrorMessage = gcnew String(strError.c_str());
		throw gcnew PropertyUpdateException(strErrorMessage);
	}
	catch(System::Exception ^ex)
	{throw ex;}
	catch(...)
	{
		String ^strErrorMessage = "An unknown error occurred while attempting to call GetBoundingBoxValue.";
		throw gcnew System::Exception(strErrorMessage);
	}
}

void DataObjectInterface::OrientNewPart(double dblXPos, double dblYPos, double dblZPos, double dblXNorm, double dblYNorm, double dblZNorm)
{
	try
	{
		if(m_lpMovable)
			m_lpMovable->OrientNewPart(dblXPos, dblYPos, dblZPos, dblXNorm, dblYNorm, dblZNorm); 
	}
	catch(CStdErrorInfo oError)
	{
		string strError = "An error occurred while attempting to OrientNewPart.\nError: " + oError.m_strError;
		String ^strErrorMessage = gcnew String(strError.c_str());
		throw gcnew PropertyUpdateException(strErrorMessage);
	}
	catch(System::Exception ^ex)
	{throw ex;}
	catch(...)
	{
		String ^strErrorMessage = "An unknown error occurred while attempting to OrientNewPart.";
		throw gcnew System::Exception(strErrorMessage);
	}
}

System::Boolean DataObjectInterface::CalculateLocalPosForWorldPos(double dblXWorldX, double dblWorldY, double dblWorldZ, System::Collections::ArrayList ^aryLocalPos)
{
	try
	{
		if(m_lpMovable)
		{
			CStdFPoint vPos;
		
			if(m_lpMovable->CalculateLocalPosForWorldPos(dblXWorldX, dblWorldY, dblWorldZ, vPos))
			{
				aryLocalPos->Clear();
				aryLocalPos->Add(vPos.x);
				aryLocalPos->Add(vPos.y);
				aryLocalPos->Add(vPos.z);
				return true;
			}
		}

		return false;
	}
	catch(CStdErrorInfo oError)
	{
		string strError = "An error occurred while attempting to CalculateLocalPosForWorldPos.\nError: " + oError.m_strError;
		String ^strErrorMessage = gcnew String(strError.c_str());
		throw gcnew PropertyUpdateException(strErrorMessage);
	}
	catch(System::Exception ^ex)
	{throw ex;}
	catch(...)
	{
		String ^strErrorMessage = "An unknown error occurred while attempting to CalculateLocalPosForWorldPos.";
		throw gcnew System::Exception(strErrorMessage);
	}
}

void DataObjectInterface::EnableCollisions(String ^sOtherBodyID)
{
	try
	{
		if(m_lpBase) 
		{
			string strOtherBodyID = Util::StringToStd(sOtherBodyID);

			TRACE_DEBUG("EnableCollisions. Body1 ID: " + m_lpBase->ID() + ", Body2: " + strOtherBodyID + "\r\n");

			if(!m_lpRigidBody)
				throw gcnew System::Exception("Base object is not a rigid body.");

			RigidBody *lpOtherBody = dynamic_cast<RigidBody *>(m_lpSim->FindByID(strOtherBodyID));

			if(!lpOtherBody)
				throw gcnew System::Exception("Other object is not a rigid body.");

			m_lpRigidBody->EnableCollision(lpOtherBody);
		}
	}
	catch(CStdErrorInfo oError)
	{
		string strError = "An error occurred while attempting to add a collision exclusion pair.\nError: " + oError.m_strError;
		String ^strErrorMessage = gcnew String(strError.c_str());
		throw gcnew PropertyUpdateException(strErrorMessage);
	}
	catch(System::Exception ^ex)
	{throw ex;}
	catch(...)
	{
		String ^strErrorMessage = "An unknown error occurred while attempting  to add a collision exclusion pair.";
		throw gcnew System::Exception(strErrorMessage);
	}
}

void DataObjectInterface::DisableCollisions(String ^sOtherBodyID)
{
	try
	{
		if(m_lpBase) 
		{
			string strOtherBodyID = Util::StringToStd(sOtherBodyID);

			TRACE_DEBUG("DisableCollisions. Body1 ID: " + m_lpBase->ID() + ", Body2: " + strOtherBodyID + "\r\n");

			if(!m_lpRigidBody)
				throw gcnew System::Exception("Base object is not a rigid body.");

			RigidBody *lpOtherBody = dynamic_cast<RigidBody *>(m_lpSim->FindByID(strOtherBodyID));

			if(!lpOtherBody)
				throw gcnew System::Exception("Other object is not a rigid body.");

			m_lpRigidBody->DisableCollision(lpOtherBody);
		}
	}
	catch(CStdErrorInfo oError)
	{
		string strError = "An error occurred while attempting to remove a collision exclusion pair.\nError: " + oError.m_strError;
		String ^strErrorMessage = gcnew String(strError.c_str());
		throw gcnew PropertyUpdateException(strErrorMessage);
	}
	catch(System::Exception ^ex)
	{throw ex;}
	catch(...)
	{
		String ^strErrorMessage = "An unknown error occurred while attempting to remove a collision exclusion pair.";
		throw gcnew System::Exception(strErrorMessage);
	}
}


	}
}