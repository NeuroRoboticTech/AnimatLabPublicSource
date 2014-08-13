#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "IMotorizedJoint.h"
#include "AnimatBase.h"

#include "Node.h"
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
#include "PulsedLinkage.h"
#include "RemoteControl.h"

namespace AnimatSim
{
	namespace Robotics
	{

PulsedLinkage::PulsedLinkage(void)
{
	m_iMatchValue = 0;
	m_fltPulseDuration = 0;
	m_fltPulseCurrent = 0;
}

PulsedLinkage::~PulsedLinkage(void)
{
try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of PulsedLinkage\r\n", "", -1, false, true);}
}

void PulsedLinkage::MatchValue(unsigned int iVal) {m_iMatchValue = iVal;}

unsigned int PulsedLinkage::MatchValue() {return m_iMatchValue;}

void PulsedLinkage::PulseDuration(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "PulseDuration");
	m_fltPulseDuration = fltVal;
}

float PulsedLinkage::PulseDuration() {return m_fltPulseDuration;}

void PulsedLinkage::PulseCurrent(float fltVal)
{
	Std_IsAboveMin((float) 0, fltVal, true, "PulseCurrent");
	m_fltPulseCurrent = fltVal;
}

float PulsedLinkage::PulseCurrent() {return m_fltPulseCurrent;}

bool PulsedLinkage::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(RemoteControlLinkage::SetData(strType, strValue, false))
		return true;

	if(strType == "MATCHVALUE")
	{
		MatchValue((unsigned int) atoi(strValue.c_str()));
		return true;
	}
	else if(strType == "PULSEDURATION")
	{
		PulseDuration(atof(strValue.c_str()));
		return true;
	}
	else if(strType == "PULSECURRENT")
	{
		PulseCurrent(atof(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void PulsedLinkage::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RemoteControlLinkage::QueryProperties(aryProperties);
	aryProperties.Add(new TypeProperty("MatchValue", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("PulseDuration", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("PulseCurrent", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
}

#pragma endregion

void PulsedLinkage::StepSimulation()
{
	//if(m_bEnabled && m_lpSourceData && m_lpTargetNode && m_iTargetDataType != -1 && m_lpGain)
	//{
	//	float fltOutput = m_lpGain->CalculateGain(*m_lpSourceData);
	//	m_lpTargetNode->AddExternalNodeInput(m_iTargetDataType, fltOutput);
	//}
}

void PulsedLinkage::Load(CStdXml &oXml)
{
	RemoteControlLinkage::Load(oXml);

	oXml.IntoElem();  //Into Link Element

	MatchValue(oXml.GetChildInt("MatchValue", m_iMatchValue));
	PulseDuration(oXml.GetChildFloat("PulseDuration", m_fltPulseDuration));
	PulseCurrent(oXml.GetChildFloat("PulseCurrent", m_fltPulseCurrent));

	oXml.OutOfElem(); //OutOf Link Element
}



	}
}
