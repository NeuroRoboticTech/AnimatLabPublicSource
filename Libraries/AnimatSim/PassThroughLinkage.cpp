#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "IMotorizedJoint.h"
#include "AnimatBase.h"

#include "Node.h"
#include "Link.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "Gain.h"
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
#include "PassThroughLinkage.h"
#include "RemoteControl.h"

namespace AnimatSim
{
	namespace Robotics
	{

PassThroughLinkage::PassThroughLinkage(void)
{
	m_lpGain = NULL;
}

PassThroughLinkage::~PassThroughLinkage(void)
{
try
{
	if(m_lpGain)
	{
		delete m_lpGain;
		m_lpGain = NULL;
	}
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of PassThroughLinkage\r\n", "", -1, false, true);}
}

/**
\brief	Gets the poitner to the gain function.

\author	dcofer
\date	3/18/2011

\return	Pointer to the gain.
**/
Gain *PassThroughLinkage::GetGain() {return m_lpGain;}

void PassThroughLinkage::SetGain(Gain *lpGain)
{
	if(m_lpGain)
	{
		delete m_lpGain;
		m_lpGain = NULL;
	}

	m_lpGain = lpGain;
	m_lpGain->SetSystemPointers(m_lpSim, m_lpStructure, m_lpModule, NULL, true);
}

/**
\brief	Creates and adds a gain object. 

\author	dcofer
\date	3/2/2011

\param	strXml	The xml data packet for loading the gain. 
**/
void PassThroughLinkage::AddGain(std::string strXml)
{
	CStdXml oXml;
	oXml.Deserialize(strXml);
	oXml.FindElement("Root");
	oXml.FindChildElement("Gain");

	SetGain(LoadGain(m_lpSim, "Gain", oXml));
}


bool PassThroughLinkage::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(RemoteControlLinkage::SetData(strType, strValue, false))
		return true;

	if(strType == "GAIN")
	{
		AddGain(strValue);
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void PassThroughLinkage::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RemoteControlLinkage::QueryProperties(aryProperties);
	aryProperties.Add(new TypeProperty("Gain", AnimatPropertyType::Xml, AnimatPropertyDirection::Set));
}

#pragma endregion

float PassThroughLinkage::CalculateAppliedValue(float fltData)
{
	if(m_lpGain)
		return m_lpGain->CalculateGain(fltData);
	else
		return 0;
}

void PassThroughLinkage::Load(CStdXml &oXml)
{
	RemoteControlLinkage::Load(oXml);

	oXml.IntoElem();  //Into Link Element
	if(oXml.FindChildElement("Gain", false))
		SetGain(LoadGain(m_lpSim, "Gain", oXml));

	oXml.OutOfElem(); //OutOf Link Element
}



	}
}
