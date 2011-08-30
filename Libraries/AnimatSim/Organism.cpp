/**
\file	Organism.cpp

\brief	Implements the organism class. 
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Gain.h"
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
#include "Sensor.h"
#include "Attachment.h"
#include "Structure.h"
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

/**
\brief	Default constructor. 

\author	dcofer
\date	2/25/2011
**/
Organism::Organism()
{
	m_bDead = FALSE;
	m_lpNervousSystem = NULL;
}

/**
\brief	Destructor. 

\author	dcofer
\date	2/25/2011
**/
Organism::~Organism()
{

try
{
	if(m_lpNervousSystem) 
		{
			delete m_lpNervousSystem; 
			m_lpNervousSystem = NULL;
	}
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Organism\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Query if this object is dead. 

\author	dcofer
\date	2/25/2011

\return	true if dead, false if not. 
**/
BOOL Organism::IsDead() 
{return m_bDead;}

void Organism::Kill(BOOL bState)
{
	m_bDead = bState;
	m_lpBody->Kill(bState);
	m_lpNervousSystem->Kill(bState);
}

void Organism::Initialize()
{
	Structure::Initialize();

	m_lpNervousSystem->Initialize();
}

void Organism::ResetSimulation()
{
	if(m_lpBody)
	{
		m_lpBody->ResetSimulation();
		
		UpdateData();
	}

	m_lpNervousSystem->ResetSimulation();

	//We have to call this after method because some objects (ie: muscles and spindles, etc.) depend on other items
	//already being reset to their original positions. So they must be done first and then these items get reset.
	if(m_lpBody)
		m_lpBody->AfterResetSimulation();
}

/**
\fn	void Organism::StepNeuralEngine()

\brief	Step neural engine. 

\author	dcofer
\date	3/2/2011
**/
void Organism::StepNeuralEngine()
{
	if(!m_bDead)
		m_lpNervousSystem->StepSimulation();
}

#pragma region DataAccesMethods

BOOL Organism::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(Structure::SetData(strDataType, strValue, FALSE))
		return TRUE;

	//if(strType == "TIMESTEP")
	//{
	//	TimeStep(atof(strValue.c_str()));
	//	return TRUE;
	//}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

BOOL Organism::AddItem(string strItemType, string strXml, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(Structure::AddItem(strItemType, strXml, FALSE))
		return TRUE;

	if(strType == "NEURALMODULE")
	{
		try
		{
			m_lpNervousSystem->AddNeuralModule(strXml);
			return TRUE;
		}
		catch(CStdErrorInfo oError)
		{
			if(bThrowError)
				RELAY_ERROR(oError);
		}
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

BOOL Organism::RemoveItem(string strItemType, string strID, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);
	
	if(Structure::RemoveItem(strItemType, strID, FALSE))
		return TRUE;

	if(strType == "NEURALMODULE")
	{
		try
		{
			m_lpNervousSystem->RemoveNeuralModule(strID);
			return TRUE;
		}
		catch(CStdErrorInfo oError)
		{
			if(bThrowError)
				RELAY_ERROR(oError);
		}
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

#pragma endregion

/**
\fn	AnimatSim::Behavior::NervousSystem *Organism::NervousSystem()

\brief	returns a pointer to the nervous system

\author	dcofer
\date	3/2/2011

\return	Pointer to the nervous system. 
**/
AnimatSim::Behavior::NervousSystem *Organism::NervousSystem()
{return m_lpNervousSystem;}

long Organism::CalculateSnapshotByteSize()
{return m_lpNervousSystem->CalculateSnapshotByteSize();}

void Organism::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{m_lpNervousSystem->SaveKeyFrameSnapshot(aryBytes, lIndex);}

void Organism::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{m_lpNervousSystem->LoadKeyFrameSnapshot(aryBytes, lIndex);}

void Organism::Load(CStdXml &oXml)
{
	Structure::Load(oXml);

	oXml.IntoElem();  //Into Layout Element

	//dwc convert. Need to have a method to remove a nervous system. It needs to remove any added
	//modules from the list in the simulator.
	if(m_lpNervousSystem) {delete m_lpNervousSystem; m_lpNervousSystem = NULL;}
	m_lpNervousSystem = new AnimatSim::Behavior::NervousSystem;

	oXml.IntoChildElement("NervousSystem");

	m_lpNervousSystem->SetSystemPointers(m_lpSim, this, NULL, NULL, TRUE);
	m_lpNervousSystem->Load(oXml);

	oXml.OutOfElem();

	oXml.OutOfElem(); //OutOf Layout Element
}
