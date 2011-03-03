// ExternalStimuliMgr.cpp: implementation of the ExternalStimuliMgr class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "AnimatBase.h"


#include "stdafx.h"
#include "IBodyPartCallback.h"


#include "AnimatBase.h"

#include "Gain.h"
#include "Node.h"
#include "IPhysicsBody.h"
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
#include "Simulator.h"

namespace AnimatSim
{
	namespace ExternalStimuli
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

ExternalStimuliMgr::ExternalStimuliMgr()
{

}

ExternalStimuliMgr::~ExternalStimuliMgr()
{

}
 
BOOL ExternalStimuliMgr::AddStimulus(Simulator *lpSim, string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Stimulus");
	ExternalStimulus *lpStim = LoadExternalStimuli(oXml);
	lpStim->Initialize(lpSim);

	return TRUE;
}

BOOL ExternalStimuliMgr::RemoveStimulus(Simulator *lpSim, string strID)
{
	Remove(lpSim, strID);
	return TRUE;
}

void ExternalStimuliMgr::Load(string strProjectPath, string strFileName)
{
	CStdXml oXml;

	TRACE_DEBUG("Loading external stimuli config file.\r\n" + strProjectPath + "\r\nFileName: " + strFileName);

	oXml.Load(AnimatSim::GetFilePath(strProjectPath, strFileName));

	oXml.FindElement("StimuliConfiguration");
	oXml.FindChildElement("");

	Load(oXml);

	TRACE_DEBUG("Finished loading external stimuli config file.");
}


void ExternalStimuliMgr::Load(CStdXml &oXml)
{
	TRACE_DEBUG("Loading external stimuli from Xml.");

	Reset();

	if(oXml.FindChildElement("ExternalStimuli", FALSE))
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


ExternalStimulus *ExternalStimuliMgr::LoadExternalStimuli(CStdXml &oXml)
{
	ExternalStimulus *lpStimulus = NULL;
	string strModuleName, strType, strFilename;

try
{
	oXml.IntoElem(); //Into Stimulus Element
	strModuleName = oXml.GetChildString("ModuleName", "");
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Stimulus Element

	lpStimulus = dynamic_cast<ExternalStimulus *>(m_lpSim->CreateObject(strModuleName, "ExternalStimulus", strType));
	if(!lpStimulus)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "ExternalStimulus");

	lpStimulus->SetSystemPointers(m_lpSim, NULL, NULL, NULL);
	lpStimulus->Load(oXml);

	Add(m_lpSim, lpStimulus);
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

