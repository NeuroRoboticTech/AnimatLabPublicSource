// Mouth.cpp: implementation of the Mouth class.
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

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

/*! \brief 
   Constructs a Mouth.
   		
   \param lpParent This is a pointer to the parent rigid body of this joint. 
   \param lpChild This is a pointer to the child rigid body of this joint. 

	 \return
	 No return value.

   \remarks
	 The constructor for a Mouth joint. 
*/

Mouth::Mouth()
{
	m_strID = "MOUTH";
	m_strName = "Mouth";
	m_lpStomach = NULL;
	m_fltEatingRate = 0;
	m_fltBiteSize = 2;
	m_fltMinFoodRadius = 10;
}


/*! \brief 
   Destroys the Mouth joint object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the Mouth joint object..	 
*/

Mouth::~Mouth()
{
	try
	{
		m_lpStomach = NULL;
	}
	catch(...)
	{Std_TraceMsg(0, "Caught Error in desctructor of Mouth\r\n", "", -1, FALSE, TRUE);}
}

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

	m_fltMinFoodRadius = oXml.GetChildFloat("MinimumFoodRadius", m_fltMinFoodRadius);
	m_fltMinFoodRadius *= m_lpSim->InverseDistanceUnits();

	Std_IsAboveMin((float) 0, m_fltMinFoodRadius, TRUE, "MinFoodRadius");

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Joints
	}			//Environment
}				//AnimatSim
