/**
\file	Sensor.cpp

\brief	Implements the sensor class. 
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
	m_bUsesJoint = false;
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

void Sensor::Radius(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Sensor.Radius");
	if(bUseScaling)
		m_fltRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltRadius = fltVal;

	Resize();
}

void Sensor::LatitudeSegments(int iVal) 
{
	Std_IsAboveMin((int) 10, iVal, true, "Sensor.LatitudeSegments", true);
	m_iLatitudeSegments = iVal;
	Resize();
}

int Sensor::LatitudeSegments() {return m_iLatitudeSegments;}

void Sensor::LongtitudeSegments(int iVal)
{
	Std_IsAboveMin((int) 10, iVal, true, "Sensor.LongtitudeSegments", true);
	m_iLongtitudeSegments = iVal;
	Resize();
}

int Sensor::LongtitudeSegments() {return m_iLongtitudeSegments;}

bool Sensor::AllowRotateDragX() {return false;}

bool Sensor::AllowRotateDragY() {return false;}

bool Sensor::AllowRotateDragZ() {return false;}

bool Sensor::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
{
	std::string strType = Std_CheckString(strDataType);

	if(RigidBody::SetData(strType, strValue, false))
		return true;

	if(strType == "RADIUS")
	{
		Radius((float) atof(strValue.c_str()));
		return true;
	}

	if(strType == "LATITUDESEGMENTS")
	{
		LatitudeSegments(atoi(strValue.c_str()));
		return true;
	}

	if(strType == "LONGTITUDESEGMENTS")
	{
		LongtitudeSegments(atoi(strValue.c_str()));
		return true;
	}

	//If it was not one of those above then we have a problem.
	if(bThrowError)
		THROW_PARAM_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "Data Type", strDataType);

	return false;
}

void Sensor::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RigidBody::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Radius", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("LatitudeSegments", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("LongtitudeSegments", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
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
