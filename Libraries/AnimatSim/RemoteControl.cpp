#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "IMotorizedJoint.h"
#include "AnimatBase.h"

#include "Node.h"
#include "Link.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Structure.h"
#include "NeuralModule.h"
#include "Adapter.h"
#include "NervousSystem.h"
#include "Organism.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataChartMgr.h"
#include "ExternalStimuliMgr.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Light.h"
#include "LightManager.h"
#include "Simulator.h"

#include "RobotInterface.h"
#include "RobotIOControl.h"
#include "RemoteControlLinkage.h"
#include "RemoteControl.h"

namespace AnimatSim
{
	namespace Robotics
	{

RemoteControl::RemoteControl(void)
{
	m_iChangeSimStepCount = 5;
	m_bUseRemoteDataTypes = true;
}

RemoteControl::~RemoteControl(void)
{
try
{
	m_aryLinks.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of RemoteControl\r\n", "", -1, false, true);}
}

/**
\brief	Gets the array ofremote control links.

\author	dcofer
\date	3/2/2011

\return	pointer to remote control links.
**/
CStdPtrArray<RemoteControlLinkage> *RemoteControl::Links() {return &m_aryLinks;}

/**
\brief	Gets the array of remote control in links.

\author	dcofer
\date	3/2/2011

\return	pointer to remote control in links.
**/
CStdArray<RemoteControlLinkage *> *RemoteControl::InLinks() {return &m_aryInLinks;}

/**
\brief	Gets the array of remote control out links.

\author	dcofer
\date	3/2/2011

\return	pointer to remote control out links.
**/
CStdArray<RemoteControlLinkage *> *RemoteControl::OutLinks() {return &m_aryOutLinks;}

/**
\brief	Gets the array of data elements.

\author	dcofer
\date	5/1/2015

\return	pointer to data elements.
**/
CStdMap<int, RemoteControlLinkage *> *RemoteControl::Data() {return &m_aryData;}

void RemoteControl::ChangeSimStepCount(int iRate)
{
	Std_IsAboveMin((int) 0, iRate, true, "ChangeSimStepCount");
	m_iChangeSimStepCount = iRate;

	//Reset all the button data.

	int iCount = m_aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryLinks[iIndex]->m_Data.m_iChangeSimStepCount = m_iChangeSimStepCount;
}

int RemoteControl::ChangeSimStepCount() {return m_iChangeSimStepCount;}

bool RemoteControl::UseRemoteDataTypes() {return m_bUseRemoteDataTypes;}

#pragma region DataAccesMethods

bool RemoteControl::AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError, bool bDoNotInit)
{
	std::string strType = Std_CheckString(strItemType);

	if(strType == "REMOTECONTROLLINKAGE")
	{
		AddRemoteControlLinkage(strXml);
		return true;
	}

	return RobotIOControl::AddItem(strItemType, strXml, bThrowError, bDoNotInit);
}

bool RemoteControl::RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError)
{
	std::string strType = Std_CheckString(strItemType);

	if(strType == "REMOTECONTROLLINKAGE")
	{
		RemoveRemoteControlLinkage(strID);
		return true;
	}

	return RobotIOControl::RemoveItem(strItemType, strID, bThrowError);
}
#pragma region DataAccesMethods

float *RemoteControl::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	int iCount = m_aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(strType == Std_CheckString(m_aryLinks[iIndex]->m_Data.m_strProperty))
			return &m_aryLinks[iIndex]->m_Data.m_fltValue;
		
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(strType == Std_CheckString(m_aryLinks[iIndex]->m_Data.m_strProperty + "START"))
			return &m_aryLinks[iIndex]->m_Data.m_fltStart;

	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(strType == Std_CheckString(m_aryLinks[iIndex]->m_Data.m_strProperty + "STOP"))
			return &m_aryLinks[iIndex]->m_Data.m_fltStop;

	return RobotIOControl::GetDataPointer(strDataType);
}

