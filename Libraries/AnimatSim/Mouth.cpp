/**
\file	Mouth.cpp

\brief	Implements the mouth class. 
**/

#include "stdafx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsBase.h"
#include "IPhysicsBody.h"
#include "MovableItem.h"
#include "BodyPart.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Sensor.h"
#include "Stomach.h"
#include "Mouth.h"
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
Mouth::Mouth()
{
	m_strID = "MOUTH";
	m_strName = "Mouth";
	m_lpStomach = NULL;
	m_fltEatingRate = 0;
	m_fltMinFoodRadius = 10;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/10/2011
**/
Mouth::~Mouth()
{
	try
	{
		m_lpStomach = NULL;
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of Mouth\r\n", "", -1, FALSE, TRUE);}
}

/**
\brief	Gets the eating rate. 

\author	dcofer
\date	3/10/2011

\return	Eating rate. 
**/
float Mouth::EatingRate() {return m_fltEatingRate;}

float Mouth::MinFoodRadius() {return m_fltRadius;}

void Mouth::MinFoodRadius(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Mouth.MinFoodRadius", TRUE);
	if(bUseScaling)
		m_fltRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltRadius = fltVal;
}

/**
\brief	Step the simulation.

\details At each time step the mouth first tries to find the food source that is closes to the mouth
and is within its radius for being able to eat from that source. If it finds a source then it determines
how big of a bite it can take of the food in this step and then calculates the energy it will get from that
bite of food. It then reduces the food source quantity by the bite amount and increases the stomach energy
content by the new amount.


\author	dcofer
\date	3/10/2011
**/
void Mouth::StepSimulation()
{
	if(m_lpStomach && m_fltEatingRate > 0)
	{
		//Now lets find the closest food source.
		RigidBody *lpFood = m_lpSim->FindClosestFoodSource(this->GetCurrentPosition(), m_fltMinFoodRadius);

		if(lpFood)
		{
			float fltBiteSize = 0;

			if(lpFood->FoodQuantity() >= m_fltEatingRate)
				fltBiteSize = m_fltEatingRate;
			else
				fltBiteSize = lpFood->FoodQuantity();

			float fltEnergy = fltBiteSize*lpFood->FoodEnergyContent();
			fltEnergy += m_lpStomach->EnergyLevel(); //Find new total energy level.

			if(fltEnergy > m_lpStomach->MaxEnergyLevel())
			{
				float fltNeededEnergy = m_lpStomach->MaxEnergyLevel() - m_lpStomach->EnergyLevel();
				fltBiteSize = fltNeededEnergy/lpFood->FoodEnergyContent();
			}

			float fltFoodQty = lpFood->FoodQuantity() - fltBiteSize;

			lpFood->Eat(fltFoodQty, m_lpSim->TimeSlice());
			m_lpStomach->EnergyLevel(fltEnergy);			
		}
	}
}

float *Mouth::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);
	float *lpData = NULL;

	if(strType == "EATINGRATE")
		return &m_fltEatingRate;

	lpData = RigidBody::GetDataPointer(strDataType);

	return lpData;
}

void Mouth::AddExternalNodeInput(float fltInput)
{
	m_fltEatingRate = fltInput;
	if(m_fltEatingRate < 0)
		m_fltEatingRate = 0;
}

void Mouth::Load(CStdXml &oXml)
{
	Sensor::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element

	//Override the ID values.
	//There can only be one mouth per organism and its ID is hardcoded.
	m_strID = "MOUTH";
	m_strName = "Mouth";

	MinFoodRadius(oXml.GetChildFloat("MinimumFoodRadius", m_fltMinFoodRadius));

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Joints
	}			//Environment
}				//AnimatSim
