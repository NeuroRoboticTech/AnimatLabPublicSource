/**
\file	Mouth.cpp

\brief	Implements the mouth class. 
**/

#include "StdAfx.h"
#include "IMovableItemCallback.h"
#include "ISimGUICallback.h"
#include "AnimatBase.h"

#include "Node.h"
#include "IPhysicsMovableItem.h"
#include "IPhysicsBody.h"
#include "BoundingBox.h"
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
#include "Light.h"
#include "LightManager.h"
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
	m_lpStomach = NULL;
	m_fltEatingRate = 0;
	m_fltMinFoodRadius = 10;
	m_fltFoodDistance = 0;
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

float Mouth::MinFoodRadius() {return m_fltMinFoodRadius;}

void Mouth::MinFoodRadius(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Mouth.MinFoodRadius", TRUE);
	if(bUseScaling)
		m_fltMinFoodRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltMinFoodRadius = fltVal;
}

/**
\brief	Sets the Stomach identifier.

\author	dcofer
\date	6/12/2011

\param	strID	Identifier for the stomach.
**/
void Mouth::StomachID(string strID)
{
	SetStomachPointer(strID);
	m_strStomachID = strID;
}

/**
\brief	Gets the stomach identifier.

\author	dcofer
\date	6/12/2011

\return	ID.
**/
string Mouth::StomachID() {return m_strStomachID;}

/**
\brief	Sets the stomach pointer.

\author	dcofer
\date	6/12/2011

\param	strID	Identifier for the stomach.
**/
void Mouth::SetStomachPointer(string strID)
{
	if(Std_IsBlank(strID))
		m_lpStomach = NULL;
	else
	{
		m_lpStomach = dynamic_cast<Stomach *>(m_lpSim->FindByID(strID));
		if(!m_lpStomach)
			THROW_PARAM_ERROR(Al_Err_lPartTypeNotStomach, Al_Err_strPartTypeNotStomach, "ID", strID);
	}
}

void Mouth::Initialize()
{
	Sensor::Initialize();
	SetStomachPointer(m_strStomachID);
}

void Mouth::ResetSimulation()
{
	m_fltEatingRate = 0;
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
	Sensor::StepSimulation();

	if(m_lpStomach && m_fltEatingRate > 0)
	{
		//Now lets find the closest food source.
        CStdArray<RigidBody *> arySources;
        CStdArray<float> aryDistances;
		m_lpSim->FindClosestFoodSources(this->GetCurrentPosition(), m_fltMinFoodRadius, arySources, aryDistances);

        int iSources = arySources.GetSize();
		if(iSources > 0)
		{
            RigidBody *lpFood = NULL;
            float fltDist=0;
            for(int iFoodIdx=0; iFoodIdx<iSources; iFoodIdx++)
            {
                lpFood = arySources[iFoodIdx];
                fltDist = aryDistances[iFoodIdx];

			    float fltBiteSize = 0;

			    if(lpFood->FoodQuantity() >= m_fltEatingRate)
				    fltBiteSize = m_fltEatingRate;
			    else
				    fltBiteSize = lpFood->FoodQuantity();

			    float fltEnergy = fltBiteSize*lpFood->FoodEnergyContent();

			    if(fltEnergy > m_lpStomach->MaxEnergyLevel())
			    {
				    float fltNeededEnergy = m_lpStomach->MaxEnergyLevel() - m_lpStomach->EnergyLevel();
				    fltBiteSize = fltNeededEnergy/lpFood->FoodEnergyContent();
			    }

			    lpFood->Eat(fltBiteSize, m_lpSim->TimeSlice());
			    m_lpStomach->AddEnergy(fltEnergy);			
            }
		}
	}
}

BOOL Mouth::SetData(const string &strDataType, const string &strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(Sensor::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "STOMACHID")
	{
		StomachID(strValue);
		return TRUE;
	}

	if(strType == "MINIMUMFOODRADIUS")
	{
		MinFoodRadius(atof(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void Mouth::QueryProperties(CStdArray<string> &aryNames, CStdArray<string> &aryTypes)
{
	Sensor::QueryProperties(aryNames, aryTypes);

	aryNames.Add("StomachID");
	aryTypes.Add("String");

	aryNames.Add("MinimumFoodRadius");
	aryTypes.Add("Float");
}

float *Mouth::GetDataPointer(const string &strDataType)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "EATINGRATE")
		return &m_fltEatingRate;

	if(strType == "FOODDISTANCE")
		return &m_fltFoodDistance;

	return RigidBody::GetDataPointer(strDataType);
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

	MinFoodRadius(oXml.GetChildFloat("MinimumFoodRadius", m_fltMinFoodRadius));
	m_strStomachID = oXml.GetChildString("StomachID", "");

	oXml.OutOfElem(); //OutOf RigidBody Element
}

		}		//Joints
	}			//Environment
}				//AnimatSim