bool RemoteControl::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);
	
	if(RobotIOControl::SetData(strDataType, strValue, false))
		return true;

	if(strType == "CHANGESIMSTEPCOUNT")
	{
		ChangeSimStepCount((int) atoi(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RemoteControl::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RobotIOControl::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("ChangeSimStepCount", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));

	int iCount = m_aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		aryProperties.Add(new TypeProperty(m_aryLinks[iIndex]->m_Data.m_strProperty, AnimatPropertyType::Float, AnimatPropertyDirection::Get));
		aryProperties.Add(new TypeProperty(m_aryLinks[iIndex]->m_Data.m_strProperty + "START", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
		aryProperties.Add(new TypeProperty(m_aryLinks[iIndex]->m_Data.m_strProperty + "STOP", AnimatPropertyType::Float, AnimatPropertyDirection::Get));
	}
}

#pragma endregion

/**
\brief	Creates and adds a robot IO control.

\author	dcofer
\date	3/2/2011

\param	strXml	The xml data packet for loading the control node.
**/
RemoteControlLinkage *RemoteControl::AddRemoteControlLinkage(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Link");

	RemoteControlLinkage *lpPart = LoadRemoteControlLinkage(oXml);

	lpPart->Initialize();

	CreateDataTypes();

    return lpPart;
}

/**
\brief	Removes the rigid body with the specified ID.

\author	dcofer
\date	3/2/2011

\param	strID	ID of the body to remove
\param	bThrowError	If true and ID is not found then it will throw an error.
\exception If bThrowError is true and ID is not found.
**/
void RemoteControl::RemoveRemoteControlLinkage(std::string strID, bool bThrowError)
{
	int iPos = FindLinkageChildListPos(strID, bThrowError), iLinkPos = -1;

    RemoteControlLinkage *lpPart = m_aryLinks[iPos];

	//Remove it from the data list.
	if(m_aryData.find(lpPart->PropertyID()) != m_aryData.end())
		m_aryData.Remove(lpPart->PropertyID());

	if(lpPart->InLink())
		iLinkPos = FindLinkageChildListPos(m_aryInLinks, strID, bThrowError);
	else
		iLinkPos = FindLinkageChildListPos(m_aryOutLinks, strID, bThrowError);

	StartPause();
	if(lpPart->InLink())
		m_aryInLinks.RemoveAt(iLinkPos);
	else
		m_aryOutLinks.RemoveAt(iLinkPos);

	m_aryLinks.RemoveAt(iPos);
	ExitPause();
}


/**
\brief	Finds the array index for the child part with the specified ID

\author	dcofer
\date	3/2/2011

\param	strID ID of part to find
\param	bThrowError	If true and ID is not found then it will throw an error, else return NULL
\exception If bThrowError is true and ID is not found.

\return	If bThrowError is false and ID is not found returns NULL,
else returns the pointer to the found part.
**/
int RemoteControl::FindLinkageChildListPos(std::string strID, bool bThrowError)
{
	std::string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryLinks[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lPartInterfaceIDNotFound, Al_Err_strPartInterfaceIDNotFound, "ID", strID);

	return -1;
}

/**
\brief	Finds the array index for the child part with the specified ID

\author	dcofer
\date	3/2/2011

\param	strID ID of part to find
\param	bThrowError	If true and ID is not found then it will throw an error, else return NULL
\exception If bThrowError is true and ID is not found.

\return	If bThrowError is false and ID is not found returns NULL,
else returns the pointer to the found part.
**/
int RemoteControl::FindLinkageChildListPos(CStdArray<RemoteControlLinkage *> &aryLinks, std::string strID, bool bThrowError)
{
	std::string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(aryLinks[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lPartInterfaceIDNotFound, Al_Err_strPartInterfaceIDNotFound, "ID", strID);

	return -1;
}

RemoteControlLinkage *RemoteControl::FindLinkageWithPropertyName(std::string strName, bool bThrowError)
{
	int iCount = m_aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryLinks[iIndex]->PropertyName() == strName)
			return m_aryLinks[iIndex];

	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lPartInterfaceIDNotFound, Al_Err_strPartInterfaceIDNotFound, "PropertyName", strName);

	return NULL;
}

#pragma endregion

void RemoteControl::ResetData()
{
	int iCount = m_aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryLinks[iIndex]->m_Data.ClearData();
}

void RemoteControl::CheckStartedStopped()
{
	int iCount = m_aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryLinks[iIndex]->m_Data.CheckStartedStopped();

	////Test Code
	//m_ButtonData[BUT_ID_RT].CheckStartedStopped();
}

void RemoteControl::ClearStartStops()
{

	int iCount = m_aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryLinks[iIndex]->m_Data.ClearStartStops();

	////Test Code
	//m_ButtonData[BUT_ID_LOOKH].ClearStartStops();
}

/**
\brief	This method is called after all connections to whatever control board have been made. It calls
each parts SetupIO method. For example, We connect to a Firmata
microcontroller like an Arduino, and then do a setup that could take some time. We should not attempt to
setup any of the pins until after the board itself has been setup. After that we need to loop through and setup
all the parts.

\author	dcofer
\date	5/1/2014

**/
void RemoteControl::SetupIO()
{
	RobotIOControl::SetupIO();

	if(m_bEnabled)
	{
		int iCount = m_aryLinks.GetSize();
		for(int iIndex=0; iIndex<iCount; iIndex++)
			if(m_aryLinks[iIndex]->Enabled())
				m_aryLinks[iIndex]->SetupIO();
	}
}


/**
\brief	This method is called from within the IO thread. It calls StepIO for each part.

\author	dcofer
\date	5/2/2014

**/
void RemoteControl::StepIO()
{
	if(m_bEnabled)
	{
		//unsigned long long lStepStartTick = m_lpSim->GetTimerTick();
		RobotIOControl::StepIO();

		int iCount = m_aryLinks.GetSize();
		for(int iIndex=0; iIndex<iCount; iIndex++)
			if(m_aryLinks[iIndex]->Enabled())
				m_aryLinks[iIndex]->StepIO();

		//unsigned long long lEndStartTick = m_lpSim->GetTimerTick();
		//m_fltStepIODuration = m_lpSim->TimerDiff_m(lStepStartTick, lEndStartTick);
	}
}

/**
\brief	This method is called just before the IO thread is closed down. It gives the IO objects a chance to do
any required cleanup.

\author	dcofer
\date	5/12/2014

**/
void RemoteControl::ShutdownIO()
{
	int iCount = m_aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryLinks[iIndex]->Enabled())
			m_aryLinks[iIndex]->ShutdownIO();

	//Put this last so everything gets its ShutdownIO called before we actually exit the thread.
	RobotIOControl::ShutdownIO();
}

