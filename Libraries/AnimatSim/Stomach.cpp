// Stomach.cpp: implementation of the Stomach class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "IBodyPartCallback.h"
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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

/*! \brief 
   Constructs a Stomach.
   		
   \param lpParent This is a pointer to the parent rigid body of this joint. 
   \param lpChild This is a pointer to the child rigid body of this joint. 

	 \return
	 No return value.

   \remarks
	 The constructor for a Stomach joint. 
*/

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


/*! \brief 
   Destroys the Stomach joint object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the Stomach joint object..	 
*/

Stomach::~Stomach()
{
	try
	{
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of Stomach\r\n", "", -1, FALSE, TRUE);}
}

void Stomach::EnergyLevel(float fltVal) 
{
	if(fltVal > m_fltMaxEnergyLevel)
		m_fltEnergyLevel = m_fltMaxEnergyLevel;
	else
		m_fltEnergyLevel = fltVal;
}

// There are no parts or joints to create for muscle attachment points.
void Stomach::CreateParts(Simulator *lpSim, Structure *lpStructure)
{
}

void Stomach::StepSimulation(Simulator *lpSim, Structure *lpStructure)
{
	m_fltConsumptionRate = m_fltAdapterConsumptionRate + m_fltBaseConsumptionRate;
	m_fltAdapterConsumptionRate = 0;

	//We have the replenish rate in Quantity/s, but we need it in Quantity/timeslice. Lets recalculate it here.
	m_fltConsumptionForStep = (m_fltConsumptionRate * lpSim->PhysicsTimeStep());

	m_fltEnergyLevel -= m_fltConsumptionForStep;

	if(m_fltEnergyLevel < 0)
		m_fltEnergyLevel = 0;

	if(!m_bKilled && m_bKillOrganism && m_fltEnergyLevel == 0)
	{
		Organism *lpOrganism = dynamic_cast<Organism *>(lpStructure);
		lpOrganism->Kill(lpSim, TRUE);
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

void Stomach::AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput)
{
	m_fltAdapterConsumptionRate += fltInput;
	if(m_fltAdapterConsumptionRate < 0)
		m_fltAdapterConsumptionRate = 0;
}

void Stomach::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	oXml.IntoElem();  //Into RigidBody Element

	//There can only be one stomach per organism and its ID is hardcoded.
	ID("STOMACH");
	Name("Stomach");
	Type(oXml.GetChildString("Type", m_strType));

	m_fltMaxEnergyLevel = oXml.GetChildFloat("MaxEnergyLevel", m_fltMaxEnergyLevel);
	m_fltEnergyLevel = oXml.GetChildFloat("EnergyLevel", m_fltEnergyLevel);
	m_fltBaseConsumptionRate = oXml.GetChildFloat("BaseConsumptionRate", m_fltBaseConsumptionRate);
	m_bKillOrganism = oXml.GetChildBool("KillOrganism", TRUE);

	Std_IsAboveMin((float) 0, m_fltEnergyLevel, TRUE, "EnergyLevel");
	Std_IsAboveMin((float) 0, m_fltBaseConsumptionRate, TRUE, "BaseConsumptionRate");

	oXml.OutOfElem(); //OutOf RigidBody Element

	//This will add this object to the object list of the simulation.
	lpSim->AddToObjectList(this);
}

		}		//Joints
	}			//Environment
}				//AnimatSim
