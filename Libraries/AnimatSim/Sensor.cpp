/**
\file	Sensor.cpp

\brief	Implements the sensor class. 
**/

#include "stdafx.h"
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
Sensor::Sensor()
{
	m_fltDensity = 0;
	m_bUsesJoint = FALSE;
	m_lpJointToParent = NULL;
	m_fltRadius = 1;
	m_iLatitudeSegments = 50;
	m_iLongtitudeSegments = 50;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/10/2011
**/
Sensor::~Sensor()
{

}

float Sensor::Radius() {return m_fltRadius;}

void Sensor::Radius(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Sensor.Radius");
	if(bUseScaling)
		m_fltRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltRadius = fltVal;

	Resize();
}

void Sensor::LatitudeSegments(int iVal) 
{
	Std_IsAboveMin((int) 10, iVal, TRUE, "Sensor.LatitudeSegments", TRUE);
	m_iLatitudeSegments = iVal;
	Resize();
}

int Sensor::LatitudeSegments() {return m_iLatitudeSegments;}

void Sensor::LongtitudeSegments(int iVal)
{
	Std_IsAboveMin((int) 10, iVal, TRUE, "Sensor.LongtitudeSegments", TRUE);
	m_iLongtitudeSegments = iVal;
	Resize();
}

int Sensor::LongtitudeSegments() {return m_iLongtitudeSegments;}

BOOL Sensor::AllowRotateDragX() {return FALSE;}

BOOL Sensor::AllowRotateDragY() {return FALSE;}

BOOL Sensor::AllowRotateDragZ() {return FALSE;}

BOOL Sensor::SetData(string strDataType, string strValue, BOOL bThrowError)
{
	string strType = Std_CheckString(strDataType);

	if(RigidBody::SetData(strType, strValue, FALSE))
		return TRUE;

	if(strType == "RADIUS")
	{
		Radius(atof(strValue.c_str()));
		return TRUE;
	}

	if(strType == "LATITUDESEGMENTS")
	{
		LatitudeSegments(atoi(strValue.c_str()));
		return TRUE;
	}

	if(strType == "LONGTITUDESEGMENTS")
	{
		LongtitudeSegments(atoi(strValue.c_str()));
		return TRUE;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return FALSE;
}

void Sensor::Initialize() 
{}

// There are no parts or joints to create for muscle attachment points.
void Sensor::CreateParts()
{}

void Sensor::CreateJoints()
{}

void Sensor::Load(CStdXml &oXml)
{
	RigidBody::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element
	Radius(oXml.GetChildFloat("Radius", m_fltRadius));
	LatitudeSegments(oXml.GetChildInt("LatitudeSegments", m_iLatitudeSegments));
	LongtitudeSegments(oXml.GetChildInt("LongtitudeSegments", m_iLongtitudeSegments));
	oXml.OutOfElem(); //OutOf RigidBody Element

	//Reset the rotation to 0 for sensors
	m_oRotation.Set(0, 0, 0);
}

		}		//Bodies
	}			//Environment
}				//AnimatSim
