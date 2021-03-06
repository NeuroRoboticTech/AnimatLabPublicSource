/**
\file	NervousSystem.cpp

\brief	Implements the nervous system class. 
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "Link.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
#include "MovableItem.h"
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
#include "Light.h"
#include "LightManager.h"
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
	m_lpOrganism = NULL;
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
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of NervousSystem\r\n", "", -1, false, true);}
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
\fn	void NervousSystem::AddNeuralModule(std::string strXml)

\brief	Creates and adds a new neural module from an XML definition.

\details This method is used to both create and add a new neural module to the nervous system by
using a XML configuration data packet. This is primarily used by the AddItem method to create a
new module when the user does so in the GUI. 

\author	dcofer
\date	2/24/2011

\param	strXml	The string xml for loading the new module. 
**/

void NervousSystem::AddNeuralModule(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("NeuralModule");

	NeuralModule *lpModule = LoadNeuralModule(oXml);
	lpModule->Initialize();
}

/**
\fn	void NervousSystem::RemoveNeuralModule(std::string strID)

\brief	Removes the neural module based on its ID.

\details This method is primarily used by the RemoveItem method to allow the GUI to remove a
neural module when the user needs to do so. 

\author	dcofer
\date	2/24/2011

\param	strID	Unique GUID ID string of the module to delete. 
**/

void NervousSystem::RemoveNeuralModule(std::string strID)
{
	m_aryNeuralModules.Remove(strID);
}


/**
\fn	NeuralModule *NervousSystem::FindNeuralModule(std::string strModuleName, bool bThrowError)

\brief	Searches for a neural module with a matching module name. 

\details The module name is the name of the DLL to load. This must be unique and match the name of the DLL file.

\author	dcofer
\date	2/24/2011

\param	strModuleName	Name of the DLL module file to find. 
\param	bThrowError		true to throw error, else it just return NULL if not found. 

\return	null if it fails, else the found neural module. 
\exception If bThrowError is true and nothing is found.
**/
NeuralModule *NervousSystem::FindNeuralModule(std::string strModuleName, bool bThrowError)
{
	NeuralModule *lpModule = NULL;
	CStdPtrMap<std::string, NeuralModule>::iterator oPos;
	oPos = m_aryNeuralModules.find(Std_CheckString(strModuleName));

	if(oPos != m_aryNeuralModules.end())
		lpModule =  oPos->second;
	else if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lModuleNameNotFound, Al_Err_strModuleNameNotFound, "ModuleName", strModuleName);

	return lpModule;
}


void NervousSystem::Kill(bool bState)
{
	NeuralModule *lpModule = NULL;
	CStdPtrMap<std::string, NeuralModule>::iterator oPos;
	for(oPos=m_aryNeuralModules.begin(); oPos!=m_aryNeuralModules.end(); ++oPos)
	{
		lpModule = oPos->second;
		lpModule->Kill(bState);
	}
}

void NervousSystem::ResetSimulation()
{
	NeuralModule *lpModule = NULL;
	CStdPtrMap<std::string, NeuralModule>::iterator oPos;
	for(oPos=m_aryNeuralModules.begin(); oPos!=m_aryNeuralModules.end(); ++oPos)
	{
		lpModule = oPos->second;
		lpModule->ResetSimulation();
	}
}

void NervousSystem::SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, bool bVerify)
{
	AnimatBase::SetSystemPointers(lpSim, lpStructure, lpModule, lpNode, false);

	m_lpOrganism = dynamic_cast<Organism *>(lpStructure);

	if(bVerify) VerifySystemPointers();
}

void NervousSystem::VerifySystemPointers()
{
	AnimatBase::VerifySystemPointers();

	if(!m_lpStructure)
		THROW_PARAM_ERROR(Al_Err_lStructureNotDefined, Al_Err_strStructureNotDefined, "Link: ", m_strID);

	if(!m_lpOrganism) 
		THROW_PARAM_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Link: ", m_strID);
}

void NervousSystem::Initialize()
{
	AnimatBase::Initialize();

	NeuralModule *lpModule = NULL;
	CStdPtrMap<std::string, NeuralModule>::iterator oPos;

	//Initialize the neural modules
	for(oPos=m_aryNeuralModules.begin(); oPos!=m_aryNeuralModules.end(); ++oPos)
	{
		lpModule = oPos->second;
		lpModule->Initialize();
	}
}

void NervousSystem::MinTimeStep(float &fltMin)
{
	NeuralModule *lpModule = NULL;
	CStdPtrMap<std::string, NeuralModule>::iterator oPos;
	
	for(oPos=m_aryNeuralModules.begin(); oPos!=m_aryNeuralModules.end(); ++oPos)
	{
		lpModule = oPos->second;

		if(lpModule->TimeStep() < fltMin)
			fltMin = lpModule->TimeStep();
	}
}

