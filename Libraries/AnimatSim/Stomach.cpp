/**
\file	Stomach.cpp

\brief	Implements the stomach class. 
**/

#include "stdafx.h"
#include "IBodyPartCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBody.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Stomach.h"
#include "Structure.h"
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
	namespace Environment
	{
		namespace Bodies
		{
/**
\brief	Default constructor. 

\author	dcofer
\date	3/10/2011
**/
Stomach::Stomach()
{
	m_strID = "STOMACH";
	m_strName = "Stomach";
	m_bKilled = FALSE;
	m_fltMaxEnergyLevel = 100000;
	m_fltEnergyLevel = 2000;
	m_fltAdapterConsumptionRate = 0;
	m_fltBaseConsumptionRate = 1;
	m_fltConsumptionRate = 0;
	m_fltConsumptionForStep = 0;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/10/2011
**/
Stomach::~Stomach()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of Stomach\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Gets the energy level. 

\author	dcofer
\date	3/10/2011

\return	Current energy level. 
**/
float Stomach::EnergyLevel() {return m_fltEnergyLevel;}

/**
\brief	Sets the Energy level. 

\details If the entered value is greater than m_fltMaxEnergyLevel then
the value is defaulted to m_fltMaxEnergyLevel.

\author	dcofer
\date	3/10/2011

\param	fltVal	The new value. 
\exception Energy level cannot be less than zero.
**/
void Stomach::EnergyLevel(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "EnergyLevel", TRUE);

	if(fltVal > m_fltMaxEnergyLevel)
		m_fltEnergyLevel = m_fltMaxEnergyLevel;
	else
		m_fltEnergyLevel = fltVal;
}

/**
\brief	Gets the current consumption rate. 

\author	dcofer
\date	3/10/2011

\return	Consumption rate. 
**/
float Stomach::ConsumptionRate() {return m_fltConsumptionRate;}

/**
\brief	Sets the consumption rate. 

\author	dcofer
\date	3/10/2011

\param	fltVal	The new value. 
\exception Consumption rate cannot be less than zero.
**/
void Stomach::ConsumptionRate(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "ConsumptionRate", TRUE);
	m_fltConsumptionRate = fltVal;
}

/**
\brief	Gets the base consumption rate. 

\author	dcofer
\date	3/10/2011

\return	base consumption rate. 
**/
float Stomach::BaseConsumptionRate() {return m_fltBaseConsumptionRate;}

/**
\brief	Sets the Base consumption rate. 

\author	dcofer
\date	3/10/2011

\param	fltVal	The flt value. 
\exception Base Consumption rate cannot be less than zero.
**/
void Stomach::BaseConsumptionRate(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "BaseConsumptionRate", TRUE);
	m_fltBaseConsumptionRate = fltVal;
}

/**
\brief	Gets the maximum energy level. 

\author	dcofer
\date	3/10/2011

\return	Max energy level. 
**/
float Stomach::MaxEnergyLevel() {return m_fltMaxEnergyLevel;}

/**
\brief	Sets the Maximum energy level. 

\author	dcofer
\date	3/10/2011

\param	fltVal	The flt value. 
\exception MaxEnergyLevel cannot be less than or equal to zero.
**/
void Stomach::MaxEnergyLevel(float fltVal) 
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "MaxEnergyLevel");
	m_fltMaxEnergyLevel = fltVal;
}

/**
\brief	Gets whether to kill the organism if energy level reaches zero. 

\author	dcofer
\date	3/10/2011

\return	true if it should be killed when energy reaches zero. 
**/
BOOL Stomach::KillOrganism() {return m_bKillOrganism;}

/**
\brief	Sets whether or not to kill the organism when the energy level reaches zero. 

\author	dcofer
\date	3/10/2011

\param	bVal	true to value. 
**/
void Stomach::KillOrganism(BOOL bVal) {m_bKillOrganism = bVal;}


// There are no parts or joints to create for muscle attachment points.
void Stomach::CreateParts()
{
}

void Stomach::StepSimulation()
{
	m_fltConsumptionRate = m_fltAdapterConsumptionRate + m_fltBaseConsumptionRate;
	m_fltAdapterConsumptionRate = 0;

	//We have the replenish rate in Quantity/s, but we need it in Quantity/timeslice. Lets recalculate it here.
	m_fltConsumptionForStep = (m_fltConsumptionRate * m_lpSim->PhysicsTimeStep());

	m_fltEnergyLevel -= m_fltConsumptionForStep;

	if(m_fltEnergyLevel < 0)
		m_fltEnergyLevel = 0;

	if(!m_bKilled && m_bKillOrganism && m_fltEnergyLevel == 0)
	{
		Organism *lpOrganism = dynamic_cast<Organism *>(m_lpStructure);
		if(!lpOrganism)
			THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");

		lpOrganism->Kill(TRUE);
		m_bKilled = TRUE;
	}
}

float *Stomach::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);
	float *lpData = NULL;

	if(strType == "ENERGYLEVEL")
		return &m_fltEnergyLevel;

	if(strType == "CONSUMPTIONRATE")
		return &m_fltConsumptionRate;

	if(strType == "CONSUMPTIONFORSTEP")
		return &m_fltConsumptionForStep;

	if(strType == "ADAPTERCONSUMPTIONRATE")
		return &m_fltAdapterConsumptionRate;

	lpData = RigidBody::GetDataPointer(strDataType);
	if(lpData) return lpData;

	THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "RigidBodyID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

void Stomach::AddExternalNodeInput(float fltInput)
{
	m_fltAdapterConsumptionRate += fltInput;
	if(m_fltAdapterConsumptionRate < 0)
		m_fltAdapterConsumptionRate = 0;
}

void Stomach::Load(CStdXml &oXml)
{
	oXml.IntoElem();  //Into RigidBody Element

	//There can only be one stomach per organism and its ID is hardcoded.
	ID("STOMACH");
	Name("Stomach");
	Type(oXml.GetChildString("Type", m_strType));

	MaxEnergyLevel(oXml.GetChildFloat("MaxEnergyLevel", m_fltMaxEnergyLevel));
	EnergyLevel(oXml.GetChildFloat("EnergyLevel", m_fltEnergyLevel));
	BaseConsumptionRate(oXml.GetChildFloat("BaseConsumptionRate", m_fltBaseConsumptionRate));
	KillOrganism(oXml.GetChildBool("KillOrganism", TRUE));

	oXml.OutOfElem(); //OutOf RigidBody Element

	//This will add this object to the object list of the simulation.
	m_lpSim->AddToObjectList(this);
}

		}		//Joints
	}			//Environment
}				//AnimatSim
