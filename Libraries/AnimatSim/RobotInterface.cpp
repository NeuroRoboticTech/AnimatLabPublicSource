/**
\file	RobotInterface.cpp

\brief	Base class for the robotics interface for AnimatLab.
**/

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
#include "RobotPartInterface.h"


namespace AnimatSim
{
	namespace Robotics
	{

/**
\brief	Default constructor.

\author	dcofer
\date	9/8/2014
**/
RobotInterface::RobotInterface(void)
{
	m_bSynchSim = true;
	m_fltPhysicsTimeStep = 0.02f;
}

/**
\brief	Destructor.

\author	dcofer
\date	9/8/2014
**/
RobotInterface::~RobotInterface(void)
{

try
{
	m_aryIOControls.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of RobotInterface\r\n", "", -1, false, true);}
}

/**
\brief	Gets the array of IO controls. 

\author	dcofer
\date	3/2/2011

\return	pointer to array of IO controls. 
**/
CStdPtrArray<RobotIOControl> *RobotInterface::IOControls() {return &m_aryIOControls;}

/**
\brief	Gets the physics time step used within the robot framwork. 

\author	dcofer
\date	4/25/2014

\return	Gets physics time step.
**/
float RobotInterface::PhysicsTimeStep() {return m_fltPhysicsTimeStep;}

/**
\brief	Sets the physics time step used within the robot framwork. 

\author	dcofer
\date	4/25/2014

\param	bVal	Sets physics time step.
**/
void RobotInterface::PhysicsTimeStep(float fltStep)
{
	Std_IsAboveMin((float) 0, fltStep, true, "PhysicsTimeStep", false);
	m_fltPhysicsTimeStep = fltStep;
}

/**
\brief	Gets whether we need to delay stepping of the physics adapters in the simulation to more closely match the real robot behavior. 

\author	dcofer
\date	5/13/2014

\return	Gets whether we will attempt to synch sim update to robots update.
**/
bool RobotInterface::SynchSim() {return m_bSynchSim;}

/**
\brief	Sets whether we need to delay stepping of the physics adapters in the simulation to more closely match the real robot behavior. 

\author	dcofer
\date	5/13/2014

\param	bVal	new value.
**/
void RobotInterface::SynchSim(bool bVal)
{
	if(m_lpSim->InSimulation())
	{
		m_bSynchSim = bVal;

		if(m_lpSim)
			m_lpSim->RobotAdpaterSynch(bVal);
	}
	else
		m_lpSim->RobotAdpaterSynch(false);
}

#pragma region DataAccesMethods

float *RobotInterface::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	//if(strType == "LIMITPOS")
	//	return &m_fltLimitPos;
	//else
		THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Robot Interface ID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

bool RobotInterface::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "ENABLED")
	{
		Enabled(Std_ToBool(strValue));
		return true;
	}

	if(strType == "PHYSICSTIMESTEP")
	{
		PhysicsTimeStep((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "SYNCHSIM")
	{
		SynchSim(Std_ToBool(strValue));
		return true;
	}

	if(AnimatBase::SetData(strType, strValue, false))
		return true;

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void RobotInterface::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	AnimatBase::QueryProperties(aryProperties);

}

bool RobotInterface::AddItem(const std::string &strItemType, const std::string &strXml, bool bThrowError, bool bDoNotInit)
{
	std::string strType = Std_CheckString(strItemType);

	if(strType == "ROBOTIOCONTROL")
	{
		AddIOControl(strXml);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

bool RobotInterface::RemoveItem(const std::string &strItemType, const std::string &strID, bool bThrowError)
{
	std::string strType = Std_CheckString(strItemType);

	if(strType == "ROBOTIOCONTROL")
	{
		RemoveIOControl(strID);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return false;
}

/**
\brief	Creates and adds a robot IO control. 

\author	dcofer
\date	3/2/2011

\param	strXml	The xml data packet for loading the control node. 
**/
RobotIOControl *RobotInterface::AddIOControl(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("RobotIOControl");

	RobotIOControl *lpControl = LoadIOControl(oXml);

	lpControl->Initialize();

    return lpControl;
}

/**
\brief	Removes the rigid body with the specified ID. 

\author	dcofer
\date	3/2/2011

\param	strID	ID of the body to remove
\param	bThrowError	If true and ID is not found then it will throw an error.
\exception If bThrowError is true and ID is not found.
**/
void RobotInterface::RemoveIOControl(std::string strID, bool bThrowError)
{
	int iPos = FindChildListPos(strID, bThrowError);

    RobotIOControl *lpControl = m_aryIOControls[iPos];

	if(lpControl)
		lpControl->ShutdownIO();

	m_aryIOControls.RemoveAt(iPos);
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
int RobotInterface::FindChildListPos(std::string strID, bool bThrowError)
{
	std::string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_aryIOControls.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryIOControls[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lIOControlIDNotFound, Al_Err_strIOControlIDNotFound, "ID", strID);

	return -1;
}

#pragma endregion

void RobotInterface::Initialize()
{
	AnimatBase::Initialize();

	if(m_bEnabled)
	{
		int iCount = m_aryIOControls.GetSize();
		for(int iIndex=0; iIndex<iCount; iIndex++)
			m_aryIOControls[iIndex]->Initialize();
	}
}

void RobotInterface::ResetSimulation()
{
	AnimatBase::ResetSimulation();

	if(m_bEnabled)
	{
		int iCount = m_aryIOControls.GetSize();
		for(int iIndex=0; iIndex<iCount; iIndex++)
			m_aryIOControls[iIndex]->ResetSimulation();
	}
}

void RobotInterface::AfterResetSimulation()
{
	AnimatBase::AfterResetSimulation();

	if(m_bEnabled)
	{
		int iCount = m_aryIOControls.GetSize();
		for(int iIndex=0; iIndex<iCount; iIndex++)
			m_aryIOControls[iIndex]->AfterResetSimulation();
	}
}

void RobotInterface::StepSimulation()
{
	//If we are running in simulation mode then do not step the interfaces.
	if(!m_bEnabled)
		return;

    AnimatBase::StepSimulation();

	int iCount = m_aryIOControls.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryIOControls[iIndex]->Enabled())
			m_aryIOControls[iIndex]->StepSimulation();
}

void RobotInterface::Load(CStdXml &oXml)
{
	AnimatBase::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	PhysicsTimeStep(oXml.GetChildFloat("PhysicsTimeStep", m_fltPhysicsTimeStep));
	SynchSim(oXml.GetChildBool("SynchSim", m_bSynchSim));

	if(oXml.FindChildElement("IOControls", false))
	{
		oXml.IntoElem();  //Into ChildBodies Element
		int iChildCount = oXml.NumberOfChildren();

		for(int iIndex=0; iIndex<iChildCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadIOControl(oXml);
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

RobotIOControl *RobotInterface::LoadIOControl(CStdXml &oXml)
{
	RobotIOControl *lpChild = NULL;
	std::string strType;

try
{
	oXml.IntoElem(); //Into Child Element
	std::string strModule = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Child Element

	lpChild = dynamic_cast<RobotIOControl *>(m_lpSim->CreateObject(strModule, "RobotIOControl", strType));
	if(!lpChild)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "RobotIOControl");
	
	lpChild->ParentInterface(this);
	lpChild->SetSystemPointers(m_lpSim, m_lpStructure, NULL, NULL, true);

	lpChild->Load(oXml);

	m_aryIOControls.Add(lpChild);

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