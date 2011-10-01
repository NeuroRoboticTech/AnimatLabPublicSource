/**
\file	PhysicsNeuralModule.cpp

\brief	Implements the firing rate module class.
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Gain.h"
#include "Adapter.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Structure.h"
#include "NeuralModule.h"
#include "PhysicalNeuralModule.h"
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

namespace AnimatSim
{
	namespace Behavior
	{

/**
\brief	Default constructor.

\author	dcofer
\date	3/29/2011
**/
PhysicsNeuralModule::PhysicsNeuralModule()
{
}

/**
\brief	Destructor.

\author	dcofer
\date	3/29/2011
**/
PhysicsNeuralModule::~PhysicsNeuralModule()
{

try
{
	m_aryAdapters.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of PhysicsNeuralModule\r\n", "", -1, FALSE, TRUE);}
}


void PhysicsNeuralModule::Kill(BOOL bState)
{
	int iCount = m_aryAdapters.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryAdapters[iIndex])
			m_aryAdapters[iIndex]->Kill(bState);
}

void PhysicsNeuralModule::ResetSimulation()
{
	int iCount = m_aryAdapters.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryAdapters[iIndex])
			m_aryAdapters[iIndex]->ResetSimulation();
}

void PhysicsNeuralModule::Initialize()
{
	NeuralModule::Initialize();

	Organism *lpOrganism = dynamic_cast<Organism *>(m_lpStructure);
	if(!lpOrganism) 
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");

	int iCount = m_aryAdapters.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryAdapters[iIndex])
			m_aryAdapters[iIndex]->Initialize();
}

#pragma region DataAccesMethods

BOOL PhysicsNeuralModule::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(NeuralModule::SetData(strDataType, strValue, FALSE))
		return TRUE;

	if(strType == "TIMESTEP")
	{
		TimeStep(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void PhysicsNeuralModule::AddAdapter(string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Adapter");

	Adapter *lpAdapter = LoadAdapter(oXml);
	lpAdapter->Initialize();
}

void PhysicsNeuralModule::RemoveAdapter(string strID)
{
	int iIdx = FindAdapterListPos(strID);
	m_aryAdapters.RemoveAt(iIdx);
}

int PhysicsNeuralModule::FindAdapterListPos(string strID, BOOL bThrowError)
{
	string sID = Std_ToUpper(Std_Trim(strID));

	int iCount = m_aryAdapters.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryAdapters[iIndex]->ID() == sID)
			return iIndex;

	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lAdapterIDNotFound, Al_Err_strAdapterIDNotFound, "ID", m_strID);

	return -1;
}

BOOL PhysicsNeuralModule::AddItem(string strItemType, string strXml, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "ADAPTER")
	{
		try
		{
			AddAdapter(strXml);
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

BOOL PhysicsNeuralModule::RemoveItem(string strItemType, string strID, BOOL bThrowError)
{
	string strType = Std_CheckString(strItemType);

	if(strType == "ADAPTER")
	{
		RemoveAdapter(strID);
		return TRUE;
	}
	//Synapses are stored in the destination neuron. They will be removed there.


	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidItemType, Al_Err_strInvalidItemType, "Item Type", strItemType);

	return FALSE;
}

#pragma endregion

long PhysicsNeuralModule::CalculateSnapshotByteSize()
{
	long lByteSize = 0;
	int iCount = m_aryAdapters.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryAdapters[iIndex])
			lByteSize += m_aryAdapters[iIndex]->CalculateSnapshotByteSize();

	return lByteSize;
}

void PhysicsNeuralModule::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	int iCount = m_aryAdapters.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryAdapters[iIndex])
			m_aryAdapters[iIndex]->SaveKeyFrameSnapshot(aryBytes, lIndex);
}

void PhysicsNeuralModule::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	int iCount = m_aryAdapters.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		if(m_aryAdapters[iIndex])
			m_aryAdapters[iIndex]->LoadKeyFrameSnapshot(aryBytes, lIndex);
}

void PhysicsNeuralModule::Load(CStdXml &oXml)
{
	VerifySystemPointers();

	CStdXml oNetXml;

	oXml.IntoElem();  //Into NeuralModule Element

	ID(oXml.GetChildString("ID", m_strID));
	Type(oXml.GetChildString("Type", m_strType));
	Name(oXml.GetChildString("Name", m_strName));

	//We do NOT call the TimeStep mutator here because we need to call it only after all modules are loaded so we can calculate the min time step correctly.
	m_fltTimeStep = oXml.GetChildFloat("TimeStep", m_fltTimeStep);
	
	//This will add this object to the object list of the simulation.
	m_lpSim->AddToObjectList(this);

	m_arySourceAdapters.RemoveAll();
	m_aryTargetAdapters.RemoveAll();

	if(oXml.FindChildElement("Adapters", FALSE))
	{
		oXml.IntoElem(); //Into Adapters Element
		int iCount = oXml.NumberOfChildren();

		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadAdapter(oXml);		
		}
		oXml.OutOfElem(); //OutOf Adapters Element
	}

	oXml.OutOfElem(); //OutOf NeuralModule Element

	TRACE_DEBUG("Finished loading nervous system config file.");
}

/**
\fn	Adapter *NervousSystem::LoadAdapter(CStdXml &oXml)

\brief	Creates and loads an adapter.

\details This method uses the module name and type specified in the xml packet to create a new
adapter object using the simulator::CreateObject method. It then loads the adapter using the xml
data. 

\author	dcofer
\date	2/25/2011

\param [in,out]	oXml	The xml data packet to load. 

\return	null if it fails, else the adapter. 
**/

Adapter *PhysicsNeuralModule::LoadAdapter(CStdXml &oXml)
{
	Adapter *lpAdapter = NULL;
	string strModuleName, strType;

try
{
	oXml.IntoElem(); //Into Child Element
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Child Element

	lpAdapter = dynamic_cast<Adapter *>(m_lpSim->CreateObject(strModuleName, "Adapter", strType));
	if(!lpAdapter)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Adapter");

	lpAdapter->SetSystemPointers(m_lpSim, m_lpStructure, NULL, NULL, TRUE);
	lpAdapter->Load(oXml);

	m_aryAdapters.Add(lpAdapter);

	return lpAdapter;
}
catch(CStdErrorInfo oError)
{
	if(lpAdapter) delete lpAdapter;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpAdapter) delete lpAdapter;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

	}				//Behavior
}				//AnimatSim
