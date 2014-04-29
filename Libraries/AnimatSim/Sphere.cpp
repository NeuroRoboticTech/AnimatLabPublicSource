/**
\file	Sphere.cpp

\brief	Implements the sphere class. 
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
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Sphere.h"
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
Sphere::Sphere()
{
	m_fltRadius = 1;
	m_iLatitudeSegments = 20;
	m_iLongtitudeSegments = 20;
}

/**
\brief	Destructor. 

\author	dcofer
\date	3/10/2011
**/
Sphere::~Sphere()
{

}

float Sphere::Radius() {return m_fltRadius;}

void Sphere::Radius(float fltVal, bool bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, true, "Sphere.Radius");
	if(bUseScaling)
		m_fltRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltRadius = fltVal;

	Resize();
}

/**
\brief	Latitude segments.

\author	dcofer
\date	7/11/2011

\param	iVal	The new value.
**/
void Sphere::LatitudeSegments(int iVal) 
{
	Std_IsAboveMin((int) 10, iVal, true, "Sphere.LatitudeSegments", true);
	m_iLatitudeSegments = iVal;
	Resize();
}

/**
\brief	Gets the latitude segments.

\author	dcofer
\date	7/11/2011

\return	segments.
**/
int Sphere::LatitudeSegments() {return m_iLatitudeSegments;}

/**
\brief	Longtitude segments.

\author	dcofer
\date	7/11/2011

\param	iVal	The new value.
**/
void Sphere::LongtitudeSegments(int iVal)
{
	Std_IsAboveMin((int) 10, iVal, true, "Sphere.LongtitudeSegments", true);
	m_iLongtitudeSegments = iVal;
	Resize();
}

/**
\brief	Gets the longtitude segments.

\author	dcofer
\date	7/11/2011

\return	segments.
**/
int Sphere::LongtitudeSegments() {return m_iLongtitudeSegments;}

bool Sphere::SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError)
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

void Sphere::QueryProperties(CStdPtrArray<TypeProperty> &aryProperties)
{
	RigidBody::QueryProperties(aryProperties);

	aryProperties.Add(new TypeProperty("Radius", AnimatPropertyType::Float, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("LatitudeSegments", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
	aryProperties.Add(new TypeProperty("LongtitudeSegments", AnimatPropertyType::Integer, AnimatPropertyDirection::Set));
}

void Sphere::Load(CStdXml &oXml)
{
	RigidBody::Load(oXml);

	oXml.IntoElem();  //Into RigidBody Element
	Radius(oXml.GetChildFloat("Radius", m_fltRadius));
	LatitudeSegments(oXml.GetChildInt("LatitudeSegments", m_iLatitudeSegments));
	LongtitudeSegments(oXml.GetChildInt("LongtitudeSegments", m_iLongtitudeSegments));
	oXml.OutOfElem(); //OutOf RigidBody Element
}


		}		//Bodies
	}			//Environment
}				//AnimatSim