void RemoteControl::CreateDataTypes()
{
	if(m_bUseRemoteDataTypes && m_aryDataIDMap.GetSize() == 0)
		CreateDataIDMap();

	m_aryData.RemoveAll();
	int iCount = m_aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		if(!m_bUseRemoteDataTypes)
		{
			if(m_aryLinks[iIndex]->PropertyID() >= 0 && m_aryData.find(m_aryLinks[iIndex]->PropertyID()) == m_aryData.end())
			{
				m_aryLinks[iIndex]->m_Data.m_iChangeSimStepCount = m_iChangeSimStepCount;
				m_aryLinks[iIndex]->m_Data.ClearData();
				m_aryData.Add(m_aryLinks[iIndex]->PropertyID(), m_aryLinks[iIndex]);
			}

		}
		else
		{
			std::string strProperty = m_aryLinks[iIndex]->PropertyName();
			if(m_aryDataIDMap.find(strProperty) != m_aryDataIDMap.end())
			{
				int iID = m_aryDataIDMap[strProperty];

				if(m_aryData.find(iID) == m_aryData.end())
				{
					m_aryLinks[iIndex]->m_Data.m_iChangeSimStepCount = m_iChangeSimStepCount;
					m_aryLinks[iIndex]->m_Data.ClearData();
					m_aryLinks[iIndex]->PropertyID(iID, false);
					m_aryData.Add(iID, m_aryLinks[iIndex]);
				}
			}
		}
	}
}

void RemoteControl::CreateDataIDMap()
{
}

void RemoteControl::SetDataValue(int iID, float fltVal)
{
	if(m_aryData.find(iID) != m_aryData.end())
		if(m_aryData[iID]->InLink())
			m_aryData[iID]->m_Data.m_fltValue = fltVal;
}

void RemoteControl::Initialize()
{
	RobotIOControl::Initialize();

	int iCount = m_aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryLinks[iIndex]->Initialize();

	//If this remote control is not pre-defined with data types in the derived class
	//then we need to create them now from the remote linkages.
	if(!m_bUseRemoteDataTypes)
		CreateDataTypes();

	ResetData();
}

void RemoteControl::ResetSimulation()
{
	RobotIOControl::ResetSimulation();

	m_iCyclePartIdx = 0;
	int iCount = m_aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryLinks[iIndex]->ResetSimulation();

	ResetData();
}

void RemoteControl::AfterResetSimulation()
{
	RobotIOControl::AfterResetSimulation();

	int iCount = m_aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryLinks[iIndex]->AfterResetSimulation();
}

void RemoteControl::StepSimulation()
{
    RobotIOControl::StepSimulation();

	int iCount = m_aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryLinks[iIndex]->Enabled())
			m_aryLinks[iIndex]->StepSimulation();

	ClearStartStops();
}

void RemoteControl::Load(CStdXml &oXml)
{
	RobotIOControl::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	ChangeSimStepCount(oXml.GetChildInt("ChangeSimStepCount", m_iChangeSimStepCount));

	if(oXml.FindChildElement("Links", false))
	{
		oXml.IntoElem();  //Into ChildBodies Element
		int iChildCount = oXml.NumberOfChildren();

		for(int iIndex=0; iIndex<iChildCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadRemoteControlLinkage(oXml);
		}
		oXml.OutOfElem(); //OutOf ChildBodies Element
	}

	oXml.OutOfElem(); //OutOf RigidBody Element
}

/**
\brief	Loads a child IO Control.

\author	dcofer
\date	3/2/2011

\param [in,out]	oXml	The xml data definition of the part to load.

\return	null if it fails, else the IO control.
**/

RemoteControlLinkage *RemoteControl::LoadRemoteControlLinkage(CStdXml &oXml)
{
	RemoteControlLinkage *lpChild = NULL;
	std::string strType;

try
{
	oXml.IntoElem(); //Into Child Element
	std::string strModule = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Child Element

	lpChild = dynamic_cast<RemoteControlLinkage *>(m_lpSim->CreateObject(strModule, "RemoteControlLinkage", strType));
	if(!lpChild)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "RemoteControlLinkage");

	lpChild->ParentRemoteControl(this);
	lpChild->SetSystemPointers(m_lpSim, m_lpStructure, NULL, NULL, true);

	lpChild->Load(oXml);

	m_aryLinks.Add(lpChild);

	if(lpChild->InLink())
		m_aryInLinks.Add(lpChild);
	else
		m_aryOutLinks.Add(lpChild);

	return lpChild;
}
catch(CStdErrorInfo oError)
{
	if(lpChild) delete lpChild;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpChild) delete lpChild;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


	}
}
