/**
\file	Sphere.cpp

\brief	Implements the sphere class. 
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
	m_iLatitudeSegments = 50;
	m_iLongtitudeSegments = 50;
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

void Sphere::Radius(float fltVal, BOOL bUseScaling)
{
	Std_IsAboveMin((float) 0, fltVal, TRUE, "Sphere.Radius");
	if(bUseScaling)
		m_fltRadius = fltVal * m_lpSim->InverseDistanceUnits();
	else
		m_fltRadius = fltVal;

	Resize();
}

void Sphere::LatitudeSegments(int iVal) 
{
	Std_IsAboveMin((int) 10, iVal, TRUE, "Sphere.LatitudeSegments", TRUE);
	m_iLatitudeSegments = iVal;
	Resize();
}

int Sphere::LatitudeSegments() {return m_iLatitudeSegments;}

void Sphere::LongtitudeSegments(int iVal)
{
	Std_IsAboveMin((int) 10, iVal, TRUE, "Sphere.LongtitudeSegments", TRUE);
	m_iLongtitudeSegments = iVal;
	Resize();
}

int Sphere::LongtitudeSegments() {return m_iLongtitudeSegments;}

BOOL Sphere::SetData(string strDataType, string strValue, BOOL bThrowError)
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
