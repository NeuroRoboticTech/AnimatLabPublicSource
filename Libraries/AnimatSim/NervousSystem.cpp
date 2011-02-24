/**
\file	NervousSystem.cpp

\brief	Implements the nervous system class. 
**/

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "Adapter.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Structure.h"
#include "NeuralModule.h"
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
#include "Simulator.h"

namespace AnimatSim
{
	namespace Behavior
	{
/**
\fn	NervousSystem::NervousSystem()

\brief	Default constructor. 

\author	dcofer
\date	2/24/2011
**/
NervousSystem::NervousSystem()
{
}

/**
\fn	NervousSystem::~NervousSystem()

\brief	Destructor. 

\author	dcofer
\date	2/24/2011
**/
NervousSystem::~NervousSystem()
{

try
{
	m_aryNeuralModules.RemoveAll();
	m_aryAdapters.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of NervousSystem\r\n", "", -1, FALSE, TRUE);}
}


/**
\fn	void NervousSystem::AddNeuralModule(NeuralModule *lpModule)

\brief	Adds a neural module to the nervous system. 

\author	dcofer
\date	2/24/2011

\param [in,out]	lpModule	If non-null, the pointer to a module. 
\exception If module is null, or duplicate module name.
**/
void NervousSystem::AddNeuralModule(NeuralModule *lpModule)
{
	if(!lpModule)
		THROW_ERROR(Al_Err_lNeuralModuleNotDefined, Al_Err_strNeuralModuleNotDefined);

	try
	{
			m_aryNeuralModules.Add(Std_CheckString(lpModule->ModuleName()), lpModule);
	}
	catch(CStdErrorInfo oError)
	{
		oError.m_strError += " Duplicate Neural Module Key: " + lpModule->ModuleName(); 
		RELAY_ERROR(oError);
	}
}

/**
\fn	void NervousSystem::AddNeuralModule(Simulator *lpSim, Structure *lpStructure, string strXml)

\brief	Creates and adds a new neural module from an XML definition. 

\details This method is used to both create and add a new neural module to the nervous system by using
a XML configuration data packet. This is primarily used by the AddItem method to create a new module when
the user does so in the GUI.

\author	dcofer
\date	2/24/2011

\param [in,out]	lpSim		The pointer to a simulation. 
\param [in,out]	lpStructure	The pointer to a structure. 
\param	strXml				The string xml for loading the new module. 
**/
void NervousSystem::AddNeuralModule(Simulator *lpSim, Structure *lpStructure, string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("NeuralModule");

	NeuralModule *lpModule = LoadNeuralModule(lpSim, lpStructure, oXml);
	lpModule->Initialize(lpSim, lpStructure);
}

/**
\fn	void NervousSystem::RemoveNeuralModule(Simulator *lpSim, Structure *lpStructure, string strID)

\brief	Removes the neural module based on its ID. 

\details This method is primarily used by the RemoveItem method to allow the GUI to remove
a neural module when the user needs to do so.

\author	dcofer
\date	2/24/2011

\param [in,out]	lpSim		The pointer to a simulation. 
\param [in,out]	lpStructure	The pointer to a structure. 
\param	strID				Unique GUID ID string of the module to delete. 
**/
void NervousSystem::RemoveNeuralModule(Simulator *lpSim, Structure *lpStructure, string strID)
{
	m_aryNeuralModules.Remove(strID);
}


/**
\fn	NeuralModule *NervousSystem::FindNeuralModule(string strModuleName, BOOL bThrowError)

\brief	Searches for a neural module with a matching module name. 

\details The module name is the name of the DLL to load. This must be unique and match the name of the DLL file.

\author	dcofer
\date	2/24/2011

\param	strModuleName	Name of the DLL module file to find. 
\param	bThrowError		true to throw error, else it just return NULL if not found. 

\return	null if it fails, else the found neural module. 
\exception If bThrowError is true and nothing is found.
**/
NeuralModule *NervousSystem::FindNeuralModule(string strModuleName, BOOL bThrowError)
{
	NeuralModule *lpModule = NULL;
	CStdPtrMap<string, NeuralModule>::iterator oPos;
	oPos = m_aryNeuralModules.find(Std_CheckString(strModuleName));

	if(oPos != m_aryNeuralModules.end())
		lpModule =  oPos->second;
	else if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lModuleNameNotFound, Al_Err_strModuleNameNotFound, "ModuleName", strModuleName);

	return lpModule;
}


/**
\fn	void NervousSystem::Kill(Simulator *lpSim, Organism *lpOrganism, BOOL bState)

\brief	Calls Kill method on all sub-items. 

\details When an organism is killed then all neural elements are disabled to prevent 
any further network activity. This method goes through and calls the Kill method of 
each neural module.

\author	dcofer
\date	2/24/2011

\param [in,out]	lpSim		The pointer to a simulation. 
\param [in,out]	lpOrganism	The pointer to an organism. 
\param	bState				true to state. 
**/
void NervousSystem::Kill(Simulator *lpSim, Organism *lpOrganism, BOOL bState)
{
	NeuralModule *lpModule = NULL;
	CStdPtrMap<string, NeuralModule>::iterator oPos;
	for(oPos=m_aryNeuralModules.begin(); oPos!=m_aryNeuralModules.end(); ++oPos)
	{
		lpModule = oPos->second;
		lpModule->Kill(lpSim, lpOrganism, bState);
	}
}


/**
\fn	void NervousSystem::ResetSimulation(Simulator *lpSim, Organism *lpOrganism)

\brief	Resets the simulation to time 0. 

\details When the simulation is reset it defaults the entire system back to time 0.
This method calls the ResetSimulation method of each neural module, which in turn
resets all data within the neural code back to its initial state on simulation start.

\author	dcofer
\date	2/24/2011

\param [in,out]	lpSim		The pointer to a simulation. 
\param [in,out]	lpOrganism	The pointer to an organism. 
**/
void NervousSystem::ResetSimulation(Simulator *lpSim, Organism *lpOrganism)
{
	NeuralModule *lpModule = NULL;
	CStdPtrMap<string, NeuralModule>::iterator oPos;
	for(oPos=m_aryNeuralModules.begin(); oPos!=m_aryNeuralModules.end(); ++oPos)
	{
		lpModule = oPos->second;
		lpModule->ResetSimulation(lpSim, lpOrganism);
	}
}

/**
\fn	void NervousSystem::Initialize(Simulator *lpSim, Structure *lpStructure)

\brief	Initializes this object. 

\details This initializes all neural modules and all of the adapters for this nervous system.

\author	dcofer
\date	2/24/2011

\param [in,out]	lpSim		The pointer to a simulation. 
\param [in,out]	lpStructure	The pointer to a structure. 
**/
void NervousSystem::Initialize(Simulator *lpSim, Structure *lpStructure)
{
	NeuralModule *lpModule = NULL;
	CStdPtrMap<string, NeuralModule>::iterator oPos;

	//Initialize the neural modules
	for(oPos=m_aryNeuralModules.begin(); oPos!=m_aryNeuralModules.end(); ++oPos)
	{
		lpModule = oPos->second;
		lpModule->Initialize(lpSim, lpStructure);
	}

	//Now initialize the adapters
	int iCount = m_aryAdapters.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryAdapters[iIndex]->Initialize(lpSim, lpStructure);

}

/**
\fn	void NervousSystem::StepSimulation(Simulator *lpSim, Structure *lpStructure)

\brief	Steps simulation for the nervous system. 

\details Each NeuralModule can have a different integration time step. This method loops
through each of the modules and calls the NeedToStep method to determine if that modules
StepSimulation method should be called. Remeber that all time steps are based on the core
integer TimeStep. All of the other modules time steps are normalized to the module with the 
smallest time step. For instance, lets the firing rate time step is 0.5 ms, and the integrate
and fire time step was 0.1 ms. The base time step will be 0.1 ms, and the TimeStepInterval of the
integrate and fire module will be 1, while the TimeStepInterval of the firing rate module will be
5. So every 5th step the call to the firing rate module method NeedToStep will return true and 
it will be stepped. 

\author	dcofer
\date	2/24/2011

\param [in,out]	lpSim		The pointer to a simulation. 
\param [in,out]	lpStructure	The pointer to a structure. 
**/
void NervousSystem::StepSimulation(Simulator *lpSim, Structure *lpStructure)
{
	NeuralModule *lpModule = NULL;
	CStdPtrMap<string, NeuralModule>::iterator oPos;
	
	for(oPos=m_aryNeuralModules.begin(); oPos!=m_aryNeuralModules.end(); ++oPos)
	{
		lpModule = oPos->second;

		if(lpModule->NeedToStep())
			lpModule->StepSimulation(lpSim, lpStructure);
	}
}

/**
\fn	long NervousSystem::CalculateSnapshotByteSize()

\brief	Calculates the snapshot byte size. 

\details Sometimes the user may want to capture a snapshot of the simulation at a given point in time,
and then be able to go back to that specific point. To do this we grab a snapshot of all the data in the system,
including the neural variables. We essentially serialize the data into a binary format for later re-use. This method
calculates the number of bytes that will be required to store the entire nervous system.

\author	dcofer
\date	2/24/2011

\return	The calculated snapshot byte size. 
**/
long NervousSystem::CalculateSnapshotByteSize()
{
	NeuralModule *lpModule = NULL;
	CStdPtrMap<string, NeuralModule>::iterator oPos;
	long lSize = 0;

	for(oPos=m_aryNeuralModules.begin(); oPos!=m_aryNeuralModules.end(); ++oPos)
	{
		lpModule = oPos->second;

		lSize += lpModule->CalculateSnapshotByteSize();
	}

	return lSize;
}

/**
\fn	void NervousSystem::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)

\brief	Saves a key frame snapshot. 

\details Sometimes the user may want to capture a snapshot of the simulation at a given point in time,
and then be able to go back to that specific point. To do this we grab a snapshot of all the data in the system,
including the neural variables. We essentially serialize the data into a binary format for later re-use. This method
goes through each module and saves its data into the byte array.

\author	dcofer
\date	2/24/2011

\param [in,out]	aryBytes	The array of bytes where the data is being stored. 
\param [in,out]	lIndex		Current zero-based index of the write position in the array. 
**/
void NervousSystem::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	NeuralModule *lpModule = NULL;
	CStdPtrMap<string, NeuralModule>::iterator oPos;

