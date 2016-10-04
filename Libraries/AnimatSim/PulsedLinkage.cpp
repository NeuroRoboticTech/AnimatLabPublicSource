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
	m_bMatchOnChange = true;
	m_iPrevValue = 0;
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

void PulsedLinkage::MatchValue(int iVal) {m_iMatchValue = iVal;}

int PulsedLinkage::MatchValue() {return m_iMatchValue;}

void PulsedLinkage::MatchOnChange(bool bVal) {m_bMatchOnChange = bVal;}

bool PulsedLinkage::MatchOnChange() {return m_bMatchOnChange;}

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
		MatchValue(atoi(strValue.c_str()));
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
	else if(strType == "MATCHONCHANGE")
	{
		MatchOnChange(Std_ToBool(strValue));
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
	aryProperties.Add(new TypeProperty("MatchOnChange", AnimatPropertyType::Boolean, AnimatPropertyDirection::Set));

	aryProperties.Add(new TypeProperty("Matches", AnimatPropertyType::Integer, AnimatPropertyDirection::Get));
}

#pragma endregion

void PulsedLinkage::ResetSimulation()
{
	RemoteControlLinkage::ResetSimulation();
	m_iMatches = 0; 
	m_aryPulses.clear();
}

float PulsedLinkage::CalculateAppliedValue(float fltData)
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

			////Test Code
			//OutputDebugString("Pulse Culled\r\n");
		}
		else
			bDone = true;
	}
}

void  PulsedLinkage::IncrementMatches()
{
	m_AccessMatchesMutex.lock();
	m_iMatches++;
	m_AccessMatchesMutex.unlock();

	////Test Code
	//std::string strVal = "Matches: " + STR((int) m_iMatches) + "\r\n";
	//OutputDebugString(strVal.c_str());
}

void PulsedLinkage::StepIO()
{
	if(m_bEnabled && !m_lpSim->Paused())
	{
		int iSource = (int) (*m_lpSourceData + (Std_Sign(*m_lpSourceData) * 0.5));

		if(m_bMatchOnChange && iSource != m_iPrevValue && iSource == m_iMatchValue)
			IncrementMatches();
		else if(!m_bMatchOnChange && iSource == m_iMatchValue)
			IncrementMatches();

		m_iPrevValue = iSource;

		// Reset the source data to an invalid value once it is used so it can be set correctly 
		// again later. Otherwise, if you only send a periodic signal then it will not work 
		// correctly.
		*m_lpSourceData = -1000000;
	}
}

void PulsedLinkage::StepSimulation()
{
	if(m_bEnabled && m_lpSourceData && m_lpTarget && m_lpTargetData)
	{
		//Set the reporting value
		m_fltMatchesReport = m_iMatches;

		//If we have a match then add a new pulse
		if(m_AccessMatchesMutex.try_lock())
		{

			if(m_iMatches > 0)
			{
				//Limit to a total of 100 pulses active at any one time
				if(m_aryPulses.size() < 100)
				{	
					int iMatches = m_iMatches;
					if(iMatches+m_aryPulses.size() > 100)
						iMatches = 100 -m_aryPulses.size();

					for(int iIdx=0; iIdx<iMatches; iIdx++)
						m_aryPulses.Add(m_fltPulseDuration);
				}

				//Reset matches for next time.
				m_iMatches = 0;
			}

			m_AccessMatchesMutex.unlock();
		}

		ApplyValue(*m_lpSourceData);

		if(m_aryPulses.size() > 0)
			CullPulses();
	}
}

void PulsedLinkage::Load(CStdXml &oXml)
{
	RemoteControlLinkage::Load(oXml);

	oXml.IntoElem();  //Into Link Element

	MatchValue(oXml.GetChildInt("MatchValue", m_iMatchValue));
	MatchOnChange(oXml.GetChildInt("MatchOnChange", m_bMatchOnChange));
	PulseDuration(oXml.GetChildFloat("PulseDuration", m_fltPulseDuration));
	PulseCurrent(oXml.GetChildFloat("PulseCurrent", m_fltPulseCurrent));

	oXml.OutOfElem(); //OutOf Link Element
}



	}
}