void NervousSystem::StepSimulation()
{
	StepSim();
	StepAdapters();
}

void NervousSystem::StepSim()
{
	NeuralModule *lpModule = NULL;
	CStdPtrMap<std::string, NeuralModule>::iterator oPos;
	
	for(oPos=m_aryNeuralModules.begin(); oPos!=m_aryNeuralModules.end(); ++oPos)
	{
		lpModule = oPos->second;

		if(lpModule->NeedToStep(true))
			lpModule->StepSimulation();
	}
}

void NervousSystem::StepAdapters()
{
	NeuralModule *lpModule = NULL;
	CStdPtrMap<std::string, NeuralModule>::iterator oPos;
	
	for(oPos=m_aryNeuralModules.begin(); oPos!=m_aryNeuralModules.end(); ++oPos)
	{
		lpModule = oPos->second;

		if(lpModule->NeedToStep(false))
		{
			lpModule->StepAdapters();
			lpModule->ResetStepCounter();
		}
	}
}

long NervousSystem::CalculateSnapshotByteSize()
{
	NeuralModule *lpModule = NULL;
	CStdPtrMap<std::string, NeuralModule>::iterator oPos;
	long lSize = 0;

	for(oPos=m_aryNeuralModules.begin(); oPos!=m_aryNeuralModules.end(); ++oPos)
	{
		lpModule = oPos->second;

		lSize += lpModule->CalculateSnapshotByteSize();
	}

	return lSize;
}

void NervousSystem::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	NeuralModule *lpModule = NULL;
	CStdPtrMap<std::string, NeuralModule>::iterator oPos;

	for(oPos=m_aryNeuralModules.begin(); oPos!=m_aryNeuralModules.end(); ++oPos)
	{
		lpModule = oPos->second;

		lpModule->SaveKeyFrameSnapshot(aryBytes, lIndex);
	}
}

void NervousSystem::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	NeuralModule *lpModule = NULL;
	CStdPtrMap<std::string, NeuralModule>::iterator oPos;

	for(oPos=m_aryNeuralModules.begin(); oPos!=m_aryNeuralModules.end(); ++oPos)
	{
		lpModule = oPos->second;

		lpModule->LoadKeyFrameSnapshot(aryBytes, lIndex);
	}
}

void NervousSystem::Load(CStdXml &oXml)
{
	m_aryNeuralModules.RemoveAll();

	oXml.IntoChildElement("NeuralModules");  //Into NeuralModules Element

	int iCount = oXml.NumberOfChildren();
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		oXml.FindChildByIndex(iIndex);
		LoadNeuralModule(oXml);		
	}

	oXml.OutOfElem(); //OutOf NeuralModules Element
}

/**
\fn	NeuralModule *NervousSystem::LoadNeuralModule(CStdXml &oXml)

\brief	Creates and loads a neural module.

\details This method uses the module name, module filename and module type to load a neural
module DLL file and get its class factory (IStdClassFactory). It then uses the class factory to
create a new neural module object and then loads it from the xml data. 

\author	dcofer
\date	2/25/2011

\param [in,out]	oXml	The xml data packet that will be loaded. 

\return	null if it fails, else the neural module. 
**/

NeuralModule *NervousSystem::LoadNeuralModule(CStdXml &oXml)
{
	IStdClassFactory *lpFactory = NULL;
	NeuralModule *lpModule = NULL;
	std::string strModuleName, strModuleFileName, strModuleType;

try
{
	oXml.IntoElem(); //Into NeuralModule Element
	strModuleName = oXml.GetChildString("ModuleName", "");
	strModuleFileName = oXml.GetChildString("ModuleFileName", "");
	strModuleType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf NeuralModule Element

	//Lets load the dynamic library and get a pointer to the class factory.
	lpFactory = m_lpSim->LoadClassFactory(strModuleFileName);

	//Now create an instance of a neural module. There is only one type of 
	lpModule = dynamic_cast<NeuralModule *>(lpFactory->CreateObject("NeuralModule", strModuleType, true));
	if(!lpModule)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "NeuralModule");

	lpModule->SetSystemPointers(m_lpSim, m_lpStructure, NULL, NULL, true);

	//Clean up the original class factory. We will use the one in the NeuralModule from now on.
	if(lpFactory)
		{delete lpFactory; lpFactory = NULL;}

	//We must add the neural module factory before it is loaded because the module
	//will need to use the standard Sim->CreateObject method and that requires
	//that the sim have a pointer to this factory.
	m_lpSim->AddNeuralModuleFactory(strModuleName, lpModule);

	lpModule->SetSystemPointers(m_lpSim, m_lpStructure, NULL, NULL, true);
	lpModule->Load(oXml);

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


	}			//Behavior
}			//AnimatSim