	for(oPos=m_aryNeuralModules.begin(); oPos!=m_aryNeuralModules.end(); ++oPos)
	{
		lpModule = oPos->second;

		lpModule->SaveKeyFrameSnapshot(aryBytes, lIndex);
	}
}

/**
\fn	void NervousSystem::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)

\brief	Loads a key frame snapshot. 

\details Sometimes the user may want to capture a snapshot of the simulation at a given point in time,
and then be able to go back to that specific point. To do this we grab a snapshot of all the data in the system,
including the neural variables. We essentially serialize the data into a binary format for later re-use. This method
goes through each module and loads its data from the byte array.

\author	dcofer
\date	2/24/2011

\param [in,out]	aryBytes	The array of bytes where the data is being stored. 
\param [in,out]	lIndex		Current zero-based index of the read position in the array. 
**/
void NervousSystem::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	NeuralModule *lpModule = NULL;
	CStdPtrMap<string, NeuralModule>::iterator oPos;

	for(oPos=m_aryNeuralModules.begin(); oPos!=m_aryNeuralModules.end(); ++oPos)
	{
		lpModule = oPos->second;

		lpModule->LoadKeyFrameSnapshot(aryBytes, lIndex);
	}
}

void NervousSystem::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	if(!lpSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);
		
	m_aryNeuralModules.RemoveAll();

	oXml.IntoChildElement("NeuralModules");  //Into NeuralModules Element

	int iCount = oXml.NumberOfChildren();
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		oXml.FindChildByIndex(iIndex);
		LoadNeuralModule(lpSim, lpStructure, oXml);		
	}

	oXml.OutOfElem(); //OutOf NeuralModules Element

	if(oXml.FindChildElement("Adapters", FALSE))
	{
		oXml.IntoElem(); //Into Adapters Element
		int iCount = oXml.NumberOfChildren();

		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadAdapter(lpSim, lpStructure, oXml);		
		}
		oXml.OutOfElem(); //OutOf Adapters Element
	}

}


