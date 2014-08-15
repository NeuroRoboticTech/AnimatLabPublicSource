#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "IMotorizedJoint.h"
#include "AnimatBase.h"

#include "Node.h"
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
	int iPos = FindLinkageChildListPos(strID, bThrowError);

    RemoteControlLinkage *lpPart = m_aryLinks[iPos];

	StartPause();
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

#pragma endregion

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
		if(m_bPauseIO || m_lpSim->Paused())
			WaitWhilePaused();

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
	RobotIOControl::ShutdownIO();

	int iCount = m_aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryLinks[iIndex]->Enabled())
			m_aryLinks[iIndex]->ShutdownIO();
}

void RemoteControl::Initialize()
{
	RobotIOControl::Initialize();

	int iCount = m_aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryLinks[iIndex]->Initialize();
}

void RemoteControl::ResetSimulation()
{
	RobotIOControl::ResetSimulation();

	m_iCyclePartIdx = 0;
	int iCount = m_aryLinks.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryLinks[iIndex]->ResetSimulation();
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
}

void RemoteControl::Load(CStdXml &oXml)
{
	RobotIOControl::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

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
