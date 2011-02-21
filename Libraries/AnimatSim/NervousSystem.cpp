// NervousSystem.cpp: implementation of the NervousSystem class.
//
//////////////////////////////////////////////////////////////////////

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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
namespace AnimatSim
{
	namespace Behavior
	{

/*! \brief 
   Constructs an structure object..
   		
	 \return
	 No return value.

   \remarks
	 The constructor for a structure. 
*/

NervousSystem::NervousSystem()
{
}


/*! \brief 
   Destroys the structure object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the structure object..	 
*/

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

string NervousSystem::ProjectPath()
{return m_strProjectPath;}

void NervousSystem::ProjectPath(string strPath)
{m_strProjectPath = strPath;}

string NervousSystem::NeuralNetworkFile()
{return m_strNeuralNetworkFile;}

void NervousSystem::NeuralNetworkFile(string strFile)
{m_strNeuralNetworkFile = strFile;}

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

void NervousSystem::AddNeuralModule(Simulator *lpSim, Structure *lpStructure, string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("NeuralModule");

	NeuralModule *lpModule = LoadNeuralModule(lpSim, lpStructure, oXml);
	lpModule->Initialize(lpSim, lpStructure);
}

void NervousSystem::RemoveNeuralModule(Simulator *lpSim, Structure *lpStructure, string strID)
{
	m_aryNeuralModules.Remove(strID);
}

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

void NervousSystem::Load(Simulator *lpSim, Structure *lpStructure, string strProjectPath, string strNeuralFile)
{
	CStdXml oXml;

	TRACE_DEBUG("Loading nervous system config file.\r\nProjectPath: " + m_strProjectPath + "\r\nFile: " + strNeuralFile);

	if(Std_IsBlank(strProjectPath)) 
		THROW_ERROR(Al_Err_lProjectPathBlank, Al_Err_strProjectPathBlank);

	if(Std_IsBlank(strNeuralFile)) 
		THROW_ERROR(Al_Err_lNeuralNetworkBlank, Al_Err_strNeuralNetworkBlank);

	m_strProjectPath = strProjectPath;
	m_strNeuralNetworkFile = strNeuralFile;

	oXml.Load(AnimatSim::GetFilePath(strProjectPath, strNeuralFile));

	oXml.FindElement("NervousSystem");
	oXml.FindChildElement("NeuralModules");

	Load(lpSim, lpStructure, oXml);

	TRACE_DEBUG("Finished loading nervous system config file.");
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

	lpModule->ProjectPath(m_strProjectPath);
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
