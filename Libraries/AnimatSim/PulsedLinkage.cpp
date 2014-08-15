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
	m_iMatches = 0;
	m_fltMatchesReport = 0;
	m_aryPulses.clear();
}

PulsedLinkage::~PulsedLinkage(void)
{
try
{
	m_aryPulses.clear();
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

float *PulsedLinkage::GetDataPointer(const std::string &strDataType)
{
	std::string strType = Std_CheckString(strDataType);

	if(strType == "MATCHES")
		return &m_fltMatchesReport;

	return AnimatSim::Robotics::RemoteControlLinkage::GetDataPointer(strDataType);
}

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

	aryProperties.Add(new TypeProperty("Matches", AnimatPropertyType::Integer, AnimatPropertyDirection::Get));
}

#pragma endregion

void PulsedLinkage::ResetSimulation()
{
	m_iMatches = 0; 
	m_aryPulses.clear();
}

float PulsedLinkage::CalculateAppliedCurrent()
{
	float fltTotal = 0;
	int iCount = m_aryPulses.size();
	for(int iIdx=0; iIdx<iCount; iIdx++)
	{
		fltTotal += m_fltPulseCurrent;
		m_aryPulses[iIdx] -= m_lpSim->PhysicsTimeStep(); 
	}

	return fltTotal;
}

void PulsedLinkage::CullPulses()
{
	bool bDone = false;
	while(!bDone)
	{
		int iCount = m_aryPulses.size();
		int iDeleteID = -1;
		for(int iIdx=0; iIdx<iCount && iDeleteID == -1; iIdx++)
			if(m_aryPulses[iIdx] <= 0)
				iDeleteID = iIdx;

		if(iDeleteID > -1)
		{
			//If we found one to delete then get rid of it.
			m_aryPulses.RemoveAt(iDeleteID);
		}
		else
			bDone = true;
	}
}

void PulsedLinkage::StepIO()
{
	if(m_bEnabled && !m_lpSim->Paused())
	{
		unsigned int iSource = (unsigned int) *m_lpSourceData;
		if(iSource == m_iMatchValue)
			m_iMatches++;
	}
}

void PulsedLinkage::StepSimulation()
{
	if(m_bEnabled && m_lpSourceData && m_lpTargetNode && m_iTargetDataType != -1)
	{
		//Set the reporting value
		m_fltMatchesReport = m_iMatches;

		//If we have a match then add a new pulse
		if(m_iMatches > 0)
		{
			for(int iIdx=0; iIdx<m_iMatches; iIdx++)
				m_aryPulses.Add(m_fltPulseDuration);

			//Reset matches for next time.
			m_iMatches = 0;
		}

		//Now loop through the pulses list and add up the total current to apply
		m_fltAppliedCurrent = CalculateAppliedCurrent();

		m_lpTargetNode->AddExternalNodeInput(m_iTargetDataType, m_fltAppliedCurrent);

		if(m_aryPulses.size() > 0)
			CullPulses();
	}
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
