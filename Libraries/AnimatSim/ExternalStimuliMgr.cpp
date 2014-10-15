/**
\file	ExternalStimuliMgr.cpp

\brief	Implements the external stimuli manager class. 
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Gain.h"
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
#include "ExternalStimulus.h"
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
	namespace ExternalStimuli
	{
/**
\brief	Default constructor. 

\author	dcofer
\date	3/17/2011
**/
ExternalStimuliMgr::ExternalStimuliMgr()
{

}

/**
\brief	Destructor. 

\author	dcofer
\date	3/17/2011
**/
ExternalStimuliMgr::~ExternalStimuliMgr()
{

}

/**
\brief	Creates a new stimulus from an xml definition and adds it to the manager.

\details This is primarily used by the GUI to add new stimuli.

\author	dcofer
\date	3/17/2011

\param	strXml	The xml data for loading in the new stimuli. 

\return	true if it succeeds, false if it fails. 
**/
bool ExternalStimuliMgr::AddStimulus(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Stimulus");
	ExternalStimulus *lpStim = LoadExternalStimuli(oXml);
	lpStim->Initialize();

	return true;
}

/**
\brief	Removes the stimulus described by strID. 

\author	dcofer
\date	3/17/2011

\param	strID	GUID ID for the stimulus to remove. 

\return	true if it succeeds, false if it fails. 
**/
bool ExternalStimuliMgr::RemoveStimulus(std::string strID)
{
	Remove(strID);
	return true;
}
/*
void ExternalStimuliMgr::Load(std::string strProjectPath, std::string strFileName)
{
	CStdXml oXml;

	TRACE_DEBUG("Loading external stimuli config file.\r\n" + strProjectPath + "\r\nFileName: " + strFileName);

	oXml.Load(AnimatSim::GetFilePath(strProjectPath, strFileName));

	oXml.FindElement("StimuliConfiguration");
	oXml.FindChildElement("");

	Load(oXml);

	TRACE_DEBUG("Finished loading external stimuli config file.");
}
*/

void ExternalStimuliMgr::Load(CStdXml &oXml)
{
	VerifySystemPointers();

	Reset();

	if(oXml.FindChildElement("ExternalStimuli", false))
	{
		oXml.IntoElem(); //Into ExternalStimuli Element

		int iCount = oXml.NumberOfChildren();
		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadExternalStimuli(oXml);
		}

		oXml.OutOfElem(); //OutOf ExternalStimuli Element
	}
}

/**
\brief	Loads an external stimuli. 

\author	dcofer
\date	3/17/2011

\param [in,out]	oXml	The stimulus xml to load. 

\return	null if it fails, else the external stimuli. 
\exception Exception is thrown if there is a problem loading the stimulus.
**/
ExternalStimulus *ExternalStimuliMgr::LoadExternalStimuli(CStdXml &oXml)
{
	ExternalStimulus *lpStimulus = NULL;
	std::string strModuleName, strType, strFilename;

try
{
	oXml.IntoElem(); //Into Stimulus Element
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Stimulus Element

	lpStimulus = dynamic_cast<ExternalStimulus *>(m_lpSim->CreateObject(strModuleName, "ExternalStimulus", strType));
	if(!lpStimulus)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "ExternalStimulus");

	lpStimulus->SetSystemPointers(m_lpSim, NULL, NULL, NULL, true);
	lpStimulus->Load(oXml);

	Add(lpStimulus);
	return lpStimulus;
}
catch(CStdErrorInfo oError)
{
	if(lpStimulus) delete lpStimulus;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpStimulus) delete lpStimulus;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

	}			//ExternalStimuli
}				//AnimatSim