NeuralModule *NervousSystem::LoadNeuralModule(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	IStdClassFactory *lpFactory = NULL;
	NeuralModule *lpModule = NULL;
	string strModuleName, strModuleFileName, strModuleType;

try
{
	oXml.IntoElem(); //Into NeuralModule Element
	strModuleName = oXml.GetChildString("ModuleName", "");
	strModuleFileName = oXml.GetChildString("ModuleFileName", "");
	strModuleType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf NeuralModule Element

	//Lets load the dynamic library and get a pointer to the class factory.
	lpFactory = lpSim->LoadClassFactory(strModuleFileName);

	//Now create an instance of a neural module. There is only one type of 
	lpModule = dynamic_cast<NeuralModule *>(lpFactory->CreateObject("NeuralModule", strModuleType, TRUE));
	if(!lpModule)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "NeuralModule");

	lpModule->SetSystemPointers(lpSim, lpStructure);

	//Clean up the original class factory. We will use the one in the NeuralModule from now on.
	if(lpFactory)
		{delete lpFactory; lpFactory = NULL;}

	//We must add the neural module factory before it is loaded because the module
	//will need to use the standard Sim->CreateObject method and that requires
	//that the sim have a pointer to this factory.
	lpSim->AddNeuralModuleFactory(strModuleName, lpModule);

	lpModule->Load(lpSim, lpStructure, oXml);

	AddNeuralModule(lpModule);

	return lpModule;
}
catch(CStdErrorInfo oError)
{
	if(lpFactory) delete lpFactory;
	if(lpModule) delete lpModule;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpFactory) delete lpFactory;
	if(lpModule) delete lpModule;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


Adapter *NervousSystem::LoadAdapter(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	Adapter *lpAdapter = NULL;
	string strModuleName, strType;

try
{
	oXml.IntoElem(); //Into Child Element
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Child Element

	lpAdapter = dynamic_cast<Adapter *>(lpSim->CreateObject(strModuleName, "Adapter", strType));
	if(!lpAdapter)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Adapter");

	lpAdapter->Load(lpSim, lpStructure, oXml);

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


	}			//Behavior
}			//AnimatSim
